using Marten.Events.Daemon;
using Marten.Events.Daemon.Resiliency;
using Mess.EventStore.BackgroundTasks;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Marten.MartenServiceCollectionExtensions;

namespace Mess.EventStore.Extensions.Marten;

// TODO: fix problems with attaining lock and stuff/projection applicators not
// being called

public static class MartenConfigurationExpressionExtensions
{
  public static MartenConfigurationExpression AddOrchardCoreAsyncProjectionDaemon(
    this MartenConfigurationExpression expression
  )
  {
    expression.AddAsyncDaemon(DaemonMode.HotCold);
    expression.Services.AddSingleton<AsyncProjectionHostedService>(
      services =>
        services
          .GetServices<IHostedService>()
          .OfType<AsyncProjectionHostedService>()
          .Single()
    );
    expression.Services.AddHostedServiceModularTenantEvents<AsyncProjectionHostedService>();
    expression.Services.AddBackgroundTask<EventStoreDiagnosticsBackgroundTask>();

    return expression;
  }
}
