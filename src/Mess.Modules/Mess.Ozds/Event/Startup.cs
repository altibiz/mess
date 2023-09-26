using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Mess.Event.Abstractions.Extensions.Microsoft;

namespace Mess.Ozds.Event;

[RequireFeatures("Mess.Event")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddProjectionDispatcher<PidgeonPushProjectionApplicator>();
    services.AddProjectionDispatcher<PidgeonUpdateProjectionApplicator>();
  }
}
