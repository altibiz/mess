using Mess.Fields.Abstractions;
using Mess.Fields.Abstractions.Fields;
using Mess.Fields.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;

namespace Mess.Fields.Drivers;

public class IntervalFieldDisplayDriver
  : ContentFieldDisplayDriver<IntervalField>
{
  private readonly IStringLocalizer S;

  public IntervalFieldDisplayDriver(
    IStringLocalizer<IntervalFieldDisplayDriver> localizer
  )
  {
    S = localizer;
  }

  public override IDisplayResult Display(
    IntervalField field,
    BuildFieldDisplayContext fieldDisplayContext
  )
  {
    return Initialize<IntervalFieldViewModel>(
        GetDisplayShapeType(fieldDisplayContext),
        model =>
        {
          model.Field = field;
          model.Part = fieldDisplayContext.ContentPart;
          model.PartFieldDefinition = fieldDisplayContext.PartFieldDefinition;
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
        model.Value = field.Value ?? new Interval(IntervalUnit.Minute, 5);
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
                Selected = unit == model.Value.Unit
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
    var viewModel = new IntervalFieldEditViewModel();

    if (await updater.TryUpdateModelAsync(viewModel, Prefix, f => f.Value))
    {
      if (viewModel.Value == null)
        updater.ModelState.AddModelError(
          Prefix,
          nameof(viewModel.Value),
          S[
            "A value is required for {0}.",
            context.PartFieldDefinition.DisplayName()
          ]
        );
      else
        field.Value = viewModel.Value;
    }

    return Edit(field, context);
  }
}
