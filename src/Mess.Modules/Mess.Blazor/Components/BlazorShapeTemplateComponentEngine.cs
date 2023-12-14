using System.ComponentModel;
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

  private readonly ComponentHelper _componentHelper;
  private readonly ViewContextAccessor _viewContextAccessor;
  private readonly IShapeComponentModelStore _shapeComponentModelStore;

  public BlazorShapeTemplateComponentEngine(
    ComponentHelper componentHelper,
    ViewContextAccessor viewContextAccessor,
    IShapeComponentModelStore shapeComponentModelStore
  )
  {
    _componentHelper = componentHelper;
    _viewContextAccessor = viewContextAccessor;
    _shapeComponentModelStore = shapeComponentModelStore;
  }

  public IEnumerable<Type> TemplateBaseClasses => _templateBaseClasses;

  public async Task<IHtmlContent> RenderAsync(
    Type componentType,
    DisplayContext displayContext
  )
  {
    var viewContext = _viewContextAccessor.ViewContext;
    if (viewContext is null)
    {
      var actionContext = await _componentHelper.GetActionContextAsync();
      viewContext = await _componentHelper.MakeViewContextAsync(actionContext, displayContext.Value);
    }

    var renderId = Guid.NewGuid();
    _shapeComponentModelStore.Add(renderId, displayContext.Value);

    var viewData = new ViewDataDictionary(viewContext.ViewData);
    viewData.TemplateInfo.HtmlFieldPrefix = displayContext.HtmlFieldPrefix;
    var htmlHelper = _componentHelper.MakeHtmlHelper(viewContext, viewData);

    return await htmlHelper.RenderComponentAsync(componentType,
      RenderMode.ServerPrerendered,
      new { RenderId = renderId }
    );
  }
}
