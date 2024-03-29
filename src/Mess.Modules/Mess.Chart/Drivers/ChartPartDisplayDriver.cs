using Mess.Chart.Abstractions.Models;
using Mess.Chart.Indexes;
using Mess.Chart.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;
using YesSql;

namespace Mess.Chart.Drivers;

public class ChartPartDisplayDriver : ContentPartDisplayDriver<ChartPart>
{
  private readonly ISession _session;

  private readonly IStringLocalizer S;

  public ChartPartDisplayDriver(
    IStringLocalizer<ChartPartDisplayDriver> localizer,
    ISession session
  )
  {
    _session = session;
    S = localizer;
  }

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
    var charts = await _session
      .QueryIndex<ChartIndex>()
      .Where(
        chartIndex => chartIndex.ContentType == part.ContentItem.ContentType
      )
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
      if (string.IsNullOrWhiteSpace(viewModel.ChartContentItemId))
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.ChartContentItemId),
          S["A value is required for {0}.", context.TypePartDefinition.Name]
        );

      part.ChartContentItemId = viewModel.ChartContentItemId;
    }

    return await EditAsync(part, context);
  }
}
