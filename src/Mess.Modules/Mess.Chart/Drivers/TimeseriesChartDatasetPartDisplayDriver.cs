using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;
using OrchardCore.ContentManagement.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Chart.Drivers;

public class TimeseriesChartDatasetPartDisplayDriver
  : ContentPartDisplayDriver<TimeseriesChartDatasetPart>
{
  public override IDisplayResult Display(
    TimeseriesChartDatasetPart part,
    BuildPartDisplayContext context
  )
  {
    return Initialize<TimeseriesChartDatasetPartViewModel>(
      GetDisplayShapeType(context),
      model =>
      {
        model.Property = part.Property;
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override IDisplayResult Edit(
    TimeseriesChartDatasetPart part,
    BuildPartEditorContext context
  )
  {
    return Initialize<TimeseriesChartDatasetPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Property = part.Property;
        model.PropertyOptions = new List<SelectListItem>
        {
          new SelectListItem { Text = "Test", Value = "test" }
        };
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    TimeseriesChartDatasetPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new TimeseriesChartDatasetPartEditViewModel();

    if (
      await updater.TryUpdateModelAsync(
        viewModel,
        Prefix,
        model => model.Property
      )
    )
    {
      part.Property = viewModel.Property;
    }

    return Edit(part, context);
  }

  public TimeseriesChartDatasetPartDisplayDriver(
    IStringLocalizer<TimeseriesChartDatasetPartDisplayDriver> localizer,
    IContentDefinitionManager contentDefinitionManager
  )
  {
    S = localizer;
    _contentDefinitionManager = contentDefinitionManager;
  }

  private readonly IStringLocalizer S;
  private readonly IContentDefinitionManager _contentDefinitionManager;
}
