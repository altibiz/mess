using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Models;
using Mess.Chart.ViewModels;
using Mess.Chart.Settings;

namespace Mess.Chart.Drivers;

public class ChartPartSettingsDisplayDriver
  : ContentTypePartDefinitionDisplayDriver<ChartPart>
{
  public override IDisplayResult Edit(
    ContentTypePartDefinition contentTypePartDefinition,
    IUpdateModel updater
  )
  {
    return Initialize<ChartPartSettingsViewModel>(
        "ChartPartSettings_Edit",
        model =>
        {
          // TODO: processing
          // var settings =
          //   contentTypePartDefinition.GetSettings<ChartPartSettings>();
          //
          // model.SanitizeHtml = settings.SanitizeHtml;
        }
      )
      .Location("Content:20");
  }

#pragma warning disable CS1998 // async method lacks await
  public override async Task<IDisplayResult> UpdateAsync(
    ContentTypePartDefinition contentTypePartDefinition,
    UpdateTypePartEditorContext context
  )
  {
    var model = new ChartPartSettingsViewModel();
    var settings = new ChartPartSettings();

    // TODO: processing
    // if (await context.Updater.TryUpdateModelAsync(model, Prefix))
    // {
    //   settings.SanitizeHtml = model.SanitizeHtml;
    //
    //   context.Builder.WithSettings(settings);
    // }

    return Edit(contentTypePartDefinition, context.Updater);
  }
#pragma warning restore CS1998 // async method lacks await
}
