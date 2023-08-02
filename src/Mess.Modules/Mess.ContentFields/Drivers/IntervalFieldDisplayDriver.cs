using Mess.ContentFields.Abstractions;
using Mess.ContentFields.Abstractions.Fields;
using Mess.ContentFields.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;

namespace Mess.ContentFields.Drivers;

public class IntervalFieldDisplayDriver
  : ContentFieldDisplayDriver<IntervalField>
{
  public override IDisplayResult Display(
    IntervalField field,
    BuildFieldDisplayContext context
  )
  {
    return Initialize<IntervalFieldViewModel>(
        GetDisplayShapeType(context),
        model =>
        {
          model.Field = field;
          model.Part = context.ContentPart;
          model.PartFieldDefinition = context.PartFieldDefinition;
        }
      )
      .Location("Detail", "Content")
      .Location("Summary", "Content");
  }

  public override IDisplayResult Edit(
    IntervalField field,
    BuildFieldEditorContext context
  )
  {
    return Initialize<IntervalFieldEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Value = field.Value;
        model.Field = field;
        model.Part = context.ContentPart;
        model.PartFieldDefinition = context.PartFieldDefinition;
        model.UnitOptions = Enum.GetValues<IntervalUnit>()
          .Select(
            unit =>
              new SelectListItem
              {
                Text = unit.ToString(),
                Value = unit.ToString(),
                Selected = unit == field.Value.Unit
              }
          )
          .ToList();
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    IntervalField field,
    IUpdateModel updater,
    UpdateFieldEditorContext context
  )
  {
    var viewModel = new IntervalFieldEditViewModel { };

    if (await updater.TryUpdateModelAsync(viewModel, Prefix, f => f.Value))
    {
      if (viewModel.Value == null)
      {
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Value),
          S[
            "A value is required for {0}.",
            context.PartFieldDefinition.DisplayName()
          ]
        );
      }
      else
      {
        field.Value = viewModel.Value;
      }
    }

    return Edit(field, context);
  }

  public IntervalFieldDisplayDriver(
    IStringLocalizer<IntervalFieldDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  private readonly IStringLocalizer S;
}
