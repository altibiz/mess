using Mess.EventStore.Abstractions.Client;
using Mess.EventStore.Client;
using Mess.Test.Extensions.Microsoft;
using Mess.EventStore.Test.Abstractions;

namespace Mess.EventStore.Test;

public class Startup : Mess.OrchardCore.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.RegisterTenantFixture();
    services.RegisterTestEventStore();

    services.AddScoped<IEventStoreSession, EventStoreSession>();
    services.AddScoped<IEventStoreQuery, EventStoreQuery>();
    services.AddSingleton<IEventStoreClient, EventStoreClient>();
  }
}
