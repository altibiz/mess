using Mess.Billing.Abstractions.Extensions;
using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Services;
using Mess.Prelude.Extensions.Timestamps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Billing.BackgroundTasks;

[BackgroundTask(Schedule = "1 * * */1 *")] // NOTE: second minute of every month
public class BillingBackgroundTask : IBackgroundTask
{
  public async Task DoWorkAsync(
    IServiceProvider serviceProvider,
    CancellationToken cancellationToken
  )
  {
    var now = DateTimeOffset.UtcNow;
    var startOfThisMonth = now.GetStartOfMonth();

    var session = serviceProvider.GetRequiredService<ISession>();
    var billingItems = await session
      .Query<ContentItem, BillingIndex>()
      .ListAsync();

    var logger = serviceProvider.GetRequiredService<
      ILogger<BillingBackgroundTask>
    >();
    foreach (var billingItem in billingItems)
    {
      var _ = await serviceProvider.CreateInvoicesUntilAsync(
        logger,
        billingItem,
        now,
        startOfThisMonth
      );
    }

    await session.SaveChangesAsync();
  }
}
