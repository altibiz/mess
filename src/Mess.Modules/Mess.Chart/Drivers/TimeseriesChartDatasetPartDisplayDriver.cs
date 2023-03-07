using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;
using Mess.Chart.Abstractions.Services;
using Mess.Chart.Abstractions.Providers;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.Mvc.ModelBinding;
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
          model.Part = part;
          model.Definition = context.TypePartDefinition;
        }
      )
      .Location("Content");
  }

  public override IDisplayResult Edit(
    TimeseriesChartDatasetPart part,
    BuildPartEditorContext context
  )
  {
    var chartDataProvider = _chartDataProviderLookup.Get(
      part.ChartDataProviderId
    );
    if (chartDataProvider is null)
    {
      throw new InvalidOperationException(
        "Chart doesn't have a valid data provider"
      );
    }

    return Initialize<TimeseriesChartDatasetPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Property = part.Property;
        model.PropertyOptions = chartDataProvider
          .GetTimeseriesChartDatasetProperties()
          .Select(
            (option, index) =>
              new SelectListItem()
              {
                Value = option,
                Text = option,
                Selected = model.Property is not null
                  ? model.Property == option
                  : index == 0,
                Disabled = false
              }
          )
          .ToList();
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
      var chartDataProvider = _chartDataProviderLookup.Get(
        part.ChartDataProviderId
      );
      if (chartDataProvider is null)
      {
        throw new InvalidOperationException(
          "Chart doesn't have a valid data provider"
        );
      }

      if (
        !chartDataProvider.IsValidTimeseriesChartDatasetProperty(
          viewModel.Property
        )
      )
      {
        var partName = context.TypePartDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Property),
          S["{0} doesn't contain a valid property", partName]
        );
        return Edit(part, context);
      }

      part.Property = viewModel.Property;
    }

    return Edit(part, context);
  }

  public TimeseriesChartDatasetPartDisplayDriver(
    IStringLocalizer<TimeseriesChartDatasetPartDisplayDriver> localizer,
    IChartDataProviderLookup chartDataProviderLookup,
    IContentDefinitionManager contentDefinitionManager
  )
  {
    S = localizer;
    _chartDataProviderLookup = chartDataProviderLookup;
    _contentDefinitionManager = contentDefinitionManager;
  }

  private readonly IStringLocalizer S;
  private readonly IChartDataProviderLookup _chartDataProviderLookup;
  private readonly IContentDefinitionManager _contentDefinitionManager;
}
