using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Models;
using Mess.Chart.ViewModels;
using Mess.Chart.Abstractions.Providers;
using Microsoft.Extensions.Localization;

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
          var settings =
            contentTypePartDefinition.GetSettings<ChartPartSettings>();

          model.Provider = settings.Provider;
        }
      )
      .Location("Content");
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ContentTypePartDefinition contentTypePartDefinition,
    UpdateTypePartEditorContext context
  )
  {
    var model = new ChartFieldSettingsViewModel();
    var settings = new ChartFieldSettings();

    if (await context.Updater.TryUpdateModelAsync(model, Prefix))
    {
      if (!_lookup.Exists(model.Provider))
      {
        context.Updater.ModelState.AddModelError(
          Prefix,
          S["Provider does not exist"]
        );
        return Edit(contentTypePartDefinition);
      }

      settings.Provider = model.Provider;

      context.Builder.WithSettings(settings);
    }

    return Edit(contentTypePartDefinition);
  }

  public ChartPartSettingsDisplayDriver(
    IChartProviderLookup lookup,
    IStringLocalizer<ChartFieldSettingsDriver> localizer
  )
  {
    _lookup = lookup;
    S = localizer;
  }

  private readonly IChartProviderLookup _lookup;
  private readonly IStringLocalizer S;
}
