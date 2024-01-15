using System.Security.Claims;
using Mess.Blazor.Abstractions.Extensions;
using Mess.Blazor.Abstractions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Html;
using OrchardCore.DisplayManagement.Razor;
using OrchardCore.DisplayManagement.Title;
using OrchardCore.DisplayManagement.Zones;
using OrchardCore.Modules;
using OrchardCore.Settings;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using YesSql;

// TODO: optimize user fetching - static async local

namespace Mess.Blazor.Abstractions.Components;

public abstract class AbstractComponent : OwningComponentBase
{
  [Inject]
  private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

  // NOTE: it has to be injected - otherwise it doesn't get initialized
  [Inject]
  private NavigationManager NavigationManager { get; set; } = default!;

  [Inject]
  private ILogger<AbstractComponent> Logger { get; set; } = default!;

  private IDisplayHelper? _displayHelper;

  private HttpContext? _httpContext;

  private IOrchardDisplayHelper? _orchardHelper;

  private IPageTitleBuilder? _pageTitleBuilder;

  private IServiceProvider? _serviceProvider;

  private IShapeFactory? _shapeFactory;

  private ISite? _site;

  private IComponentLocalizer? _t;

  private IZoneHolding? _themeLayout;

  private ClaimsPrincipal? _claimsPrincipal;

  private User? _user;

  public HttpContext HttpContext => _httpContext ??=
    HttpContextAccessor.HttpContext ??
    throw new InvalidOperationException(
      "HttpContext is null");

  public IServiceProvider ServiceProvider => _serviceProvider ??=
    HttpContext.RequestServices;

  /// <summary>
  ///   Gets the <see cref="ISite" /> instance.
  /// </summary>
  protected ISite? Site =>
    _site ??= HttpContext.Features.Get<RazorViewFeature>()?.Site;

  protected IZoneHolding? ThemeLayout => _themeLayout ??=
    HttpContext.Features.Get<RazorViewFeature>()?.ThemeLayout;

  protected IDisplayHelper DisplayHelper => _displayHelper ??=
    ServiceProvider.GetRequiredService<IDisplayHelper>();

  protected IShapeFactory ShapeFactory => _shapeFactory ??=
    ServiceProvider.GetRequiredService<IShapeFactory>();

  protected NavigationManager Navigation => NavigationManager;

  protected IOrchardDisplayHelper Orchard => _orchardHelper ??=
    new ShapeComponentOrchardDisplayHelper(HttpContext, DisplayHelper);

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
  protected dynamic New => ShapeFactory.New;

  /// <summary>
  ///   Gets an <see cref="IShapeFactory" /> to create new shapes.
  /// </summary>
  protected IShapeFactory Factory => ShapeFactory;

  protected string ViewLayout
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

  protected IPageTitleBuilder Title => _pageTitleBuilder ??=
    HttpContext.RequestServices.GetRequiredService<IPageTitleBuilder>();

  /// <summary>
  ///   The <see cref="IComponentLocalizer" /> instance for the current component.
  /// </summary>
  protected IComponentLocalizer T
  {
    get
    {
      if (_t is null)
      {
        _t = ServiceProvider.GetRequiredService<IComponentLocalizer>();
        (_t as IComponentAware)?.Contextualize(GetType());
      }

      return _t;
    }
  }

  protected ClaimsPrincipal ClaimsPrincipal => _claimsPrincipal
    ??= HttpContext.User;

  protected User? User
  {
    get
    {
      if (_user is null || _user.UserName != ClaimsPrincipal.Identity?.Name)
      {
        var component = this;
        InvokeAsync(async () =>
        {
          var user = await component.GetAuthenticatedOrchardCoreUserAsync();
          component._user = user;
          component.StateHasChanged();
        });
        return null;
      }

      return _user;
    }
  }

  /// <summary>
  ///   Returns the full escaped path of the current request.
  /// </summary>
  protected string FullRequestPath =>
    HttpContext.Request.PathBase +
    HttpContext.Request.Path +
    HttpContext.Request.QueryString;

  /// <summary>
  ///   In a Razor layout page, renders the portion of a content page that is not
  ///   within a named zone.
  /// </summary>
  /// <returns>The HTML content to render.</returns>
  protected virtual Task<RenderFragment> RenderBodyAsync()
  {
    if (ThemeLayout is null)
    {
      return Task.FromResult<RenderFragment>(builder => { });
    }

    return DisplayAsync(ThemeLayout.Zones["Content"]);
  }

  /// <summary>
  ///   Renders a shape.
  /// </summary>
  /// <param name="shape">The shape.</param>
  protected async Task<RenderFragment> DisplayAsync(dynamic dynamic)
  {
    var content = dynamic switch
    {
      IShape shape => await DisplayHelper.ShapeExecuteAsync(shape),
      IHtmlContent htmlContent => htmlContent,
      string str => new HtmlContentString(str),
      _ => throw new ArgumentException(
        "DisplayAsync requires an instance of IShape")
    };

    return builder =>
    {
      builder.AddContent(0, content.ToMarkupString());
    };
  }

  /// <summary>
  ///   Renders a shape.
  /// </summary>
  /// <param name="shape">The shape.</param>
  protected async Task<RenderFragment> DisplayAsync(IShape shape)
  {
    var content = await DisplayHelper.ShapeExecuteAsync(shape);
    return builder =>
    {
      builder.AddContent(0, content.ToMarkupString());
    };
  }

  /// <summary>
  ///   Adds a segment to the title and returns all segments.
  /// </summary>
  /// <param name="segment">The segment to add to the title.</param>
  /// <param name="position">Optional. The position of the segment in the title.</param>
  /// <param name="separator">The html string that should separate all segments.</param>
  /// <returns>And <see cref="IHtmlContent" /> instance representing the full title.</returns>
  protected RenderFragment RenderTitleSegments(
    IHtmlContent segment,
    string position = "0",
    IHtmlContent? separator = null
  )
  {
    Title.AddSegment(segment, position);
    var content = Title.GenerateTitle(separator);
    return builder =>
    {
      builder.AddContent(0, content.ToMarkupString());
    };
  }

  /// <summary>
  ///   Adds a segment to the title and returns all segments.
  /// </summary>
  /// <param name="segment">The segment to add to the title.</param>
  /// <param name="position">Optional. The position of the segment in the title.</param>
  /// <param name="separator">The html string that should separate all segments.</param>
  /// <returns>And <see cref="IHtmlContent" /> instance representing the full title.</returns>
  protected RenderFragment RenderTitleSegments(
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

    var content = Title.GenerateTitle(separator);
    return builder =>
    {
      builder.AddContent(0, content.ToMarkupString());
    };
  }

  /// <summary>
  ///   Creates a <see cref="TagBuilder" /> to render a shape.
  /// </summary>
  /// <param name="shape">The shape.</param>
  /// <returns>A new <see cref="TagBuilder" />.</returns>
  protected TagBuilder Tag(IShape shape)
  {
    return shape.GetTagBuilder();
  }

  /// <summary>
  ///   Creates a <see cref="TagBuilder" /> to render a shape.
  /// </summary>
  /// <param name="shape">The shape.</param>
  /// <param name="tag">The tag name to use.</param>
  /// <returns>A new <see cref="TagBuilder" />.</returns>
  protected TagBuilder Tag(IShape shape, string tag)
  {
    return shape.GetTagBuilder(tag);
  }

  /// <summary>
  ///   Check if a zone is defined in the layout or it has items.
  /// </summary>
  /// <param name="name"></param>
  /// <returns></returns>
  protected bool IsSectionDefined(string name)
  {
    // We can replace the base implementation as it can't be called on a view that is not an actual MVC Layout.

    if (name == null) throw new ArgumentNullException(nameof(name));

    return ThemeLayout?.Zones.IsNotEmpty(name) is true;
  }

  /// <summary>
  ///   Renders a zone from the layout.
  /// </summary>
  /// <param name="name">The name of the zone to render.</param>
  protected RenderFragment RenderSection(string name)
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
  protected RenderFragment RenderSection(string name, bool required)
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
  protected Task<RenderFragment> RenderSectionAsync(string name)
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
  protected Task<RenderFragment> RenderSectionAsync(string name, bool required)
  {
    // We can replace the base implementation as it can't be called on a view that is not an actual MVC Layout.

    if (name == null) throw new ArgumentNullException(nameof(name));

    var zone = ThemeLayout?.Zones[name];

    if (required && zone.IsNullOrEmpty())
      throw new InvalidOperationException("Zone not found: " + name);

    return DisplayAsync(zone!);
  }

  protected object OrDefault(object text, object other)
  {
    if (text == null || Convert.ToString(text) == "") return other;

    return text;
  }

  protected void WithSessionLock(Action<IServiceProvider> action)
  {
    action(ServiceProvider);
  }

  protected async Task WithSessionLockAsync(Func<IServiceProvider, Task> action)
  {
    await action(ServiceProvider);
  }
}
