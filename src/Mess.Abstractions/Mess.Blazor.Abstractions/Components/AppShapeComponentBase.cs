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

  private AppViewModel? _model;

  [Parameter] public Guid? CaptureId { get; set; }

  /// <summary>
  ///   Gets the <see cref="TModel" /> instance.
  /// </summary>
  protected AppViewModel Model
  {
    get
    {
      var captureId = CaptureId
        ?? throw new InvalidOperationException("CaptureId is null");

      var circuitId = CircuitAccessor.Circuit?.Id;

      var capture = ComponentCaptureStore.Get(captureId, circuitId);

      var model = capture?.Model as AppViewModel
        ?? throw new InvalidOperationException("App model is invalid");

      return _model ??= model;
    }
  }

  protected override void OnAfterRender(bool firstRender)
  {
    OnAfterAppRender();
  }

  protected override async Task OnAfterRenderAsync(bool firstRender)
  {
    if (!firstRender)
    {
      await AppQueryExecutor.ExecuteAsync();
    }

    await OnAfterAppRenderAsync();
  }

  protected virtual void OnAfterAppRender() { }

  protected virtual async Task OnAfterAppRenderAsync() { }
}
