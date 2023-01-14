using Microsoft.AspNetCore.Html;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Models;
using Mess.Chart.Models;
using Mess.Chart.Settings;
using Mess.Chart.ViewModels;

namespace Mess.Chart.Handlers;

public class ChartPartHandler : ContentPartHandler<ChartPart>
{
  private readonly IContentDefinitionManager _contentDefinitionManager;

  public ChartPartHandler(IContentDefinitionManager contentDefinitionManager)
  {
    _contentDefinitionManager = contentDefinitionManager;
  }

  public override Task GetContentItemAspectAsync(
    ContentItemAspectContext context,
    ChartPart part
  )
  {
    return context.ForAsync<BodyAspect>(async bodyAspect =>
    {
      try
      {
        var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(
          part.ContentItem.ContentType
        );
        var contentTypePartDefinition =
          contentTypeDefinition.Parts.FirstOrDefault(
            x => string.Equals(x.PartDefinition.Name, "ChartPart")
          )!;
        var settings =
          contentTypePartDefinition.GetSettings<ChartPartSettings>();

        // TODO: processing
        // var html = part.Type;
        //
        // if (!settings.SanitizeHtml)
        // {
        //   var model = new ChartPartViewModel()
        //   {
        //     Type = part.Type,
        //     ChartPart = part,
        //     ContentItem = part.ContentItem
        //   };
        //
        //   html = await _liquidTemplateManager.RenderStringAsync(
        //     html,
        //     _htmlEncoder,
        //     model,
        //     new Dictionary<string, FluidValue>()
        //     {
        //       ["ContentItem"] = new ObjectValue(model.ContentItem)
        //     }
        //   );
        // }
        //
        // html = await _shortcodeService.ProcessAsync(
        //   html,
        //   new Context
        //   {
        //     ["ContentItem"] = part.ContentItem,
        //     ["TypePartDefinition"] = contentTypePartDefinition
        //   }
        // );

        bodyAspect.Body = HtmlString.Empty;
      }
      catch
      {
        bodyAspect.Body = HtmlString.Empty;
      }
    });
  }
}
