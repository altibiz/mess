namespace Mess.Eor.Test;

public class Startup : Mess.OrchardCore.Test.Startup
{
  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);
  }
}
