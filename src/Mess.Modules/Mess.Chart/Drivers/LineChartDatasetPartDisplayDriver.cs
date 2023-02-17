using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
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
    return Initialize<LineChartDatasetPartEditViewModel>(
        "LineChartDatasetPart",
        model =>
        {
          model.Part = part;
          model.Definition = context.TypePartDefinition;
        }
      )
      .Location("Content");
  }

  public LineChartDatasetPartDisplayDriver(
    IStringLocalizer<LineChartDatasetPartDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  private readonly IStringLocalizer S;
}
