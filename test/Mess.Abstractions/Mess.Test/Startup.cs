using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Mess.Test.Extensions.Microsoft;

namespace Mess.Test;

public class Startup
{
  public virtual void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    services.UseDemystifyExceptionFilter();
    services.AddSkippableFactSupport();

    services.AddTenantFixture();
    services.AddE2eFixture();
    services.AddSnapshotFixture();

    services.AddLogging(
      builder =>
        builder.AddXunitOutput(builder =>
        {
          builder.Filter = (_, level) =>
            level is >= LogLevel.Debug and < LogLevel.None;
        })
    );
  }
}
