using System.Text.Encodings.Web;
using Mess.Blazor.Abstractions.Extensions;
using Mess.Blazor.Abstractions.Localization;
using Mess.Blazor.Abstractions.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Html;
using OrchardCore.DisplayManagement.Razor;
using OrchardCore.DisplayManagement.Title;
using OrchardCore.DisplayManagement.Zones;
using OrchardCore.Settings;

namespace Mess.Blazor.Abstractions.Components;

public class LayoutShapeComponentBase : AbstractComponent
{
  [Inject]
  private ICircuitAccessor CircuitAccessor { get; set; } = default!;

  [Inject]
  private IComponentCaptureStore ComponentCaptureStore { get; set; } = default!;

  private LayoutViewModel? _model;

  [Parameter] public Guid? CaptureId { get; set; }

  [Parameter] public RenderFragment? Body { get; set; }

  /// <summary>
  ///   Gets the <see cref="TModel" /> instance.
  /// </summary>
  public LayoutViewModel Model
  {
    get
    {
      var captureId = CaptureId
        ?? throw new InvalidOperationException("CaptureId is null");

      var circuitId = CircuitAccessor.Circuit?.Id;

      var capture = ComponentCaptureStore.Get(captureId, circuitId);

      var model = capture?.Model as LayoutViewModel
        ?? throw new InvalidOperationException("Model is invalid");


      return _model ??= model;
    }
  }

  public MarkupString? Meta => Model.Meta;

  public MarkupString? HeadLink => Model.HeadLink;

  public MarkupString? HeadScript => Model.HeadScript;

  public MarkupString? Stylesheet => Model.Stylesheet;

  public MarkupString? FootScript => Model.FootScript;

  protected sealed override void OnInitialized()
  {
    OnLayoutInitialized();
  }

  protected sealed async override Task OnInitializedAsync()
  {
    Body ??= await RenderBodyAsync();
    await OnLayoutInitializedAsync();
  }

  protected virtual void OnLayoutInitialized()
  {
  }

  protected virtual Task OnLayoutInitializedAsync()
  {
    return Task.CompletedTask;
  }

  /// <summary>
  ///   In a Razor layout page, renders the portion of a content page that is not
  ///   within a named zone.
  /// </summary>
  /// <returns>The HTML content to render.</returns>
  public override async Task<RenderFragment> RenderBodyAsync()
  {
    if (Model.ComponentType is null || Model.CaptureId is null)
    {
      return builder => { };
    }

    return builder =>
    {
      builder.OpenComponent(0, Model.ComponentType);
      builder.AddAttribute(0, "CaptureId", Model.CaptureId);
      builder.CloseComponent();
    };
  }
}
