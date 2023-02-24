using OrchardCore.Deployment;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using Mess.EventStore.ViewModels;

namespace Mess.EventStore.Deployment;

public class AllEventsDeploymentStepDriver
  : DisplayDriver<DeploymentStep, AllEventsDeploymentStep>
{
  public override IDisplayResult Display(AllEventsDeploymentStep step)
  {
    return Combine(
      View("AllEventsDeploymentStep_Summary", step)
        .Location("Summary", "Content"),
      View("AllEventsDeploymentStep_Thumbnail", step)
        .Location("Thumbnail", "Content")
    );
  }

  public override IDisplayResult Edit(AllEventsDeploymentStep step)
  {
    return Initialize<AllEventsDeploymentStepViewModel>(
        "AllEventsDeploymentStep_Fields_Edit",
        model => { }
      )
      .Location("Content");
  }

  public override async Task<IDisplayResult> UpdateAsync(
    AllEventsDeploymentStep step,
    IUpdateModel updater
  )
  {
    await updater.TryUpdateModelAsync(step, Prefix);

    return Edit(step);
  }
}
