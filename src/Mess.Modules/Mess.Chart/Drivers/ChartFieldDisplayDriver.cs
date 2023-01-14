using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Fields;
using Mess.Chart.ViewModels;
using Mess.Chart.Settings;

namespace Mess.Chart.Drivers;

public class ChartFieldDisplayDriver : ContentFieldDisplayDriver<ChartField>
{
  private readonly IStringLocalizer S;

  public ChartFieldDisplayDriver(
    IStringLocalizer<ChartFieldDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  public override IDisplayResult Display(
    ChartField field,
    BuildFieldDisplayContext context
  )
  {
    return Initialize<DisplayChartFieldViewModel>(
        GetDisplayShapeType(context),
        async model =>
        {
          model.Type = field.Type;
          model.Field = field;
          model.Part = context.ContentPart;
          model.PartFieldDefinition = context.PartFieldDefinition;

          // TODO: settings
          // var settings =
          //   context.PartFieldDefinition.GetSettings<ChartFieldSettings>();
          // if (!settings.SanitizeHtml)
          // {
          //   model.Html = await _liquidTemplateManager.RenderStringAsync(
          //     field.Html,
          //     _htmlEncoder,
          //     model,
          //     new Dictionary<string, FluidValue>()
          //     {
          //       ["ContentItem"] = new ObjectValue(field.ContentItem)
          //     }
          //   );
          // }

          // TODO: extra processing
          // model.Html = await _shortcodeService.ProcessAsync(
          //   model.Html,
          //   new Context
          //   {
          //     ["ContentItem"] = field.ContentItem,
          //     ["PartFieldDefinition"] = context.PartFieldDefinition
          //   }
          // );
        }
      )
      .Location("Detail", "Content")
      .Location("Summary", "Content");
  }

  public override IDisplayResult Edit(
    ChartField field,
    BuildFieldEditorContext context
  )
  {
    return Initialize<EditChartFieldViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Type = field.Type;
        model.Field = field;
        model.Part = context.ContentPart;
        model.PartFieldDefinition = context.PartFieldDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ChartField field,
    IUpdateModel updater,
    UpdateFieldEditorContext context
  )
  {
    var viewModel = new EditChartFieldViewModel();

    var settings =
      context.PartFieldDefinition.GetSettings<ChartFieldSettings>();

    // TODO: validation
    // if (await updater.TryUpdateModelAsync(viewModel, Prefix, f => f.Type))
    // {
    //   if (
    //     !string.IsNullOrEmpty(viewModel.Html)
    //     && !_liquidTemplateManager.Validate(viewModel.Html, out var errors)
    //   )
    //   {
    //     var fieldName = context.PartFieldDefinition.DisplayName();
    //     context.Updater.ModelState.AddModelError(
    //       Prefix,
    //       nameof(viewModel.Html),
    //       S[
    //         "{0} doesn't contain a valid Liquid expression. Details: {1}",
    //         fieldName,
    //         string.Join(" ", errors)
    //       ]
    //     );
    //   }
    //   else
    //   {
    //     field.Html = settings.SanitizeHtml
    //       ? _htmlSanitizerService.Sanitize(viewModel.Html)
    //       : viewModel.Html;
    //   }
    // }

    return Edit(field, context);
  }
}
