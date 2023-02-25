using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;
using Mess.Xunit.Extensions.Microsoft;

namespace Mess.Xunit;

public class Startup
{
  public virtual IHostBuilder CreateHostBuilder()
  {
    return Host.CreateDefaultBuilder();
  }

  public virtual void ConfigureHost(IHostBuilder hostBuilder)
  {
    hostBuilder.ConfigureLogging(builder => builder.ClearProviders());
  }

  public virtual void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    services.RegisterTestTenants();
    services.UseDemystifyExceptionFilter();
    services.AddSkippableFactSupport();
  }

  public virtual void Configure(IServiceProvider services)
  {
    // NOTE: this one doesn't respect IConfiguration
    services
      .GetRequiredService<ILoggerFactory>()
      .AddProvider(
        new XunitTestOutputLoggerProvider(
          services.GetRequiredService<ITestOutputHelperAccessor>(),
          (_, level) => level is >= LogLevel.Debug and < LogLevel.None
        )
      );
  }
}
