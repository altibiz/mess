using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;

namespace Mess.Chart.Drivers;

public class TimeseriesChartDatasetPartDisplayDriver
  : ContentPartDisplayDriver<TimeseriesChartDatasetPart>
{
  public override IDisplayResult Display(
    TimeseriesChartDatasetPart part,
    BuildPartDisplayContext context
  )
  {
    return Combine(
      Initialize<TimeseriesChartDatasetPartThumbnailViewModel>(
          "TimeseriesChartDatasetPart_Thumbnail",
          model =>
          {
            model.Part = part;
            model.Definition = context.TypePartDefinition;
          }
        )
        .Location("Thumbnail", "Content"),
      Initialize<TimeseriesChartDatasetPartEditViewModel>(
          "TimeseriesChartDatasetPart_Admin",
          model =>
          {
            model.Part = part;
            model.Definition = context.TypePartDefinition;
          }
        )
        .Location("Admin", "Content")
    );
  }

  // TODO: validate property and history
  public override async Task<IDisplayResult> UpdateAsync(
    TimeseriesChartDatasetPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new TimeseriesChartDatasetPartEditViewModel();

    if (await updater.TryUpdateModelAsync(viewModel, Prefix))
    {
      return Edit(part, context);
    }

    return Edit(part, context);
  }

  public TimeseriesChartDatasetPartDisplayDriver(
    IStringLocalizer<TimeseriesChartDatasetPartDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  private readonly IStringLocalizer S;
}
