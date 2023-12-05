using Mess.Event.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Mess.Ozds.Event;

[RequireFeatures("Mess.Event")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddEventDispatcher<PidgeonPushEventDispatcher>();
    services.AddEventDispatcher<PidgeonUpdateEventDispatcher>();
  }
}
