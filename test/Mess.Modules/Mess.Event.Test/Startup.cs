using Mess.Event.Abstractions.Client;
using Mess.Event.Abstractions.Session;
using Mess.Event.Client;
using Mess.Event.Session;
using Mess.Event.Test.Abstractions.Extensions;

namespace Mess.Event.Test;

public class Startup : Cms.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.AddTestEventStore();

    services.AddScoped<IEventStoreSession, EventStoreSession>();
    services.AddScoped<IEventStoreQuery, EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();
  }
}
