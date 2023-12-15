using System.Collections.Concurrent;
using Mess.Blazor.Abstractions.Components;
using Mess.Blazor.Abstractions.ShapeTemplateStrategy;
using Mess.Prelude.Extensions.Enumerables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.DisplayManagement.Descriptors.ShapeTemplateStrategy;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules.Manifest;

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

  private record class HarvesterData(
    IShapeComponentHarvester Harvester,
    string[] SubNamespaces,
    Type[] BaseClasses
  );

  private record class HarvesterSubNamespaceData(
    IShapeComponentHarvester Harvester,
    string SubNamespace,
    Type[] Types
  );

  private record class HarvesterTypeData(
    IShapeComponentHarvester Harvester,
    Type Type,
    string RelativeTypePath,
    string SubNamespace
  );

  private record class HarvesterShapeData(
    IExtensionInfo Extension,
    IShapeComponentHarvester Harvester,
    HarvestShapeComponentInfo Component,
    HarvestShapeHit Hit,
    Type Type
  );

  public void Discover(ShapeTableBuilder builder)
  {
    if (_logger.IsEnabled(LogLevel.Information))
      _logger.LogInformation("Start discovering shapes");

    if (!_viewEnginesByBaseClass.Any())
      foreach (var viewEngine in _shapeTemplateComponentEngines)
        foreach (var baseClass in viewEngine.TemplateBaseClasses)
          if (!_viewEnginesByBaseClass.ContainsKey(baseClass))
            _viewEnginesByBaseClass[baseClass] = viewEngine;

    var harvesters = _harvesters
      .Select(harvester => new HarvesterData(
        harvester,
        harvester.SubNamespaces().ToArray(),
        harvester.BaseClasses().ToArray()
      ))
      .ToList();

    var enabledFeatures = _shellFeaturesManager
      .GetEnabledFeaturesAsync()
      .GetAwaiter()
      .GetResult();

    var enabledFeatureIds = enabledFeatures
      .Select(feature => feature.Id)
      .ToArray();

    var activeExtensions = enabledFeatures
      .Select(feature => feature.Extension)
      .DistinctBy(extension => extension.Id)
      .Where(extension => !extension.Features
        .Any(feature => builder.ExcludedFeatureIds
          .Contains(feature.Id)))
      .ToArray();

    var shapes = activeExtensions
      .SelectMany(extension =>
      {
        _logger.LogInformation("Start discovering candidate component types");

        var subnamespaces = harvesters
          .SelectMany(harvester => harvester.SubNamespaces
            .Select(subNamespace =>
            {
              var extensionAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly
                  .GetCustomAttributes(false)
                  .Where(attribute =>
                    attribute is ModuleAttribute moduleAttribute &&
                    moduleAttribute.Id ==
                    extension.Manifest.ModuleInfo.Id)
                  .Any());
              if (extensionAssembly is null) return null;

              var types = extensionAssembly
                .ExportedTypes
                .Where(type => type.Name != "_Imports")
                .Select(type =>
                  type.Namespace is not null &&
                  type.Namespace ==
                  $"{extension.Manifest.ModuleInfo.Id}.{subNamespace}" &&
                  harvester.BaseClasses.Any(
                    baseClass =>
                      baseClass == type.BaseType ||
                      (type.BaseType?.IsGenericType is true &&
                      baseClass == type.BaseType?.GetGenericTypeDefinition()))
                    ? type
                    : null)
                .WhereNotNull();

              return new HarvesterSubNamespaceData(
                harvester.Harvester,
                subNamespace,
                types.ToArray()
              );
            }))
          .WhereNotNull()
          .ToList();

        _logger.LogInformation("Done discovering candidate component types");

        var types = subnamespaces
          .SelectMany(subnamespace => _shapeTemplateComponentEngines
            .SelectMany(engine =>
            {
              return subnamespace.Types.Select(type => new HarvesterTypeData(
                subnamespace.Harvester,
                type,
                $"{subnamespace.SubNamespace}.{type.Name}",
                subnamespace.SubNamespace
              ));
            }));

        var shapes = types
          .SelectMany(type =>
          {
            var component = new HarvestShapeComponentInfo
            {
              SubNamespace = type.SubNamespace,
              Type = type.Type,
              RelativeTypePath = type.RelativeTypePath,
              BaseClass = type.Type.BaseType is { IsGenericType: true }
                ? type.Type.BaseType.GetGenericTypeDefinition()
                : type.Type.BaseType!
            };

            var harvestShapeHits =
              type.Harvester.HarvestShape(component);

            return harvestShapeHits
              .Select(hit => new HarvesterShapeData(
                extension,
                type.Harvester,
                component,
                hit,
                type.Type
              ));
          });

        return shapes;
      });

    foreach (var shape in shapes)
    {
      var feature =
        shape.Extension.Features.First(feature =>
          enabledFeatureIds.Contains(feature.Id));

      _logger.LogDebug(
        "Binding '{RelativeTypePath}' as shape '{ShapeType}' for feature '{FeatureName}'",
        shape.Component.RelativeTypePath,
        shape.Hit.ShapeType,
        feature.Id
      );

      var viewEngineType = _viewEnginesByBaseClass[
        shape.Component.BaseClass
      ].GetType();

      builder.Describe(shape.Hit.ShapeType)
        .From(feature)
        .BoundAs(
          shape.Component.RelativeTypePath,
          displayContext =>
          {
            var viewEngine = displayContext.ServiceProvider
              .GetServices<IShapeTemplateComponentEngine>()
              .FirstOrDefault(engine =>
                engine.GetType() == viewEngineType)!;

            return viewEngine.RenderAsync(
              shape.Component.Type,
              displayContext
            );
          });
    }

    _logger.LogInformation("Done discovering shapes");
  }
}
