using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Mess.Event.Abstractions.Extensions.Microsoft;

namespace Mess.Iot.Event;

[RequireFeatures("Mess.Event")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddProjectionDispatcher<PushProjectionApplicator>();
    services.AddProjectionDispatcher<UpdateProjectionApplicator>();
  }
}
