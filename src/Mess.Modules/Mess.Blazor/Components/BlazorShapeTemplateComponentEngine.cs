using Mess.Blazor.Abstractions.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.DisplayManagement;
using OrchardCore.DisplayManagement.Implementation;

namespace Mess.Blazor.Components;

public class BlazorShapeTemplateComponentEngine : IShapeTemplateComponentEngine
{
  private static readonly List<Type> _templateBaseClasses = new(new[]
  {
    typeof(ShapeComponentBase),
    typeof(ShapeComponentBase<>)
  });

  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IActionContextAccessor _actionContextAccessor;
  private readonly ViewContextAccessor _viewContextAccessor;
  private readonly IShapeComponentModelStore _shapeComponentModelStore;
  private readonly ITempDataProvider _tempDataProvider;

  public BlazorShapeTemplateComponentEngine(
    IHttpContextAccessor httpContextAccessor,
    IActionContextAccessor actionContextAccessor,
    ViewContextAccessor viewContextAccessor,
    IShapeComponentModelStore shapeComponentModelStore,
    ITempDataProvider tempDataProvider
  )
  {
    _httpContextAccessor = httpContextAccessor;
    _actionContextAccessor = actionContextAccessor;
    _viewContextAccessor = viewContextAccessor;
    _tempDataProvider = tempDataProvider;
    _shapeComponentModelStore = shapeComponentModelStore;
  }

  public IEnumerable<Type> TemplateBaseClasses => _templateBaseClasses;

  public async Task<IHtmlContent> RenderAsync(
    Type componentType,
    string htmlPrefix,
    object? model,
    TextWriter? writer
  )
  {
    var viewContext = _viewContextAccessor.ViewContext;
    if (viewContext is null)
    {
      var actionContext = await GetActionContextAsync();
      viewContext = await MakeViewContextAsync(actionContext, model, writer);
    }

    var renderId = Guid.NewGuid();
    _shapeComponentModelStore.Add(renderId, model);

    var viewData = new ViewDataDictionary(viewContext.ViewData);
    viewData.TemplateInfo.HtmlFieldPrefix = htmlPrefix;
    var htmlHelper = MakeHtmlHelper(viewContext, viewData);

    return await htmlHelper.RenderComponentAsync(componentType,
      RenderMode.ServerPrerendered,
      new { RenderId = renderId }
    );
  }

  private async Task<ViewContext> MakeViewContextAsync(
    ActionContext actionContext, object? model, TextWriter? writer)
  {
    var viewContext = new ViewContext(
      actionContext,
      new ComponentView(),
      new ViewDataDictionary(
        new EmptyModelMetadataProvider(),
        new ModelStateDictionary()
      )
      {
        Model = model
      },
      new TempDataDictionary(
        actionContext.HttpContext,
        _tempDataProvider
      ),
      writer!,
      new HtmlHelperOptions()
    );

    return viewContext;
  }


  private static IHtmlHelper MakeHtmlHelper(ViewContext viewContext,
    ViewDataDictionary viewData)
  {
    var htmlHelper = viewContext.HttpContext.RequestServices
      .GetRequiredService<IHtmlHelper>();

    if (htmlHelper is IViewContextAware contextable)
    {
      var newViewContext = new ViewContext(
        viewContext,
        viewContext.View,
        viewData,
        viewContext.Writer
      );
      contextable.Contextualize(newViewContext);
    }

    return htmlHelper;
  }

  private async Task<ActionContext> GetActionContextAsync()
  {
    var httpContext = _httpContextAccessor.HttpContext!;
    var actionContext = _actionContextAccessor.ActionContext;

    if (actionContext is not null) return actionContext;

    var routeData = new RouteData();
    routeData.Routers.Add(new RouteCollection());

    actionContext =
      new ActionContext(httpContext, routeData, new ActionDescriptor());
    var filters =
      httpContext.RequestServices.GetServices<IAsyncViewActionFilter>();

    foreach (var filter in filters)
      await filter.OnActionExecutionAsync(actionContext);

    return actionContext;
  }
}
