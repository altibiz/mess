using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;
using OrchardCore.ContentManagement.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Mess.Chart.Abstractions.Providers;

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
      )
      .Location("Content");
  }

  public override IDisplayResult Edit(
    TimeseriesChartDatasetPart part,
    BuildPartEditorContext context
  )
  {
    var chartDataProviderId = (string)
      part.ContentItem.Content.ChartDataProviderId;
    var chartDataProvider = _serviceProvider
      .GetServices<IChartDataProvider>()
      .First(x => x.Id == chartDataProviderId);

    return Initialize<TimeseriesChartDatasetPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Property =
          part.Property
          ?? chartDataProvider.TimeseriesChartDatasetProperties.First();
        model.PropertyOptions =
          chartDataProvider.TimeseriesChartDatasetProperties
            .Select(
              property =>
                new SelectListItem { Text = property, Value = property }
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
      part.Property = viewModel.Property;
    }

    return Edit(part, context);
  }

  public TimeseriesChartDatasetPartDisplayDriver(
    IServiceProvider serviceProvider,
    IStringLocalizer<TimeseriesChartDatasetPartDisplayDriver> localizer,
    IContentDefinitionManager contentDefinitionManager
  )
  {
    S = localizer;
    _contentDefinitionManager = contentDefinitionManager;
    _serviceProvider = serviceProvider;
  }

  private readonly IStringLocalizer S;
  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IServiceProvider _serviceProvider;
}
