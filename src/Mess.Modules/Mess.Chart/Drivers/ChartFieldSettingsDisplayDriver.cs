using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using Mess.Chart.Models;
using Mess.Chart.ViewModels;
using Mess.Chart.Abstractions.Providers;
using Microsoft.Extensions.Localization;

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

          model.Provider = settings.Provider;
        }
      )
      .Location("Content");
  }

  public override async Task<IDisplayResult> UpdateAsync(
    ContentPartFieldDefinition partFieldDefinition,
    UpdatePartFieldEditorContext context
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
        return Edit(partFieldDefinition);
      }

      settings.Provider = model.Provider;

      context.Builder.WithSettings(settings);
    }

    return Edit(partFieldDefinition);
  }

  public ChartFieldSettingsDriver(
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
