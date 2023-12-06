using System.Reflection.Metadata;

namespace Mess.Blazor.Abstractions.ShapeTemplateStrategy;

public class HarvestShapeComponentInfo
{
  public string SubNamespace { get; set; } = default!;

  public Type Type { get; set; } = default!;

  public string RelativeTypePath { get; set; } = default!;

  public Type BaseClass { get; set; } = default!;
}
