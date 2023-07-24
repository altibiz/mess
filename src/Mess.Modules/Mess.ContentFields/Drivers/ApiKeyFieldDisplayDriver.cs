using Mess.ContentFields.Abstractions.Fields;
using Mess.ContentFields.Abstractions.Services;
using Mess.ContentFields.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;

namespace Mess.ContentFields.Drivers;

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
        model.Value = field.Value;
        model.Field = field;
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
    var model = new ApiKeyFieldEditViewModel { };

    if (await updater.TryUpdateModelAsync(model, Prefix, f => f.Value))
    {
      if (String.IsNullOrWhiteSpace(model.Value))
      {
        updater.ModelState.AddModelError(
          Prefix,
          nameof(model.Value),
          S[
            "A value is required for {0}.",
            context.PartFieldDefinition.DisplayName()
          ]
        );
      }
      else
      {
        var salt = await _apiKeyFieldService.GenerateApiKeySaltAsync();
        var hash = await _apiKeyFieldService.HashApiKeyAsync(model.Value, salt);
        field.Hash = hash;
        field.Salt = salt;
      }

      field.Value = model.Value;
    }

    return Edit(field, context);
  }

  public ApiKeyFieldDisplayDriver(
    IStringLocalizer<ApiKeyFieldDisplayDriver> localizer,
    IApiKeyFieldService apiKeyFieldService
  )
  {
    S = localizer;
    _apiKeyFieldService = apiKeyFieldService;
  }

  private readonly IStringLocalizer S;

  private readonly IApiKeyFieldService _apiKeyFieldService;
}
