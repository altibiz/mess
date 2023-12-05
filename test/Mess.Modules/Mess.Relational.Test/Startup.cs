using Mess.Relational.Test.Abstractions.Extensions;

namespace Mess.Relational.Test;

public class Startup : Cms.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    services.AddTestRelationalStore();
  }
}
