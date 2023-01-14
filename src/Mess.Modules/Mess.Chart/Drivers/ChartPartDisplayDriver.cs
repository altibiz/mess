using Mess.Chart.Models;
using Mess.Chart.Settings;
using Mess.Chart.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Mess.Chart.Drivers;

public class ChartPartDisplayDriver : ContentPartDisplayDriver<ChartPart>
{
  private readonly IStringLocalizer S;

  public ChartPartDisplayDriver(
    IStringLocalizer<ChartPartDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  public override IDisplayResult Display(
    ChartPart ChartPart,
    BuildPartDisplayContext context
  )
  {
    return Initialize<ChartPartViewModel>(
        GetDisplayShapeType(context),
        m => BuildViewModelAsync(m, ChartPart, context)
      )
      .Location("Detail", "Content:5")
      .Location("Summary", "Content:10");
  }

  public override IDisplayResult Edit(
    ChartPart ChartPart,
    BuildPartEditorContext context
  )
  {
    return Initialize<ChartPartViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Type = ChartPart.Type;
        model.ContentItem = ChartPart.ContentItem;
        model.ChartPart = ChartPart;
        model.TypePartDefinition = context.TypePartDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ChartPart model,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new ChartPartViewModel();

    var settings = context.TypePartDefinition.GetSettings<ChartPartSettings>();

    // TODO: processing
    // if (await updater.TryUpdateModelAsync(viewModel, Prefix, t => t.Html))
    // {
    //   if (
    //     !string.IsNullOrEmpty(viewModel.Html)
    //     && !_liquidTemplateManager.Validate(viewModel.Html, out var errors)
    //   )
    //   {
    //     var partName = context.TypePartDefinition.DisplayName();
    //     updater.ModelState.AddModelError(
    //       Prefix,
    //       nameof(viewModel.Html),
    //       S[
    //         "{0} doesn't contain a valid Liquid expression. Details: {1}",
    //         partName,
    //         string.Join(" ", errors)
    //       ]
    //     );
    //   }
    //   else
    //   {
    //     model.Html = settings.SanitizeHtml
    //       ? _htmlSanitizerService.Sanitize(viewModel.Html)
    //       : viewModel.Html;
    //   }
    // }

    return Edit(model, context);
  }

  private async ValueTask BuildViewModelAsync(
    ChartPartViewModel model,
    ChartPart chartPart,
    BuildPartDisplayContext context
  )
  {
    model.Type = chartPart.Type;
    model.ChartPart = chartPart;
    model.ContentItem = chartPart.ContentItem;
    var settings = context.TypePartDefinition.GetSettings<ChartPartSettings>();

    // TODO: processing
    // if (!settings.SanitizeHtml)
    // {
    //   model.Html = await _liquidTemplateManager.RenderStringAsync(
    //     chartPart.Html,
    //     _htmlEncoder,
    //     model,
    //     new Dictionary<string, FluidValue>()
    //     {
    //       ["ContentItem"] = new ObjectValue(model.ContentItem)
    //     }
    //   );
    // }
    //
    // model.Html = await _shortcodeService.ProcessAsync(
    //   model.Html,
    //   new Context
    //   {
    //     ["ContentItem"] = chartPart.ContentItem,
    //     ["TypePartDefinition"] = context.TypePartDefinition
    //   }
    // );
  }
}
