using Mess.ContentFields.Abstractions.Fields;
using Mess.ContentFields.Abstractions.Settings;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.ContentFields.Settings
{
  public class ApiKeyFieldSettingsDriver
    : ContentPartFieldDefinitionDisplayDriver<ApiKeyField>
  {
    public override IDisplayResult Edit(
      ContentPartFieldDefinition partFieldDefinition
    )
    {
      return Initialize<ApiKeyFieldSettings>(
          "ApiKeyFieldSettings_Edit",
          model => partFieldDefinition.PopulateSettings(model)
        )
        .Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(
      ContentPartFieldDefinition partFieldDefinition,
      UpdatePartFieldEditorContext context
    )
    {
      var model = new ApiKeyFieldSettings();

      await context.Updater.TryUpdateModelAsync(model, Prefix);

      context.Builder.WithSettings(model);

      return Edit(partFieldDefinition);
    }
  }
}
