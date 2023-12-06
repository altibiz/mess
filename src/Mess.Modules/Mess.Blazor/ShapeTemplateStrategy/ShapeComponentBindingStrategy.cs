using System.Collections.Concurrent;
using Mess.Blazor.Abstractions;
using Mess.Blazor.Abstractions.ShapeTemplateStrategy;
using Mess.Prelude.Extensions.Enumerables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;

namespace Mess.Blazor.ShapeTemplateStrategy;

public class ShapeComponentBindingStrategy : IShapeTableHarvester
{
  private readonly IEnumerable<IShapeComponentHarvester> _harvesters;
  private readonly ILogger _logger;

  private readonly IEnumerable<IShapeTemplateComponentEngine>
    _shapeTemplateComponentEngines;

  private readonly IShellFeaturesManager _shellFeaturesManager;

  private readonly Dictionary<Type, IShapeTemplateComponentEngine>
    _viewEnginesByBaseClass = new();

  public ShapeComponentBindingStrategy(
    IEnumerable<IShapeComponentHarvester> harvesters,
    IShellFeaturesManager shellFeaturesManager,
    IEnumerable<IShapeTemplateComponentEngine> shapeTemplateComponentEngines,
    ILogger<ShapeComponentBindingStrategy> logger
  )
  {
    _harvesters = harvesters;
    _shellFeaturesManager = shellFeaturesManager;
    _shapeTemplateComponentEngines = shapeTemplateComponentEngines;
    _logger = logger;
  }

  public void Discover(ShapeTableBuilder builder)
  {
    if (_logger.IsEnabled(LogLevel.Information))
      _logger.LogInformation("Start discovering shapes");

    var harvesterInfos = _harvesters
      .Select(harvester => new
      {
        harvester, subNamespaces = harvester.SubNamespaces(),
        baseClasses = harvester.BaseClasses()
      })
      .ToList();

    var enabledFeatures = _shellFeaturesManager.GetEnabledFeaturesAsync()
      .GetAwaiter().GetResult();
    var enabledFeatureIds = enabledFeatures.Select(f => f.Id).ToArray();

    // Excludes the extensions whose templates are already associated to an excluded feature that is still enabled.
    var activeExtensions = Once(enabledFeatures)
      .Where(e => !e.Features.Any(f =>
        builder.ExcludedFeatureIds.Contains(f.Id) &&
        enabledFeatureIds.Contains(f.Id)))
      .ToArray();

    if (!_viewEnginesByBaseClass.Any())
      foreach (var viewEngine in _shapeTemplateComponentEngines)
      foreach (var baseClass in viewEngine.TemplateBaseClasses)
        if (!_viewEnginesByBaseClass.ContainsKey(baseClass))
          _viewEnginesByBaseClass[baseClass] = viewEngine;

    var hits = activeExtensions.Select(extensionDescriptor =>
    {
      if (_logger.IsEnabled(LogLevel.Information))
        _logger.LogInformation("Start discovering candidate component types");

      var pathContexts = harvesterInfos.SelectMany(harvesterInfo =>
          harvesterInfo.subNamespaces.Select(subNamespace =>
          {
            var extensionAssembly = AppDomain.CurrentDomain.GetAssemblies()
              .FirstOrDefault(assembly => assembly
                .GetCustomAttributes(false)
                .Where(attribute =>
                  attribute is OrchardCore.Modules.Manifest.ModuleAttribute moduleAttribute &&
                  moduleAttribute.Id == extensionDescriptor.Manifest.ModuleInfo.Id)
                .Any());
            if (extensionAssembly is null) {
              return null;
            }

            var types = extensionAssembly
              .ExportedTypes
              .Where(type => type.Name != "_Imports")
              .Select(type =>
                type.Namespace is not null &&
                type.Namespace ==
                $"{extensionDescriptor.Manifest.ModuleInfo.Id}.{subNamespace}" &&
                harvesterInfo.baseClasses.Contains(type.BaseType)
                  ? type
                  : null)
              .WhereNotNull();

            return new { harvesterInfo.harvester, subNamespace, types };
          }))
        .WhereNotNull()
        .ToList();

      if (_logger.IsEnabled(LogLevel.Information))
        _logger.LogInformation("Done discovering candidate component types");

      var fileContexts = pathContexts.SelectMany(namespaceContext =>
        _shapeTemplateComponentEngines.SelectMany(ve =>
        {
          return namespaceContext.types.Select(
            type => new
            {
              type,
              relativeTypePath =
                $"{namespaceContext.subNamespace}.{type.Name}",
              namespaceContext
            });
        }));

      var shapeContexts = fileContexts.SelectMany(typeContext =>
      {
        var harvestShapeInfo = new HarvestShapeComponentInfo
        {
          SubNamespace = typeContext.namespaceContext.subNamespace,
          Type = typeContext.type,
          RelativeTypePath = typeContext.relativeTypePath,
          BaseClass = typeContext.type.BaseType!
        };
        var harvestShapeHits =
          typeContext.namespaceContext.harvester.HarvestShape(harvestShapeInfo);
        return harvestShapeHits.Select(harvestShapeHit =>
          new { harvestShapeInfo, harvestShapeHit, typeContext });
      });

      return shapeContexts
        .Select(shapeContext => new { extensionDescriptor, shapeContext })
        .ToList();
    }).SelectMany(hits2 => hits2);

    foreach (var iter in hits)
    {
      var hit = iter;

      // The template files of an active module need to be associated to one of its enabled feature.
      var feature =
        hit.extensionDescriptor.Features.First(f =>
          enabledFeatureIds.Contains(f.Id));

      if (_logger.IsEnabled(LogLevel.Debug))
        _logger.LogDebug(
          "Binding '{RelativeTypePath}' as shape '{ShapeType}' for feature '{FeatureName}'",
          hit.shapeContext.harvestShapeInfo.RelativeTypePath,
          iter.shapeContext.harvestShapeHit.ShapeType,
          feature.Id
        );

      var viewEngineType = _viewEnginesByBaseClass[
        iter.shapeContext.harvestShapeInfo.BaseClass
      ].GetType();

      builder.Describe(iter.shapeContext.harvestShapeHit.ShapeType)
        .From(feature)
        .BoundAs(
          hit.shapeContext.harvestShapeInfo.RelativeTypePath,
          displayContext =>
          {
            var viewEngine = displayContext.ServiceProvider
              .GetServices<IShapeTemplateComponentEngine>()
              .FirstOrDefault(e => e.GetType() == viewEngineType)!;

            return viewEngine.RenderAsync(
              hit.shapeContext.harvestShapeInfo.Type,
              displayContext
            );
          });
    }

    if (_logger.IsEnabled(LogLevel.Information))
      _logger.LogInformation("Done discovering shapes");
  }

  private static IEnumerable<IExtensionInfo> Once(
    IEnumerable<IFeatureInfo> featureDescriptors)
  {
    var once = new ConcurrentDictionary<string, object?>();
    return featureDescriptors.Select(x => x.Extension)
      .Where(ed => once.TryAdd(ed.Id, null)).ToList();
  }
}
