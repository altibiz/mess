using OrchardCore.Deployment;

namespace Mess.Event.Deployment;

public class AllEventsDeploymentStep : DeploymentStep
{
  public AllEventsDeploymentStep()
  {
    Name = "AllEvents";
  }
}
