using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Abstractions.Pooling;
using OrchardCore.DisplayManagement.Descriptors.ShapeTemplateStrategy;
using OrchardCore.DisplayManagement.Implementation;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Razor;
using Mess.Blazor.Abstractions;

namespace Mess.Blazor;

// TODO: null handling

public class BlazorShapeTemplateComponentEngine : IShapeTemplateComponentEngine
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ViewContextAccessor _viewContextAccessor;
  private readonly ITempDataProvider _tempDataProvider;

  private readonly List<Type> _templateBaseClasses = new(new [ ] { typeof(Microsoft.AspNetCore.Components.ComponentBase) });

  public BlazorShapeTemplateComponentEngine(
    IHttpContextAccessor httpContextAccessor,
    ViewContextAccessor viewContextAccessor,
    ITempDataProvider tempDataProvider
  ) {
    _httpContextAccessor = httpContextAccessor;
    _viewContextAccessor = viewContextAccessor;
    _tempDataProvider = tempDataProvider;
  }

  public IEnumerable<Type> TemplateBaseClasses => _templateBaseClasses;

  public async Task<IHtmlContent> RenderAsync(Type componentType, DisplayContext displayContext)
  {
    var viewContext = _viewContextAccessor.ViewContext;
    if (viewContext is not { HttpContext: not null })
    {
      var actionContext = await GetActionContextAsync();
      viewContext = await MakeViewContextAsync(actionContext, displayContext);
    }

    var viewData = new ViewDataDictionary(viewContext.ViewData);
    viewData.TemplateInfo.HtmlFieldPrefix = displayContext.HtmlFieldPrefix;
    IHtmlHelper htmlHelper = MakeHtmlHelper(viewContext, viewData);

    return await htmlHelper.RenderComponentAsync(componentType, RenderMode.ServerPrerendered, displayContext.Value);
  }

  private async Task<ViewContext> MakeViewContextAsync(ActionContext actionContext, DisplayContext displayContext)
  {
    var viewContext = new ViewContext(
      actionContext,
      null!,
      new ViewDataDictionary(
        metadataProvider: new EmptyModelMetadataProvider(),
        modelState: new ModelStateDictionary()
      ) {
        Model = displayContext.Value
      },
      new TempDataDictionary(
        actionContext.HttpContext,
        _tempDataProvider
      ),
      null!,
      new HtmlHelperOptions()
    );

    return viewContext;
  }

  private static IHtmlHelper MakeHtmlHelper(ViewContext viewContext, ViewDataDictionary viewData)
  {
    var newHelper = viewContext.HttpContext.RequestServices.GetRequiredService<IHtmlHelper>();

    if (newHelper is IViewContextAware contextable)
    {
      var newViewContext = new ViewContext(viewContext, viewContext.View, viewData, viewContext.Writer);
      contextable.Contextualize(newViewContext);
    }

    return newHelper;
  }

  private async Task<ActionContext> GetActionContextAsync()
  {
    var httpContext = _httpContextAccessor.HttpContext!;
    var actionContext = httpContext.RequestServices.GetService<IActionContextAccessor>()?.ActionContext!;

    if (actionContext != null)
    {
      return actionContext;
    }

    var routeData = new RouteData();
    routeData.Routers.Add(new RouteCollection());

    actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
    var filters = httpContext.RequestServices.GetServices<IAsyncViewActionFilter>();

    foreach (var filter in filters)
    {
      await filter.OnActionExecutionAsync(actionContext);
    }

    return actionContext;
  }
}
