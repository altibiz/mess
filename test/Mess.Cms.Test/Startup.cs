using Mess.Cms.Test.Extensions.Microsoft;

namespace Mess.Cms.Test;

public class Startup : Prelude.Test.Startup
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
