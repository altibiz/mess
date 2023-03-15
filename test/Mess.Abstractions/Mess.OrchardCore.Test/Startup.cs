using Mess.OrchardCore.Test.Extensions.Microsoft;

namespace Mess.OrchardCore.Test;

public class Startup : Mess.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.RegisterOrchardSnapshotFixture();
    services.RegisterOrchardE2eFixture();
  }
}
