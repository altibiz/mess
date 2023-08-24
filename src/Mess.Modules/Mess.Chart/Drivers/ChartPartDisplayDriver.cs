using Mess.Chart.Abstractions.Models;
using Mess.Chart.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.ContentManagement.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mess.Chart.Abstractions.Providers;
using Microsoft.Extensions.DependencyInjection;
using YesSql;
using Mess.Chart.Indexes;
using OrchardCore.Mvc.ModelBinding;

namespace Mess.Chart.Drivers;

public class ChartPartDisplayDriver : ContentPartDisplayDriver<ChartPart>
{
  public override IDisplayResult Display(
    ChartPart part,
    BuildPartDisplayContext context
  )
  {
    return Initialize<ChartPartViewModel>(
        GetDisplayShapeType(context),
        model =>
        {
          model.ChartContentItemId = part.ChartContentItemId;
          model.Part = part;
          model.Definition = context.TypePartDefinition;
        }
      )
      .Location("Detail", "Content");
  }

  public override async Task<IDisplayResult> EditAsync(
    ChartPart part,
    BuildPartEditorContext context
  )
  {
    var chartProvider = _serviceProvider
      .GetServices<IChartProvider>()
      .FirstOrDefault(
        provider => provider.ContentType == part.ContentItem.ContentType
      );
    if (chartProvider == null)
    {
      throw new NotImplementedException(
        "No chart provider implemented for this content type."
      );
    }

    var charts = await _session
      .QueryIndex<ChartIndex>()
      .Where(chartIndex => chartIndex.ContentType == chartProvider.ContentType)
      .ListAsync();

    return Initialize<ChartPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.ChartContentItemId =
          part.ChartContentItemId
          ?? charts.FirstOrDefault()?.ChartContentItemId!;
        model.ChartContentItemIdOptions = charts
          .Select(
            chart =>
              new SelectListItem
              {
                Text = chart.Title ?? chart.ChartContentItemId,
                Value = chart.ChartContentItemId
              }
          )
          .ToList();
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ChartPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new ChartPartEditViewModel();

    if (
      await updater.TryUpdateModelAsync(
        viewModel,
        Prefix,
        model => model.ChartContentItemId
      )
    )
    {
      if (String.IsNullOrWhiteSpace(viewModel.ChartContentItemId))
      {
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.ChartContentItemId),
          S["A value is required for {0}.", context.TypePartDefinition.Name]
        );
      }

      part.ChartContentItemId = part.ChartContentItemId;
    }

    return await EditAsync(part, context);
  }

  public ChartPartDisplayDriver(
    IServiceProvider serviceProvider,
    IStringLocalizer<ChartPartDisplayDriver> localizer,
    ISession session
  )
  {
    _serviceProvider = serviceProvider;
    _session = session;
    S = localizer;
  }

  private readonly IStringLocalizer S;
  private readonly IServiceProvider _serviceProvider;
  private readonly ISession _session;
}
