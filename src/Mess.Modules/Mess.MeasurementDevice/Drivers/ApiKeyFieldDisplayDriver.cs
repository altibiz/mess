using Mess.MeasurementDevice.Abstractions.Fields;
using Mess.MeasurementDevice.Abstractions.Security;
using Mess.MeasurementDevice.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;

namespace Mess.MeasurementDevice.Drivers;

public class ApiKeyFieldDisplayDriver : ContentFieldDisplayDriver<ApiKeyField>
{
  public override IDisplayResult Display(
    ApiKeyField field,
    BuildFieldDisplayContext context
  )
  {
    return Initialize<ApiKeyFieldViewModel>(
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
    ApiKeyField field,
    BuildFieldEditorContext context
  )
  {
    return Initialize<ApiKeyFieldEditViewModel>(
      GetEditorShapeType(context),
      model =>
      {
        model.Field = field;
        model.Value = "";
        model.Part = context.ContentPart;
        model.PartFieldDefinition = context.PartFieldDefinition;
      }
    );
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ApiKeyField field,
    IUpdateModel updater,
    UpdateFieldEditorContext context
  )
  {
    if (await updater.TryUpdateModelAsync(field, Prefix, f => f.Value))
    {
      if (String.IsNullOrWhiteSpace(field.Value))
      {
        updater.ModelState.AddModelError(
          Prefix,
          nameof(field.Value),
          S[
            "A value is required for {0}.",
            context.PartFieldDefinition.DisplayName()
          ]
        );
      }

      var salt = await _measurementDeviceGuard.GenerateApiKeySaltAsync();

      var hash = await _measurementDeviceGuard.HashApiKeyAsync(
        field.Value,
        salt
      );

      field.Hash = hash;
      field.Salt = salt;
    }

    return Edit(field, context);
  }

  public ApiKeyFieldDisplayDriver(
    IStringLocalizer<ApiKeyFieldDisplayDriver> localizer,
    IMeasurementDeviceGuard measurementDeviceGuard
  )
  {
    S = localizer;
    _measurementDeviceGuard = measurementDeviceGuard;
  }

  private readonly IStringLocalizer S;

  private readonly IMeasurementDeviceGuard _measurementDeviceGuard;
}
