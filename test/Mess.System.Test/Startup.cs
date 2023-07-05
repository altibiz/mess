using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Mess.System.Test.Extensions.Microsoft;
using Mess.System.Test.Migrations;

namespace Mess.System.Test;

public class Startup
{
  public virtual void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    services.UseDemystifyExceptionFilter();
    services.AddSkippableFactSupport();

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

  public virtual void ConfigureServices(IServiceProvider serviceProvider)
  {
    using var scope = serviceProvider.CreateScope();
    var migrators = scope.ServiceProvider.GetServices<ITestMigrator>();
    foreach (var migrator in migrators)
    {
      migrator.MigrateAsync().Wait();
    }
  }
}
