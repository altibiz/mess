namespace Mess.Blazor.Abstractions.Components;

public class ComponentCapture
{
  public object? Model { get; set; }
}

public interface IComponentCaptureStore
{
  void Add(Guid renderId, ComponentCapture? capture);

  ComponentCapture? Get(Guid renderId, string? circuitId);

  void Remove(string circuitId);
}
