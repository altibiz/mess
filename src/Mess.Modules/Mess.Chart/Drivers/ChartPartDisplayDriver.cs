using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Models;
using Mess.Chart.Settings;
using Mess.Chart.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
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
        m =>
        {
          m.Parameters = part.Parameters;
          m.ChartPart = part;
          m.ContentItem = part.ContentItem;
        }
      )
      .Location("Detail", "Content")
      .Location("Summary", "Content");
  }

  public override IDisplayResult Edit(
    ChartPart part,
    BuildPartEditorContext context
  )
  {
    var settings = context.TypePartDefinition.GetSettings<ChartPartSettings>();
    var provider = _lookup.Get(settings.Provider);

    return Initialize<ChartPartViewModel>(
      provider is null
        ? GetEditorShapeType(context)
        : provider.GetPartEditorShapeType(context),
      model =>
      {
        model.Parameters = part.Parameters;
        model.ContentItem = part.ContentItem;
        model.ChartPart = part;
        model.TypePartDefinition = context.TypePartDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ChartPart part,
    IUpdateModel updater,
    UpdatePartEditorContext context
  )
  {
    var viewModel = new ChartPartViewModel();

    var settings = context.TypePartDefinition.GetSettings<ChartPartSettings>();
    var provider = _lookup.Get(settings.Provider);
    if (provider is null)
    {
      var partName = context.TypePartDefinition.DisplayName();
      updater.ModelState.AddModelError(
        Prefix,
        nameof(settings.Provider),
        S["{0} doesn't contain a valid Chart provider", partName]
      );
      return Edit(part, context);
    }

    if (await updater.TryUpdateModelAsync(viewModel, Prefix, t => t.Parameters))
    {
      var errors = await provider.ValidateParametersAsync(viewModel.Parameters);
      if (errors is not null)
      {
        var partName = context.TypePartDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Parameters),
          S[
            "{0} doesn't contain a valid parameters",
            partName,
            string.Join(" ", errors)
          ]
        );
        return Edit(part, context);
      }

      part.Parameters = viewModel.Parameters;
    }

    return Edit(part, context);
  }

  public ChartPartDisplayDriver(
    IChartProviderLookup lookup,
    IStringLocalizer<ChartPartDisplayDriver> localizer
  )
  {
    _lookup = lookup;
    S = localizer;
  }

  private readonly IStringLocalizer S;
  private readonly IChartProviderLookup _lookup;
}
