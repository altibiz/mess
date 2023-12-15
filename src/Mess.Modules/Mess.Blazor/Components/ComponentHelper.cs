using Microsoft.AspNetCore.Components;
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

namespace Mess.Blazor.Components;

public class ComponentHelper
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IActionContextAccessor _actionContextAccessor;
  private readonly ITempDataProvider _tempDataProvider;

  public ComponentHelper(
    IActionContextAccessor actionContextAccessor,
    IHttpContextAccessor httpContextAccessor,
    ITempDataProvider tempDataProvider
  )
  {
    _httpContextAccessor = httpContextAccessor;
    _actionContextAccessor = actionContextAccessor;
    _tempDataProvider = tempDataProvider;
  }

#pragma warning disable CA1822
  public IHtmlHelper MakeHtmlHelper(
    ViewContext viewContext,
    ViewDataDictionary viewData
  )
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
#pragma warning restore CA1822

  public async Task<ViewContext> MakeViewContextAsync(
    ActionContext actionContext,
    object? model
  )
  {
    using var writer = new StringWriter();
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
      writer,
      new HtmlHelperOptions()
    );

    return viewContext;
  }

  public async Task<ActionContext> GetActionContextAsync()
  {
    var httpContext = _httpContextAccessor.HttpContext!;
    var actionContext = _actionContextAccessor.ActionContext;

    if (actionContext is not null) return actionContext;

    var routeData = new Microsoft.AspNetCore.Routing.RouteData();
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
