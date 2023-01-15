using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Fields;
using Mess.Chart.ViewModels;
using Mess.Chart.Settings;
using Mess.Chart.Abstractions.Providers;
using OrchardCore.ContentManagement.Metadata.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.Mvc.ModelBinding;

namespace Mess.Chart.Drivers;

public class ChartFieldDisplayDriver : ContentFieldDisplayDriver<ChartField>
{
  public override IDisplayResult Display(
    ChartField field,
    BuildFieldDisplayContext context
  )
  {
    return Initialize<DisplayChartFieldViewModel>(
        GetDisplayShapeType(context),
        m =>
        {
          m.Parameters = field.Parameters;
          m.Field = field;
          m.Part = context.ContentPart;
          m.PartFieldDefinition = context.PartFieldDefinition;
        }
      )
      .Location("Detail", "Content")
      .Location("Summary", "Content");
  }

  public override IDisplayResult Edit(
    ChartField field,
    BuildFieldEditorContext context
  )
  {
    var settings = context.TypePartDefinition.GetSettings<ChartPartSettings>();
    var provider = _lookup.Get(settings.Provider);

    return Initialize<EditChartFieldViewModel>(
      provider is null
        ? GetEditorShapeType(context)
        : provider.GetFieldEditorShapeType(context),
      model =>
      {
        model.Parameters = field.Parameters;
        model.Field = field;
        model.Part = context.ContentPart;
        model.PartFieldDefinition = context.PartFieldDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ChartField field,
    IUpdateModel updater,
    UpdateFieldEditorContext context
  )
  {
    var viewModel = new EditChartFieldViewModel();

    var settings =
      context.PartFieldDefinition.GetSettings<ChartFieldSettings>();

    var provider = _lookup.Get(settings.Provider);
    if (provider is null)
    {
      var fieldName = context.PartFieldDefinition.DisplayName();
      updater.ModelState.AddModelError(
        Prefix,
        nameof(settings.Provider),
        S["{0} doesn't contain a valid Chart provider", fieldName]
      );
      return Edit(field, context);
    }

    if (await updater.TryUpdateModelAsync(viewModel, Prefix, t => t.Parameters))
    {
      var errors = await provider.ValidateParametersAsync(viewModel.Parameters);
      if (errors is not null)
      {
        var fieldName = context.PartFieldDefinition.DisplayName();
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Parameters),
          S[
            "{0} doesn't contain a valid parameters",
            fieldName,
            string.Join(" ", errors)
          ]
        );
        return Edit(field, context);
      }

      field.Parameters = viewModel.Parameters;
    }

    return Edit(field, context);
  }

  public ChartFieldDisplayDriver(
    IChartProviderLookup lookup,
    IStringLocalizer<ChartFieldDisplayDriver> localizer
  )
  {
    S = localizer;
    _lookup = lookup;
  }

  private readonly IStringLocalizer S;
  private readonly IChartProviderLookup _lookup;
}
