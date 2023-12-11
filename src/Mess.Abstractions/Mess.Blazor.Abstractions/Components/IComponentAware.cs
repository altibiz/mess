namespace Mess.Blazor.Abstractions.Components;

public interface IComponentAware
{
  void Contextualize(Type componentType);
}
