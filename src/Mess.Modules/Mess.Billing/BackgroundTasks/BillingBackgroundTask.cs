using Mess.Billing.Abstractions.Services;
using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
using Mess.System.Extensions.Timestamps;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Billing.BackgroundTasks;

[BackgroundTask(Schedule = "0 * * */1 *")]
public class BillingBackgroundTask : IBackgroundTask
{
  public async Task DoWorkAsync(
    IServiceProvider serviceProvider,
    CancellationToken cancellationToken
  )
  {
    (DateTimeOffset nowLastMonthStart, DateTimeOffset nowLastMonthEnd) =
      DateTimeOffset.UtcNow.AddMonths(-1).GetMonthRange();

    var session = serviceProvider.GetRequiredService<ISession>();
    var billingItems = await session
      .Query<ContentItem, BillingIndex>()
      .ListAsync();

    var logger = serviceProvider.GetRequiredService<
      ILogger<BillingBackgroundTask>
    >();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    foreach (var billingItem in billingItems)
    {
      var billingPart = billingItem.As<BillingPart>()!;

      var billingFactory = serviceProvider
        .GetServices<IBillingFactory>()
        .Where(factory => factory.IsApplicable(billingItem))
        .FirstOrDefault();
      if (billingFactory == null)
      {
        logger.LogError($"No receipt factory for {billingItem.ContentType}");
        continue;
      }

      try
      {
        var invoiceItem = await billingFactory.CreateInvoiceAsync(
          billingItem,
          nowLastMonthStart,
          nowLastMonthEnd
        );
        await contentManager.CreateAsync(invoiceItem);
      }
      catch (Exception exception)
      {
        logger.LogError(
          $"Failed to create invoice for item '{billingItem.ContentItemId}' of type '{billingItem.ContentType}'",
          exception
        );
      }
    }

    await session.SaveChangesAsync();
  }
}
