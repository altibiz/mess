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
using Mess.Chart.Providers;

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
    var charts = await _session.QueryIndex<ChartIndex>().ListAsync();
    var chartDataProviders = _serviceProvider
      .GetServices<IChartProvider>()
      .Where(
        provider => provider.ContentType != PreviewChartDataProvider.ProviderId
      );

    return Initialize<ChartPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.ChartContentItemId =
          part.ChartContentItemId ?? charts.First().ChartContentItemId!;
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
      part.ChartContentItemId = viewModel.ChartContentItemId;
    }

    return Edit(part, context);
  }

  public ChartPartDisplayDriver(
    IServiceProvider serviceProvider,
    IContentDefinitionManager contentDefinitionManager,
    IStringLocalizer<ChartPartDisplayDriver> localizer,
    ISession session
  )
  {
    _serviceProvider = serviceProvider;
    _contentDefinitionManager = contentDefinitionManager;
    _session = session;
    S = localizer;
  }

  private readonly IStringLocalizer S;
  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IServiceProvider _serviceProvider;
  private readonly ISession _session;
}
