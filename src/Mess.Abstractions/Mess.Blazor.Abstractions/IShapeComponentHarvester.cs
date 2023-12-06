using OrchardCore.DisplayManagement.Descriptors.ShapeTemplateStrategy;

namespace Mess.Blazor.Abstractions;

public interface IShapeComponentHarvester
{
  IEnumerable<string> SubNamespaces();

  IEnumerable<HarvestShapeHit> HarvestShape(HarvestShapeComponentInfo info);
}
