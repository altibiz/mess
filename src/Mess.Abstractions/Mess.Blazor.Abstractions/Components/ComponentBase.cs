using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Html;
using OrchardCore.DisplayManagement.Razor;
using OrchardCore.DisplayManagement.Title;
using OrchardCore.DisplayManagement.Zones;
using OrchardCore.Settings;

namespace Mess.Blazor.Abstractions.Components;

public partial class
  ComponentBase<TModel> : Microsoft.AspNetCore.Components.ComponentBase, IDisposable
  where TModel : class
{
  private HttpContext? _httpContext;

  private ViewContext? _viewContext;

  private TModel? _model;

  private IDisplayHelper? _displayHelper;

  private IOrchardDisplayHelper? _orchardHelper;

  private IPageTitleBuilder? _pageTitleBuilder;

  private IShapeFactory? _shapeFactory;

  private ISite? _site;

  private IViewLocalizer? _t;

  private IZoneHolding? _themeLayout;

  [Parameter]
  public Guid ViewContextId { get; set; } = default!;

  public ViewContext ViewContext => _viewContext ??= viewContextStore.Get(ViewContextId) ??
                                    throw new InvalidOperationException(
                                      "ViewContext is null");

  public HttpContext HttpContext => _httpContext ??= httpContextAccessor.HttpContext ??
                                    throw new InvalidOperationException(
                                      "HttpContext is null");

  /// <summary>
  ///   Gets the <see cref="TModel" /> instance.
  /// </summary>
  public TModel Model => _model ??= ViewContext.ViewData.Model as TModel ??
                         throw new InvalidOperationException(
                           "Model type is invalid");

  /// <summary>
  ///   Gets the <see cref="ISite" /> instance.
  /// </summary>
  public ISite? Site =>
    _site ??= HttpContext.Features.Get<RazorViewFeature>()?.Site;

  public IOrchardDisplayHelper Orchard => _orchardHelper ??=
    new OrchardDisplayHelper(HttpContext, DisplayHelper);

  private IDisplayHelper DisplayHelper => _displayHelper ??=
    HttpContext.RequestServices.GetRequiredService<IDisplayHelper>();

  private IShapeFactory ShapeFactory => _shapeFactory ??=
    HttpContext.RequestServices.GetRequiredService<IShapeFactory>();

  /// <summary>
  ///   Gets a dynamic shape factory to create new shapes.
  /// </summary>
  /// <example>
  ///   Usage:
  ///   <code>
  /// await New.MyShape()
  /// await New.MyShape(A: 1, B: "Some text")
  /// (await New.MyShape()).A(1).B("Some text")
  /// </code>
  /// </example>
  public dynamic New => ShapeFactory;

  /// <summary>
  ///   Gets an <see cref="IShapeFactory" /> to create new shapes.
  /// </summary>
  public IShapeFactory Factory => ShapeFactory;

  public IZoneHolding? ThemeLayout => _themeLayout ??=
    HttpContext.Features.Get<RazorViewFeature>()?.ThemeLayout;

  public string ViewLayout
  {
    get
    {
      if (ThemeLayout is IShape layout)
      {
        if (layout.Metadata.Alternates.Count > 0)
          return layout.Metadata.Alternates.Last;

        return layout.Metadata.Type;
      }

      return string.Empty;
    }

    set
    {
      if (ThemeLayout is IShape layout)
      {
        if (layout.Metadata.Alternates.Contains(value))
        {
          if (layout.Metadata.Alternates.Last == value) return;

          layout.Metadata.Alternates.Remove(value);
        }

        layout.Metadata.Alternates.Add(value);
      }
    }
  }

  public IPageTitleBuilder Title => _pageTitleBuilder ??=
    HttpContext.RequestServices.GetRequiredService<IPageTitleBuilder>();

  /// <summary>
  ///   The <see cref="IViewLocalizer" /> instance for the current view.
  /// </summary>
  public IViewLocalizer T
  {
    get
    {
      if (_t == null)
      {
        _t = HttpContext.RequestServices.GetRequiredService<IViewLocalizer>();
        ((IViewContextAware)_t).Contextualize(ViewContext);
      }

      return _t;
    }
  }

  /// <summary>
  ///   Returns the full escaped path of the current request.
  /// </summary>
  public string FullRequestPath =>
    HttpContext.Request.PathBase +
    HttpContext.Request.Path +
    HttpContext.Request.QueryString;

  /// <summary>
  ///   Renders a shape.
  /// </summary>
  /// <param name="shape">The shape.</param>
  public Task<IHtmlContent> DisplayAsync(dynamic dynamic)
  {
    return dynamic switch
    {
      IShape shape => DisplayHelper.ShapeExecuteAsync(shape),
      IHtmlContent htmlContent => Task.FromResult(htmlContent),
      string str => Task.FromResult<IHtmlContent>(new HtmlContentString(str)),
      _ => throw new ArgumentException(
        "DisplayAsync requires an instance of IShape")
    };
  }

  /// <summary>
  ///   Renders a shape.
  /// </summary>
  /// <param name="shape">The shape.</param>
  public Task<IHtmlContent> DisplayAsync(IShape shape)
  {
    return DisplayHelper.ShapeExecuteAsync(shape);
  }

  /// <summary>
  ///   Adds a segment to the title and returns all segments.
  /// </summary>
  /// <param name="segment">The segment to add to the title.</param>
  /// <param name="position">Optional. The position of the segment in the title.</param>
  /// <param name="separator">The html string that should separate all segments.</param>
  /// <returns>And <see cref="IHtmlContent" /> instance representing the full title.</returns>
  public IHtmlContent RenderTitleSegments(
    IHtmlContent segment,
    string position = "0",
    IHtmlContent? separator = null
  )
  {
    Title.AddSegment(segment, position);
    return Title.GenerateTitle(separator);
  }

  /// <summary>
  ///   Adds a segment to the title and returns all segments.
  /// </summary>
  /// <param name="segment">The segment to add to the title.</param>
  /// <param name="position">Optional. The position of the segment in the title.</param>
  /// <param name="separator">The html string that should separate all segments.</param>
  /// <returns>And <see cref="IHtmlContent" /> instance representing the full title.</returns>
  public IHtmlContent RenderTitleSegments(
    string segment,
    string position = "0",
    IHtmlContent? separator = null
  )
  {
    if (!string.IsNullOrEmpty(segment))
      Title.AddSegment(
        new HtmlContentString(segment),
        position
      );

    return Title.GenerateTitle(separator);
  }

  /// <summary>
  ///   Creates a <see cref="TagBuilder" /> to render a shape.
  /// </summary>
  /// <param name="shape">The shape.</param>
  /// <returns>A new <see cref="TagBuilder" />.</returns>
  public TagBuilder Tag(IShape shape)
  {
    return shape.GetTagBuilder();
  }

  /// <summary>
  ///   Creates a <see cref="TagBuilder" /> to render a shape.
  /// </summary>
  /// <param name="shape">The shape.</param>
  /// <param name="tag">The tag name to use.</param>
  /// <returns>A new <see cref="TagBuilder" />.</returns>
  public TagBuilder Tag(IShape shape, string tag)
  {
    return shape.GetTagBuilder(tag);
  }

  /// <summary>
  ///   In a Razor layout page, renders the portion of a content page that is not
  ///   within a named zone.
  /// </summary>
  /// <returns>The HTML content to render.</returns>
  public Task<IHtmlContent> RenderBodyAsync()
  {
    return ThemeLayout is null
      ? Task.FromResult<IHtmlContent>(HtmlString.Empty)
      : DisplayAsync(ThemeLayout.Zones["Content"]);
  }

  /// <summary>
  ///   Check if a zone is defined in the layout or it has items.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  public bool IsSectionDefined(string name)
  {
    // We can replace the base implementation as it can't be called on a view that is not an actual MVC Layout.

    if (name == null) throw new ArgumentNullException(nameof(name));

    return ThemeLayout?.Zones.IsNotEmpty(name) is true;
  }

  /// <summary>
  ///   Renders a zone from the layout.
  /// </summary>
  /// <param name="name">The name of the zone to render.</param>
  public IHtmlContent RenderSection(string name)
  {
    // We can replace the base implementation as it can't be called on a view that is not an actual MVC Layout.

    if (name == null) throw new ArgumentNullException(nameof(name));

    return RenderSection(name, true);
  }

  /// <summary>
  ///   Renders a zone from the layout.
  /// </summary>
  /// <param name="name">The name of the zone to render.</param>
  /// <param name="required">Whether the zone is required or not.</param>
  public IHtmlContent RenderSection(string name, bool required)
  {
    // We can replace the base implementation as it can't be called on a view that is not an actual MVC Layout.

    if (name == null) throw new ArgumentNullException(nameof(name));

    return RenderSectionAsync(name, required)
      .GetAwaiter()
      .GetResult();
  }

  /// <summary>
  ///   Renders a zone from the layout.
  /// </summary>
  /// <param name="name">The name of the zone to render.</param>
  public Task<IHtmlContent> RenderSectionAsync(string name)
  {
    // We can replace the base implementation as it can't be called on a view that is not an actual MVC Layout.

    if (name == null) throw new ArgumentNullException(nameof(name));

    return RenderSectionAsync(name, true);
  }

  /// <summary>
  ///   Renders a zone from the layout.
  /// </summary>
  /// <param name="name">The name of the zone to render.</param>
  /// <param name="required">Whether the zone is required or not.</param>
  public Task<IHtmlContent> RenderSectionAsync(string name, bool required)
  {
    // We can replace the base implementation as it can't be called on a view that is not an actual MVC Layout.

    if (name == null) throw new ArgumentNullException(nameof(name));

    var zone = ThemeLayout?.Zones[name];

    if (required && zone.IsNullOrEmpty())
      throw new InvalidOperationException("Zone not found: " + name);

    return DisplayAsync(zone!);
  }

  public object OrDefault(object text, object other)
  {
    if (text == null || Convert.ToString(text) == "") return other;

    return text;
  }

#pragma warning disable CA1816
  void IDisposable.Dispose()
  {
    HttpContext.RequestServices
      .GetRequiredService<IViewContextStore>()
      .Remove(ViewContextId);
  }
#pragma warning restore CA1816
}

public class ComponentBase : ComponentBase<dynamic>
{
}
