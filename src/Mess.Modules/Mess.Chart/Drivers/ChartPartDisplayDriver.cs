using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Abstractions.Models;
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
      model =>
      {
        model.Part = part;
        model.Definition = context.TypePartDefinition;
      }
    );
  }

  public override IDisplayResult Edit(
    ChartPart part,
    BuildPartEditorContext context
  )
  {
    return Initialize<ChartPartViewModel>(
      GetEditorShapeType(context),
      model =>
      {
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
    var viewModel = new ChartPartViewModel();

    if (await updater.TryUpdateModelAsync(viewModel, Prefix))
    {
      var dataProvider = _lookup.Get(viewModel.Part.DataProviderId);
      if (dataProvider is null)
      {
        var partName = context.TypePartDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Part.DataProviderId),
          S["{0} doesn't contain a valid chart provider", partName]
        );
        return Edit(part, context);
      }
    }

    return Edit(part, context);
  }

  public ChartPartDisplayDriver(
    IChartDataProviderLookup lookup,
    IStringLocalizer<ChartPartDisplayDriver> localizer
  )
  {
    _lookup = lookup;
    S = localizer;
  }

  private readonly IStringLocalizer S;
  private readonly IChartDataProviderLookup _lookup;
}
