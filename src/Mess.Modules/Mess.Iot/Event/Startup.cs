using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Mess.Event.Abstractions.Extensions;

namespace Mess.Iot.Event;

[RequireFeatures("Mess.Event")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddEventDispatcher<PushEventDispatcher>();
    services.AddEventDispatcher<UpdateEventDispatcher>();
  }
}
