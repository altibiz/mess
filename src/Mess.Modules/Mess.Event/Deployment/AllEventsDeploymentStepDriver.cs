using Mess.Event.ViewModels;
using OrchardCore.Deployment;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace Mess.Event.Deployment;

public class AllEventsDeploymentStepDriver
  : DisplayDriver<DeploymentStep, AllEventsDeploymentStep>
{
  public override IDisplayResult Display(AllEventsDeploymentStep model)
  {
    return Combine(
      View("AllEventsDeploymentStep_Summary", model)
        .Location("Summary", "Content"),
      View("AllEventsDeploymentStep_Thumbnail", model)
        .Location("Thumbnail", "Content")
    );
  }

  public override IDisplayResult Edit(AllEventsDeploymentStep model)
  {
    return Initialize<AllEventsDeploymentStepViewModel>(
        "AllEventsDeploymentStep_Fields_Edit",
        model => { }
      )
      .Location("Content");
  }

  public override async Task<IDisplayResult> UpdateAsync(
    AllEventsDeploymentStep model,
    IUpdateModel updater
  )
  {
    await updater.TryUpdateModelAsync(model, Prefix);

    return Edit(model);
  }
}
