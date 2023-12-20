namespace Mess.Blazor.Abstractions.Components;

// TODO: get ISite, ViewContext, ThemeLayout, and other stuff in here
// TODO: mimic properties from RazorPageBase, RazorPage, RazorePage<T>, and
// OrchardCore RazorPage

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
