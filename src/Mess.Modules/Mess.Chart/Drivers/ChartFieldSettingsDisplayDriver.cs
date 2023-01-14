using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Fields;
using Mess.Chart.Settings;
using Mess.Chart.ViewModels;

namespace Mess.Chart.Drivers;

public class ChartFieldSettingsDriver
  : ContentPartFieldDefinitionDisplayDriver<ChartField>
{
  public override IDisplayResult Edit(
    ContentPartFieldDefinition partFieldDefinition
  )
  {
    return Initialize<ChartFieldSettingsViewModel>(
        "ChartFieldSettings_Edit",
        model =>
        {
          var settings = partFieldDefinition.GetSettings<ChartFieldSettings>();

          // TODO: update
          // model.SanitizeHtml = settings.SanitizeHtml;
          // model.Hint = settings.Hint;
        }
      )
      .Location("Content:20");
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ContentPartFieldDefinition partFieldDefinition,
    UpdatePartFieldEditorContext context
  )
  {
    var model = new ChartFieldSettingsViewModel();
    var settings = new ChartFieldSettings();

    await context.Updater.TryUpdateModelAsync(model, Prefix);

    // TODO: update
    // settings.SanitizeHtml = model.SanitizeHtml;
    // settings.Hint = model.Hint;

    context.Builder.WithSettings(settings);

    return Edit(partFieldDefinition);
  }
}
