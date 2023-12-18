using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Routing;
using RouteData = Microsoft.AspNetCore.Components.RouteData;

// TODO: query string params

namespace Mess.Blazor.Abstractions.Components;

/// <summary>
/// Displays the specified page component, rendering it inside its layout
/// and any further nested layouts.
/// </summary>
public class CapturedRouteView : IComponent
{
  private readonly RenderFragment _renderDelegate;
  private readonly RenderFragment _renderPageWithParametersDelegate;
  private RenderHandle _renderHandle;

  [Inject]
  private NavigationManager NavigationManager { get; set; } = default!;

  /// <summary>
  /// Gets or sets the route data. This determines the page that will be
  /// displayed and the parameter values that will be supplied to the page.
  /// </summary>
  [Parameter]
  public RouteData RouteData { get; set; } = default!;

  /// <summary>
  /// Gets or sets the type of a layout to be used if the page does not
  /// declare any layout. If specified, the type must implement <see cref="IComponent"/>
  /// and accept a parameter named <see cref="LayoutComponentBase.Body"/>.
  /// </summary>
  [Parameter]
  public Type DefaultLayout { get; set; } = default!;

  [Parameter]
  public Guid CaptureId { get; set; } = default!;

  /// <summary>
  /// Initializes a new instance of <see cref="CapturedRouteView"/>.
  /// </summary>
  public CapturedRouteView()
  {
    // Cache the delegate instances
    _renderDelegate = Render;
    _renderPageWithParametersDelegate = RenderPageWithParameters;
  }

  /// <inheritdoc />
  public void Attach(RenderHandle renderHandle)
  {
    _renderHandle = renderHandle;
  }

  /// <inheritdoc />
  public Task SetParametersAsync(ParameterView parameters)
  {
    parameters.SetParameterProperties(this);

    if (RouteData == null)
    {
      throw new InvalidOperationException($"The {nameof(CapturedRouteView)} component requires a non-null value for the parameter {nameof(RouteData)}.");
    }

    _renderHandle.Render(_renderDelegate);
    return Task.CompletedTask;
  }

  /// <summary>
  /// Renders the component.
  /// </summary>
  /// <param name="builder">The <see cref="RenderTreeBuilder"/>.</param>
  protected virtual void Render(RenderTreeBuilder builder)
  {
    var pageLayoutType = this.RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType
        ?? DefaultLayout;

    builder.OpenComponent<CapturedLayoutView>(0);
    builder.AddAttribute(1, nameof(CapturedLayoutView.Layout), pageLayoutType);
    builder.AddAttribute(2, nameof(CapturedLayoutView.ChildContent), _renderPageWithParametersDelegate);
    builder.AddAttribute(3, nameof(CapturedLayoutView.CaptureId), CaptureId);
    builder.CloseComponent();
  }

  private void RenderPageWithParameters(RenderTreeBuilder builder)
  {
    builder.OpenComponent(0, RouteData.PageType);

    foreach (var kvp in RouteData.RouteValues)
    {
      builder.AddAttribute(1, kvp.Key, kvp.Value);
    }

    // var queryParameterSupplier = QueryParameterValueSupplier.ForType(RouteData.PageType);
    // if (queryParameterSupplier is not null)
    // {
    //   // Since this component does accept some parameters from query, we must supply values for all of them,
    //   // even if the querystring in the URI is empty. So don't skip the following logic.
    //   var url = NavigationManager.Uri;
    //   ReadOnlyMemory<char> query = default;
    //   var queryStartPos = url.IndexOf('?');
    //   if (queryStartPos >= 0)
    //   {
    //     var queryEndPos = url.IndexOf('#', queryStartPos);
    //     query = url.AsMemory(queryStartPos..(queryEndPos < 0 ? url.Length : queryEndPos));
    //   }
    //   queryParameterSupplier.RenderParametersFromQueryString(builder, query);
    // }

    builder.CloseComponent();
  }
}
