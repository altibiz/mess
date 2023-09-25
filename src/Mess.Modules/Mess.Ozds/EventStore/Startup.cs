using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Mess.EventStore.Abstractions.Extensions.Microsoft;

namespace Mess.Ozds.EventStore;

[RequireFeatures("Mess.EventStore")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddProjectionDispatcher<PidgeonPushProjectionApplicator>();
    services.AddProjectionDispatcher<PidgeonUpdateProjectionApplicator>();
  }
}
