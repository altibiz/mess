using System.Threading.Tasks;
using Mess.Blazor.Abstractions.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Mess.Blazor.Abstractions.Components;

public partial class AppShapeComponentBase : AbstractComponent
{
  [Inject]
  private ICircuitAccessor CircuitAccessor { get; set; } = default!;

  [Inject]
  private IComponentCaptureStore ComponentCaptureStore { get; set; } = default!;

  [Inject]
  private IAppQueryExecutor AppQueryExecutor { get; set; } = default!;

  private dynamic? _model;

  [Parameter] public Guid? CaptureId { get; set; }

  /// <summary>
  ///   Gets the <see cref="TModel" /> instance.
  /// </summary>
  protected dynamic Model
  {
    get
    {
      var captureId = CaptureId
        ?? throw new InvalidOperationException("CaptureId is null");

      var circuitId = CircuitAccessor.Circuit?.Id;

      var capture = ComponentCaptureStore.Get(captureId, circuitId);

      var model = capture?.Model as dynamic
        ?? throw new InvalidOperationException("App model is invalid");

      return _model ??= model;
    }
  }

  protected Guid LayoutCaptureId => Model.CaptureId ?? CaptureId
    ?? throw new InvalidOperationException("CaptureId is null");

  protected override void OnAfterRender(bool firstRender)
  {
    OnAfterAppRender();
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    await AppQueryExecutor.ExecuteAsync();
    await OnAfterAppRenderAsync();
  }

  protected virtual void OnAfterAppRender() { }

  protected virtual async Task OnAfterAppRenderAsync() { }
}
