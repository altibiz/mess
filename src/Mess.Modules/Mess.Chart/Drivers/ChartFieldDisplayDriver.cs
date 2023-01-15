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
using OrchardCore.DisplayManagement;

namespace Mess.Chart.Drivers;

public class ChartFieldDisplayDriver : ContentFieldDisplayDriver<ChartField>
{
  public override IDisplayResult Display(
    ChartField field,
    BuildFieldDisplayContext context
  )
  {
    return Initialize<ChartFieldViewModel>(
        GetDisplayShapeType(context),
        model =>
        {
          model.Parameters = field.Parameters;
          model.Field = field;
          model.Part = context.ContentPart;
          model.ContentItem = context.ContentPart.ContentItem;
          model.PartFieldDefinition = context.PartFieldDefinition;
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
    if (provider is null)
    {
      return Initialize<ChartFieldViewModel>(
        GetEditorShapeType(context),
        model =>
        {
          model.Parameters = field.Parameters;
          model.Field = field;
          model.Part = context.ContentPart;
          model.ContentItem = context.ContentPart.ContentItem;
          model.PartFieldDefinition = context.PartFieldDefinition;
        }
      );
    }

    var shapeType = provider.GetFieldEditorShapeType(context);
    var model = provider.CreateFieldEditorModel(
      context,
      field,
      field.Parameters
    );
    return Factory(
      shapeType,
      ctx => ctx.ShapeFactory.CreateAsync(shapeType, Arguments.From(model))
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ChartField field,
    IUpdateModel updater,
    UpdateFieldEditorContext context
  )
  {
    var viewModel = new ChartFieldViewModel();

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
