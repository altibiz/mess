using Mess.Chart.Abstractions.Providers;
using Mess.Chart.Models;
using Mess.Chart.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement;
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
          model.Parameters = part.Parameters;
          model.Part = part;
          model.ContentItem = part.ContentItem;
          model.TypePartDefinition = context.TypePartDefinition;
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
    if (provider is null)
    {
      return Initialize<ChartPartViewModel>(
        GetEditorShapeType(context),
        model =>
        {
          model.Parameters = part.Parameters;
          model.Part = part;
          model.ContentItem = part.ContentItem;
          model.TypePartDefinition = context.TypePartDefinition;
        }
      );
    }

    var shapeType = provider.GetPartEditorShapeType(context);
    var model = provider.CreatePartEditorModel(context, part, part.Parameters);
    return Factory(
      shapeType,
      ctx => ctx.ShapeFactory.CreateAsync(shapeType, Arguments.From(model))
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
