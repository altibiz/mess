using OrchardCore.DisplayManagement.Descriptors.ShapeTemplateStrategy;

namespace Mess.Blazor.Abstractions.ShapeTemplateStrategy;

public interface IShapeComponentHarvester
{
  IEnumerable<string> SubNamespaces();

  IEnumerable<Type> BaseClasses();

  IEnumerable<HarvestShapeHit> HarvestShape(HarvestShapeComponentInfo info);
}
