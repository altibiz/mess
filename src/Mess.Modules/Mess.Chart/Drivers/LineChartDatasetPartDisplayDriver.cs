using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;

namespace Mess.Chart.Drivers;

public class LineChartDatasetPartDisplayDriver
  : ContentPartDisplayDriver<LineChartDatasetPart>
{
  public override IDisplayResult Display(
    LineChartDatasetPart part,
    BuildPartDisplayContext context
  )
  {
    return Dynamic(
        "LineChartDatasetPart",
        model =>
        {
          model.Part = part;
          model.Definition = context.TypePartDefinition;
        }
      )
      .Location("Content:10");
  }

  // TODO: validate color
  public override async Task<IDisplayResult> UpdateAsync(
    LineChartDatasetPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new LineChartDatasetPartViewModel();

    if (await updater.TryUpdateModelAsync(viewModel, Prefix))
    {
      return Edit(part, context);
    }

    return Edit(part, context);
  }

  public LineChartDatasetPartDisplayDriver(
    IStringLocalizer<LineChartDatasetPartDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  private readonly IStringLocalizer S;
}
