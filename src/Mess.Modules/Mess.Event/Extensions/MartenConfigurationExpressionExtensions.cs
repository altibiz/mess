using Marten.Events.Daemon;
using Marten.Events.Daemon.Resiliency;
using Mess.Cms.Extensions.Microsoft;
using Mess.Event.BackgroundTasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Marten.MartenServiceCollectionExtensions;

namespace Mess.Event.Extensions;

// TODO: fix problems with attaining lock and stuff/projection applicators not
// being called

public static class MartenConfigurationExpressionExtensions
{
  public static MartenConfigurationExpression
    AddOrchardCoreAsyncProjectionDaemon(
      this MartenConfigurationExpression expression
    )
  {
    expression.AddAsyncDaemon(DaemonMode.HotCold);
    expression.Services.AddSingleton(
      services =>
        services
          .GetServices<IHostedService>()
          .OfType<AsyncProjectionHostedService>()
          .Single()
    );
    expression.Services
      .AddHostedServiceModularTenantEvents<AsyncProjectionHostedService>();
    expression.Services
      .AddBackgroundTask<EventStoreDiagnosticsBackgroundTask>();

    return expression;
  }
}
