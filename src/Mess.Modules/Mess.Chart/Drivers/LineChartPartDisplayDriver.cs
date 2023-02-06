using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;

namespace Mess.Chart.Drivers;

public class LineChartPartDisplayDriver : ContentPartDisplayDriver<LineChartPart>
{
  public override IDisplayResult Display(
    LineChartPart part,
    BuildPartDisplayContext context
  )
  {
    return Initialize<LineChartPartViewModel>(
      GetDisplayShapeType(context),
      model =>
      {
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override IDisplayResult Edit(
    LineChartPart part,
    BuildPartEditorContext context
  )
  {
    return Initialize<LineChartPartViewModel>(
      GetEditorShapeType(context),
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
