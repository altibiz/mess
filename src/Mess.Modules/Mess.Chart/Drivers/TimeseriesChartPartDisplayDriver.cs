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

public class TimeseriesChartPartDisplayDriver
  : ContentPartDisplayDriver<TimeseriesChartPart>
{
  public override IDisplayResult Edit(
    TimeseriesChartPart part,
    BuildPartEditorContext context
  )
  {
    return Initialize<TimeseriesChartPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.DataProviderId = part.DataProviderId;
        model.DataProviderIdOptions = new List<SelectListItem>
        {
          new SelectListItem { Text = "Test", Value = "test" }
        };
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    TimeseriesChartPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new TimeseriesChartPartEditViewModel();

    if (
      await updater.TryUpdateModelAsync(
        viewModel,
        Prefix,
        model => model.DataProviderId
      )
    )
    {
      part.DataProviderId = viewModel.DataProviderId;
    }

    return Edit(part, context);
  }

  public TimeseriesChartPartDisplayDriver(
    IStringLocalizer<TimeseriesChartPartDisplayDriver> localizer,
    IContentDefinitionManager contentDefinitionManager
  )
  {
    S = localizer;
    _contentDefinitionManager = contentDefinitionManager;
  }

  private readonly IStringLocalizer S;
  private readonly IContentDefinitionManager _contentDefinitionManager;
}
