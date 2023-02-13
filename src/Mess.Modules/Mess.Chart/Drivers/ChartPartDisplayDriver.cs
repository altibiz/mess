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
using OrchardCore.ContentManagement.Metadata;
using Mess.System.Extensions.Object;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        model.ProviderIds = _lookup.Ids
          .Select(
            (id, index) =>
              new SelectListItem()
              {
                Value = id,
                Text = id,
                Selected = model.Part.DataProviderId is not null
                  ? model.Part.DataProviderId == id
                  : index == 0,
                Disabled = false
              }
          )
          .ToList();
        model.Part.DataProviderId ??= model.ProviderIds.First().Value;
        model.ChartContentTypes = _contentDefinitionManager
          .ListTypeDefinitions()
          .Where(
            contentTypeDefinition =>
              contentTypeDefinition.GetStereotype() == "Chart"
          );
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
        model.ProviderIds = _lookup.Ids
          .Select(
            (id, index) =>
              new SelectListItem()
              {
                Value = id,
                Text = id,
                Selected = model.Part.DataProviderId is not null
                  ? model.Part.DataProviderId == id
                  : index == 0,
                Disabled = false
              }
          )
          .ToList();
        model.Part.DataProviderId ??= model.ProviderIds.First().Value;
        model.ChartContentTypes = _contentDefinitionManager
          .ListTypeDefinitions()
          .Where(
            contentTypeDefinition =>
              contentTypeDefinition.GetStereotype() == "Chart"
          );
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
    IContentDefinitionManager contentDefinitionManager,
    IStringLocalizer<ChartPartDisplayDriver> localizer
  )
  {
    _lookup = lookup;
    _contentDefinitionManager = contentDefinitionManager;
    S = localizer;
  }

  private readonly IStringLocalizer S;
  private readonly IChartDataProviderLookup _lookup;
  private readonly IContentDefinitionManager _contentDefinitionManager;
}
