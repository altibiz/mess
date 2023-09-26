using Mess.Relational.Test.Abstractions.Extensions;

namespace Mess.Relational.Test;

public class Startup : OrchardCore.Test.Startup
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
