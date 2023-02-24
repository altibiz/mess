using OrchardCore.Deployment;

namespace Mess.EventStore.Deployment;

public class AllEventsDeploymentStep : DeploymentStep
{
  public AllEventsDeploymentStep()
  {
    Name = "AllEvents";
  }
}
