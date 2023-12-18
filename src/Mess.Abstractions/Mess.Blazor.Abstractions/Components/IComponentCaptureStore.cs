namespace Mess.Blazor.Abstractions.Components;

public class ComponentCapture
{
  public object? Model { get; set; }
}

public interface IComponentCaptureStore
{
  void Add(Guid captureId, ComponentCapture? capture);

  ComponentCapture? Get(Guid captureId, string? circuitId);

  void Remove(string circuitId);
}
