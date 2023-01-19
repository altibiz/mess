using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.MeasurementDevice.Chart.Models;
using Mess.MeasurementDevice.Chart.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.Mvc.ModelBinding;
using Mess.MeasurementDevice.Abstractions.Models;
using Mess.System.Extensions.Type;
using OrchardCore.ContentManagement;
using Mess.MeasurementDevice.Models;

namespace Mess.MeasurementDevice.Chart.Drivers;

public class ContentMenuItemPartDisplayDriver
  : ContentPartDisplayDriver<EgaugeChartFieldPart>
{
  public override IDisplayResult Display(
    EgaugeChartFieldPart part,
    BuildPartDisplayContext context
  )
  {
    return Initialize<EgaugeChartFieldPartViewModel>(
        "EgaugeChartFieldPart_Admin",
        model =>
        {
          model.Label = part.Label;
          model.Unit = part.Unit;
          model.Color = part.Color;
          model.Field = part.Field;

          model.Part = part;
        }
      )
      .Location("Admin", "Content");
  }

  public override IDisplayResult Edit(EgaugeChartFieldPart part)
  {
    return Initialize<EgaugeChartFieldPartEditViewModel>(
      "EgaugeChartFieldPart_Edit",
      model =>
      {
        model.Label = part.Label;
        model.Unit = part.Unit;
        model.Color = part.Color;
        model.Field = part.Field;

        model.Part = part;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    EgaugeChartFieldPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new EgaugeChartFieldPartEditViewModel();

    // TODO: validate color/unit?
    if (await updater.TryUpdateModelAsync(viewModel, Prefix))
    {
      if (string.IsNullOrWhiteSpace(viewModel.Label))
      {
        var partName = context.TypePartDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Label),
          S["Label of {0} is empty", partName]
        );
        return Edit(part, context);
      }

      var modelFieldAndPropertyNames =
        typeof(EgaugeMeasurementModel).GetFieldAndPropertyNames();
      if (!modelFieldAndPropertyNames.Any(name => name == viewModel.Field))
      {
        var partName = context.TypePartDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Field),
          S[
            "Field of {0} has to be one of: {1}",
            partName,
            string.Join(", ", modelFieldAndPropertyNames)
          ]
        );
        return Edit(part, context);
      }

      part.ContentItem.DisplayText = viewModel.Label;
    }

    return Edit(part, context);
  }

  public ContentMenuItemPartDisplayDriver(
    IStringLocalizer<ContentMenuItemPartDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  private readonly IStringLocalizer S;
}
