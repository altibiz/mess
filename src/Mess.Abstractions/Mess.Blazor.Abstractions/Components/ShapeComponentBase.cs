using System.Security.Claims;
using System.Text.Encodings.Web;
using Mess.Blazor.Abstractions.Extensions;
using Mess.Blazor.Abstractions.Localization;
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
using OrchardCore.Users.Models;

namespace Mess.Blazor.Abstractions.Components;

public partial class ShapeComponentBase<TModel> : AbstractComponent
  where TModel : class
{
  [Inject]
  private ICircuitAccessor CircuitAccessor { get; set; } = default!;

  [Inject]
  private IComponentCaptureStore ComponentCaptureStore { get; set; } = default!;

  private TModel? _model;

  [Parameter] public Guid? CaptureId { get; set; }

  /// <summary>
  ///   Gets the <see cref="TModel" /> instance.
  /// </summary>
  public TModel Model
  {
    get
    {
      var captureId = CaptureId
        ?? throw new InvalidOperationException("CaptureId is null");

      var circuitId = CircuitAccessor.Circuit?.Id;

      var capture = ComponentCaptureStore.Get(captureId, circuitId);

      var model = capture?.Model as TModel
        ?? throw new InvalidOperationException("Model is invalid");

      return _model ??= model;
    }
  }
}

public class ShapeComponentBase : ShapeComponentBase<dynamic>
{
}
