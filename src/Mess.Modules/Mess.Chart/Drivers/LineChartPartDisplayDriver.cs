using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;

namespace Mess.Chart.Drivers;

public class LineChartPartDisplayDriver
  : ContentPartDisplayDriver<LineChartPart>
{
  public override IDisplayResult Display(
    LineChartPart part,
    BuildPartDisplayContext context
  )
  {
    return Combine(
      Dynamic(
          "LineChartPart_Admin",
          model =>
          {
            model.Part = part;
            model.Definition = context.TypePartDefinition;
          }
        )
        .Location("Admin", "Content:10"),
      Dynamic(
          "LineChartPart_Thumbnail",
          model =>
          {
            model.Part = part;
            model.Definition = context.TypePartDefinition;
          }
        )
        .Location("Thumbnail", "Content:10")
    );
  }

  public override IDisplayResult Edit(
    LineChartPart part,
    BuildPartEditorContext context
  )
  {
    return Initialize<LineChartPartViewModel>(
      "LineChartPart_Edit",
      model =>
      {
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    LineChartPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new LineChartPartViewModel();

    if (await updater.TryUpdateModelAsync(viewModel, Prefix))
    {
      return Edit(part, context);
    }

    return Edit(part, context);
  }

  public LineChartPartDisplayDriver(
    IStringLocalizer<LineChartPartDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  private readonly IStringLocalizer S;
}
