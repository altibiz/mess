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
using Mess.Chart.Providers;
using Mess.Chart.Abstractions.Providers;

namespace Mess.Chart.Drivers;

public class TimeseriesChartPartDisplayDriver
  : ContentPartDisplayDriver<TimeseriesChartPart>
{
  public override IDisplayResult Edit(
    TimeseriesChartPart part,
    BuildPartEditorContext context
  )
  {
    var chartDataProviders = _serviceProvider
      .GetServices<IChartProvider>()
      .Where(
        chartDataProvider =>
          chartDataProvider.ContentType != PreviewChartDataProvider.ProviderId
      );
    return Initialize<TimeseriesChartPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.ChartContentType =
          part.ChartContentType ?? chartDataProviders.First().ContentType;
        model.ChartContentTypeOptions = chartDataProviders
          .Select(
            chartDataProvider =>
              new SelectListItem
              {
                Text = chartDataProvider.ContentType,
                Value = chartDataProvider.ContentType
              }
          )
          .ToList();
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
        model => model.ChartContentType
      )
    )
    {
      if (part.ChartContentType != viewModel.ChartContentType)
      {
        part.Datasets = new();
        part.ChartContentType = viewModel.ChartContentType;
      }
    }

    return Edit(part, context);
  }

  public TimeseriesChartPartDisplayDriver(
    IServiceProvider serviceProvider,
    IStringLocalizer<TimeseriesChartPartDisplayDriver> localizer,
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
