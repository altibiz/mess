using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Mess.Blazor.Abstractions.Components;

/// <summary>
/// Displays the specified content inside the specified layout and any further
/// nested layouts.
/// </summary>
public class CapturedLayoutView : IComponent
{
  private static readonly RenderFragment EmptyRenderFragment = builder => { };

  private RenderHandle _renderHandle;

  /// <summary>
  /// Gets or sets the content to display.
  /// </summary>
  [Parameter]
  public RenderFragment ChildContent { get; set; } = default!;

  /// <summary>
  /// Gets or sets the type of the layout in which to display the content.
  /// The type must implement <see cref="IComponent"/> and accept a parameter named <see cref="LayoutComponentBase.Body"/>.
  /// </summary>
  [Parameter]
  public Type Layout { get; set; } = default!;

  [Parameter]
  public Guid CaptureId { get; set; } = default!;

  /// <inheritdoc />
  public void Attach(RenderHandle renderHandle)
  {
    _renderHandle = renderHandle;
  }

  /// <inheritdoc />
  public Task SetParametersAsync(ParameterView parameters)
  {
    parameters.SetParameterProperties(this);

    var captureId = CaptureId;

    // In the middle goes the supplied content
    var fragment = ChildContent ?? EmptyRenderFragment;

    // Then repeatedly wrap that in each layer of nested layout until we get
    // to a layout that has no parent
    var layoutType = Layout;
    while (layoutType != null)
    {
      fragment = WrapInLayout(layoutType, fragment, captureId);
      layoutType = GetParentLayoutType(layoutType);
    }

    _renderHandle.Render(fragment);

    return Task.CompletedTask;
  }

  private static RenderFragment WrapInLayout(Type layoutType, RenderFragment bodyParam, Guid captureId)
  {
    void Render(RenderTreeBuilder builder)
    {
      builder.OpenComponent(0, layoutType);
      builder.AddAttribute(1, "Body", bodyParam);
      builder.AddAttribute(1, "CaptureId", captureId);
      builder.CloseComponent();
    };

    return Render;
  }

  private static Type? GetParentLayoutType(Type type)
    => type.GetCustomAttribute<LayoutAttribute>()?.LayoutType;
}
