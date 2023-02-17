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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mess.Chart.Drivers;

public class ChartPartDisplayDriver : ContentPartDisplayDriver<ChartPart>
{
  public override IDisplayResult Edit(
    ChartPart part,
    BuildPartEditorContext context
  )
  {
    return Initialize<ChartPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Part = part;
        model.Definition = context.TypePartDefinition;
        model.ProviderIdOptions = _lookup.Ids
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
        model.DataProviderId ??= model.ProviderIdOptions.First().Value;
        model.ChartContentTypes = _contentDefinitionManager
          .ListTypeDefinitions()
          .Where(
            contentTypeDefinition =>
              contentTypeDefinition.GetStereotype() == "ConcreteChart"
          )
          .ToList();
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
        model => model.DataProviderId
      ) && !String.IsNullOrWhiteSpace(viewModel.DataProviderId)
    )
    {
      var dataProvider = _lookup.Get(viewModel.DataProviderId);
      if (dataProvider is null)
      {
        var partName = context.TypePartDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.DataProviderId),
          S["{0} doesn't contain a valid chart provider", partName]
        );
        return Edit(part, context);
      }

      part.DataProviderId = viewModel.DataProviderId;
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
