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
using Mess.Chart.Abstractions.Services;
using OrchardCore.Mvc.ModelBinding;

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
    string contentType = part.ContentItem.Content.ChartContentType;
    var chartProvider = _serviceProvider
      .GetServices<IChartFactory>()
      .FirstOrDefault(provider => provider.ContentType == contentType) ??
      throw new NotImplementedException(
        "No chart provider implemented for this content type."
      );

    return Initialize<TimeseriesChartDatasetPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Property =
          part.Property
          ?? chartProvider.TimeseriesChartDatasetProperties.FirstOrDefault()!;
        model.PropertyOptions = chartProvider.TimeseriesChartDatasetProperties
          .Select(
            property => new SelectListItem { Text = property, Value = property }
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
    string contentType = part.ContentItem.Content.ChartContentType;
    var chartProvider = _serviceProvider
      .GetServices<IChartFactory>()
      .FirstOrDefault(provider => provider.ContentType == contentType) ??
      throw new NotImplementedException(
        "No chart provider implemented for this content type."
      );

    var viewModel = new TimeseriesChartDatasetPartEditViewModel();

    if (
      await updater.TryUpdateModelAsync(
        viewModel,
        Prefix,
        model => model.Property
      )
    )
    {
      if (
        !chartProvider.TimeseriesChartDatasetProperties.Contains(
          viewModel.Property
        )
      )
      {
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Property),
          S["Invalid value for {0}", context.TypePartDefinition.Name]
        );
      }

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
