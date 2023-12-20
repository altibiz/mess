using System.Collections.Concurrent;
using System.Reflection;
using Mess.Blazor.Abstractions.Components;
using Mess.Prelude.Extensions.Enumerables;
using Microsoft.AspNetCore.Components;
using OrchardCore.DisplayManagement.Manifest;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules.Manifest;

// TODO: optimize all this
// TODO: find layout in the right theme - use theme manager?
// TODO: remove disabled extensions like for shapes?

namespace Mess.Blazor.Abstractions.Extensions;

public static class ShapeComponentBaseAppExtensions
{
  public static Assembly GetAppAssembly(this ShapeComponentBase component)
  {
    return component.GetType().Assembly;
  }

  public static Assembly[] GetExtensionAssemblies(this ShapeComponentBase component)
  {
    return component
      .GetExtensionAssemblyDataCached()
      .Select(data => data.Assembly)
      .ToArray();
  }

  public static Type GetLayoutType(this ShapeComponentBase component)
  {
    var data = component
      .GetExtensionTypeDataCached()
      .FirstOrDefault(data =>
        data.Extension.Manifest.ModuleInfo is ThemeAttribute &&
        data.Type.Name == "Layout" &&
        data.Type.BaseType == typeof(LayoutShapeComponentBase)
      ) ?? throw new InvalidOperationException("Couldn't find layout");

    return data.Type;
  }

  private static readonly ConcurrentDictionary<Type, List<ExtensionTypeData>> _extensionTypeDataCache = new();

  private record class ExtensionTypeData(
    IExtensionInfo Extension,
    Type Type
  );

  private static IEnumerable<ExtensionTypeData> GetExtensionTypeDataCached(
    this ShapeComponentBase component)
  {
    return _extensionTypeDataCache.GetOrAdd(
        component.GetType(),
        _ => component.GetExtensionTypeData().ToList()
      );
  }

  private static IEnumerable<ExtensionTypeData> GetExtensionTypeData(this ShapeComponentBase component)
  {
    return component.GetExtensionAssemblyData()
      .SelectMany(data => data.Assembly.GetTypes()
        .Select(type => new ExtensionTypeData(data.Extension, type)));
  }

  private static readonly ConcurrentDictionary<Type, List<ExtensionAssemblyData>> _extensionAssemblyDataCache = new();

  private static IEnumerable<ExtensionAssemblyData> GetExtensionAssemblyDataCached(
    this ShapeComponentBase component)
  {
    return _extensionAssemblyDataCache.GetOrAdd(
        component.GetType(),
        _ => component.GetExtensionAssemblyData().ToList()
      );
  }

  private record class ExtensionAssemblyData(
    IExtensionInfo Extension,
    Assembly Assembly
  );

  private static IEnumerable<ExtensionAssemblyData> GetExtensionAssemblyData(
    this ShapeComponentBase component)
  {
    var enabledFeatures = component.ServiceProvider
      .GetRequiredService<IShellFeaturesManager>()
      .GetEnabledFeaturesAsync()
      .GetAwaiter()
      .GetResult();

    var enabledFeatureIds = enabledFeatures
      .Select(feature => feature.Id)
      .ToArray();

    var activeExtensions = enabledFeatures
      .Select(feature => feature.Extension)
      .DistinctBy(extension => extension.Id)
      .ToArray();

    var extensionAssemblies = activeExtensions
      .Select(extension =>
      {
        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(assembly => assembly
              .GetCustomAttributes(false)
              .Where(attribute =>
                attribute is ModuleAttribute moduleAttribute &&
                moduleAttribute.Id ==
                extension.Manifest.ModuleInfo.Id)
              .Any());

        if (assembly is null)
        {
          return null;
        }

        return new ExtensionAssemblyData(extension, assembly);

      })
      .WhereNotNull();

    return extensionAssemblies;
  }
}
