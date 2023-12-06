using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Mvc.FileProviders;
using OrchardCore.DisplayManagement.Descriptors.ShapeTemplateStrategy;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.DisplayManagement;
using Mess.Blazor.Abstractions;

namespace Mess.Blazor;

public class ShapeComponentBindingStrategy : IShapeTableHarvester
{
  private readonly IEnumerable<IShapeComponentHarvester> _harvesters;
  private readonly IEnumerable<IShapeTemplateComponentEngine> _shapeTemplateComponentEngines;
  private readonly ILogger _logger;
  private readonly IShellFeaturesManager _shellFeaturesManager;
  private readonly Dictionary<Type, IShapeTemplateComponentEngine> _viewEnginesByBaseClass = new();

  public ShapeComponentBindingStrategy(
    IEnumerable<IShapeComponentHarvester> harvesters,
    IShellFeaturesManager shellFeaturesManager,
    IEnumerable<IShapeTemplateComponentEngine> shapeTemplateComponentEngines,
    ILogger<ShapeComponentBindingStrategy> logger
  ) {
    _harvesters = harvesters;
    _shellFeaturesManager = shellFeaturesManager;
    _shapeTemplateComponentEngines = shapeTemplateComponentEngines;
    _logger = logger;
  }

  private static IEnumerable<IExtensionInfo> Once(IEnumerable<IFeatureInfo> featureDescriptors)
  {
      var once = new ConcurrentDictionary<string, object?>();
      return featureDescriptors.Select(x => x.Extension).Where(ed => once.TryAdd(ed.Id, null)).ToList();
  }

  public void Discover(ShapeTableBuilder builder)
  {
    if (_logger.IsEnabled(LogLevel.Information))
    {
        _logger.LogInformation("Start discovering shapes");
    }

    var harvesterInfos = _harvesters
        .Select(harvester => new { harvester, subNamespaces = harvester.SubNamespaces() })
        .ToList();

    var enabledFeatures = _shellFeaturesManager.GetEnabledFeaturesAsync().GetAwaiter().GetResult();
    var enabledFeatureIds = enabledFeatures.Select(f => f.Id).ToArray();

    // Excludes the extensions whose templates are already associated to an excluded feature that is still enabled.
    var activeExtensions = Once(enabledFeatures)
      .Where(e => !e.Features.Any(f => builder.ExcludedFeatureIds.Contains(f.Id) && enabledFeatureIds.Contains(f.Id)))
      .ToArray();

    if (!_viewEnginesByBaseClass.Any())
    {
      foreach (var viewEngine in _shapeTemplateComponentEngines)
      {
        foreach (var baseClass in viewEngine.TemplateBaseClasses)
        {
          if (!_viewEnginesByBaseClass.ContainsKey(baseClass))
          {
            _viewEnginesByBaseClass[baseClass] = viewEngine;
          }
        }
      }
    }

    var hits = activeExtensions.Select(extensionDescriptor =>
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Start discovering candidate component types");
        }

        var pathContexts = harvesterInfos.SelectMany(harvesterInfo => harvesterInfo.subNamespaces.Select(subNamespace =>
        {
            var extensionAssembly = AppDomain.CurrentDomain.GetAssemblies()
              .FirstOrDefault(assembly => assembly
                .GetCustomAttributes(false)
                .Where(attribute => attribute == extensionDescriptor.Manifest.ModuleInfo)
                .Any())!;
            var types = extensionAssembly
              .ExportedTypes
              .Where(type =>
                extensionAssembly.FullName is not null &&
                type.Namespace is not null &&
                type.Namespace == $"{extensionAssembly.FullName}.{subNamespace}");

            return new { harvesterInfo.harvester, subNamespace, types };
        }))
        .ToList();

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Done discovering candidate component types");
        }

        var fileContexts = pathContexts.SelectMany(pathContext => _shapeTemplateComponentEngines.SelectMany(ve =>
        {
            return pathContext.types.Select(
                type => new
                {
                    type,
                    relativeTypePath = filePath,
                    pathContext
                });
        }));

        var shapeContexts = fileContexts.SelectMany(fileContext =>
        {
            var harvestShapeInfo = new HarvestShapeInfo
            {
                SubPath = fileContext.pathContext.subPath,
                FileName = fileContext.fileName,
                RelativePath = fileContext.relativePath,
                Extension = Path.GetExtension(fileContext.relativePath)
            };
            var harvestShapeHits = fileContext.pathContext.harvester.HarvestShape(harvestShapeInfo);
            return harvestShapeHits.Select(harvestShapeHit => new { harvestShapeInfo, harvestShapeHit, fileContext });
        });

        return shapeContexts.Select(shapeContext => new { extensionDescriptor, shapeContext }).ToList();
    }).SelectMany(hits2 => hits2);

    foreach (var iter in hits)
    {
        var hit = iter;

        // The template files of an active module need to be associated to one of its enabled feature.
        var feature = hit.extensionDescriptor.Features.First(f => enabledFeatureIds.Contains(f.Id));

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Binding '{TemplatePath}' as shape '{ShapeType}' for feature '{FeatureName}'",
                hit.shapeContext.harvestShapeInfo.RelativePath,
                iter.shapeContext.harvestShapeHit.ShapeType,
                feature.Id);
        }

        var viewEngineType = _viewEnginesByBaseClass[iter.shapeContext.harvestShapeInfo.Extension].GetType();

        builder.Describe(iter.shapeContext.harvestShapeHit.ShapeType)
            .From(feature)
            .BoundAs(
                hit.shapeContext.harvestShapeInfo.RelativePath, displayContext =>
                {
                    var viewEngine = displayContext.ServiceProvider
                        .GetServices<IShapeTemplateViewEngine>()
                        .FirstOrDefault(e => e.GetType() == viewEngineType);

                    return viewEngine.RenderAsync(hit.shapeContext.harvestShapeInfo.RelativePath, displayContext);
                });
    }

    if (_logger.IsEnabled(LogLevel.Information))
    {
        _logger.LogInformation("Done discovering shapes");
    }
  }
}
