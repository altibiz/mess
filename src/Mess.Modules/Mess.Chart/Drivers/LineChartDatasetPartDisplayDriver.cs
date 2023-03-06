using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.ContentManagement.Metadata.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.Mvc.ModelBinding;
using Mess.System.Extensions.String;

namespace Mess.Chart.Drivers;

public class LineChartDatasetPartDisplayDriver
  : ContentPartDisplayDriver<LineChartDatasetPart>
{
  public override IDisplayResult Display(
    LineChartDatasetPart part,
    BuildPartDisplayContext context
  )
  {
    return Initialize<LineChartDatasetPartAdminViewModel>(
        "LineChartDatasetPart_Admin",
        model =>
        {
          model.Part = part;
          model.Definition = context.TypePartDefinition;
        }
      )
      .Location("Content");
  }

  public override IDisplayResult Edit(
    LineChartDatasetPart part,
    BuildPartEditorContext context
  )
  {
    return Initialize<LineChartDatasetPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Label = part.Label;
        model.Color = part.Color;
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    LineChartDatasetPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new LineChartDatasetPartEditViewModel();

    if (
      await updater.TryUpdateModelAsync(
        viewModel,
        Prefix,
        model => model.Label,
        model => model.Color
      )
    )
    {
      if (string.IsNullOrWhiteSpace(viewModel.Label))
      {
        var partName = context.TypePartDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Label),
          S["{0} requires a label", partName]
        );
      }

      if (string.IsNullOrWhiteSpace(viewModel.Color))
      {
        var partName = context.TypePartDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Color),
          S["{0} requires a color", partName]
        );
      }

      if (!viewModel.Color.RegexMatch(@"^#[0-9a-fA-F]{6}$"))
      {
        var partName = context.TypePartDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Color),
          S["{0} requires a valid hex color", partName]
        );
      }

      if (!updater.ModelState.IsValid)
      {
        return Edit(part, context);
      }

      part.Label = viewModel.Label;
      part.Color = viewModel.Color;
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
