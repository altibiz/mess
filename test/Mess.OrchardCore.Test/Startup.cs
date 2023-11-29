using Mess.OrchardCore.Test.Extensions.Microsoft;

namespace Mess.OrchardCore.Test;

public class Startup : System.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.AddOrchardCoreShellSettings();
    services.AddOrchardSnapshotFixture();
  }
}
