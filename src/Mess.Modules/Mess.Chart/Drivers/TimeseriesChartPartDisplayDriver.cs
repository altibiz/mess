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
using Mess.Chart.Abstractions.Services;
using OrchardCore.Mvc.ModelBinding;

namespace Mess.Chart.Drivers;

public class TimeseriesChartPartDisplayDriver
  : ContentPartDisplayDriver<TimeseriesChartPart>
{
  public override IDisplayResult Edit(
    TimeseriesChartPart part,
    BuildPartEditorContext context
  )
  {
    var chartProviders = _serviceProvider
      .GetServices<IChartFactory>()
      .Where(
        chartDataProvider =>
          chartDataProvider.ContentType != PreviewChartProvider.ChartContentType
      );
    return Initialize<TimeseriesChartPartEditViewModel>(
        GetEditorShapeType(context),
        model =>
        {
          model.ChartContentType =
            part.ChartContentType ?? chartProviders.First().ContentType;
          model.ChartContentTypeOptions = chartProviders
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
      )
      .Location("Content");
  }

  public override async Task<IDisplayResult> UpdateAsync(
    TimeseriesChartPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var chartProviders = _serviceProvider
      .GetServices<IChartFactory>()
      .Where(
        chartDataProvider =>
          chartDataProvider.ContentType != PreviewChartProvider.ChartContentType
      );
    var viewModel = new TimeseriesChartPartEditViewModel();

    if (
      await updater.TryUpdateModelAsync(
        viewModel,
        Prefix,
        model => model.ChartContentType
      )
    )
    {
      if (
        !chartProviders
          .Select(provider => provider.ContentType)
          .Contains(viewModel.ChartContentType)
      )
      {
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.ChartContentType),
          S["Invalid value for {0}", context.TypePartDefinition.Name]
        );
      }
      else
      {
        if (part.ChartContentType != viewModel.ChartContentType)
        {
          part.Datasets = new();
        }
      }

      part.ChartContentType = viewModel.ChartContentType;
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
