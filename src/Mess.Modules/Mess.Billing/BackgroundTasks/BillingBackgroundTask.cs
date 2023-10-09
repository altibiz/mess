using Mess.Billing.Abstractions;
using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Models;
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
    var now = DateTime.UtcNow;
    var nowLastMonth = now.AddMonths(-1);
    var nowLastMonthStart = new DateTime(
      nowLastMonth.Year,
      nowLastMonth.Month,
      1
    );
    var nowLastMonthEnd = nowLastMonthStart.AddMonths(1);

    var session = serviceProvider.GetRequiredService<ISession>();
    var logger = serviceProvider.GetRequiredService<
      ILogger<BillingBackgroundTask>
    >();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();

    var billingItems = await session
      .Query<ContentItem, BillingIndex>()
      .ListAsync();
    foreach (var billingItem in billingItems)
    {
      var billingPart = billingItem.As<BillingPart>()!;

      var billingFactory = serviceProvider
        .GetServices<IBillingFactory>()
        .Where(factory => factory.ContentType == billingItem.ContentType)
        .FirstOrDefault();
      if (billingFactory == null)
      {
        logger.LogError($"No receipt factory for {billingItem.ContentType}");
        continue;
      }

      var invoiceItem = await billingFactory.CreateInvoiceAsync(
        billingItem,
        nowLastMonthStart,
        nowLastMonthEnd
      );
      invoiceItem.Alter<InvoicePart>(invoicePart =>
      {
        invoicePart.BillingContentItemId = billingItem.ContentItemId;
        invoicePart.LegalEntityContentItemId =
          billingPart.LegalEntityContentItemId;
      });
      await contentManager.CreateAsync(invoiceItem);
    }
  }
}
