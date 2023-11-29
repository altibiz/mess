using Mess.Fields.Abstractions.Fields;
using Mess.Fields.Abstractions.Settings;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.ContentFields.Settings
{
  public class ApiKeyFieldSettingsDriver
    : ContentPartFieldDefinitionDisplayDriver<ApiKeyField>
  {
    public override IDisplayResult Edit(
      ContentPartFieldDefinition model
    )
    {
      return Initialize<ApiKeyFieldSettings>(
          "ApiKeyFieldSettings_Edit",
          model => model.PopulateSettings(model)
        )
        .Location("Content");
    }

    public override async Task<IDisplayResult> UpdateAsync(
      ContentPartFieldDefinition model,
      UpdatePartFieldEditorContext context
    )
    {
      var settings = new ApiKeyFieldSettings();

      if (await context.Updater.TryUpdateModelAsync(settings, Prefix))
      {
        context.Builder.WithSettings(settings);
      }

      return Edit(model);
    }
  }
}
