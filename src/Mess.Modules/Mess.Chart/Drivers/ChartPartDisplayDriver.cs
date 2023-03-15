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
  public override IDisplayResult Display(
    ChartPart part,
    BuildPartDisplayContext context
  )
  {
    return Initialize<ChartPartViewModel>(
      GetDisplayShapeType(context),
      model =>
      {
        model.DataProviderId = part.DataProviderId;
        model.ChartContentItemId = part.ChartContentItemId;
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
    return Initialize<ChartPartEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.DataProviderId = part.DataProviderId;
        model.DataProviderIdOptions = new List<SelectListItem>
        {
          new SelectListItem { Text = "Test", Value = "test" }
        };
        model.ChartContentItemId = part.ChartContentItemId;
        model.ChartContentItemIdOptions = new List<SelectListItem>
        {
          new SelectListItem { Text = "Test", Value = "test" }
        };
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
        model => model.DataProviderId,
        model => model.ChartContentItemId
      )
    )
    {
      part.DataProviderId = viewModel.DataProviderId;
      part.ChartContentItemId = viewModel.ChartContentItemId;
    }

    return Edit(part, context);
  }

  public ChartPartDisplayDriver(
    IContentDefinitionManager contentDefinitionManager,
    IStringLocalizer<ChartPartDisplayDriver> localizer
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    S = localizer;
  }

  private readonly IStringLocalizer S;
  private readonly IContentDefinitionManager _contentDefinitionManager;
}
