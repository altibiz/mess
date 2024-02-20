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

[BackgroundTask(Schedule = "0 * * */1 *")] // NOTE: first minute of every month
public class BillingBackgroundTask : IBackgroundTask
{
  public async Task DoWorkAsync(
    IServiceProvider serviceProvider,
    CancellationToken cancellationToken
  )
  {
    var (nowLastMonthStart, nowLastMonthEnd) =
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
        .FirstOrDefault(factory => factory.IsApplicable(billingItem));
      if (billingFactory == null)
      {
        logger.LogError("No receipt factory for {}", billingItem.ContentType);
        continue;
      }

      try
      {
        var invoiceItem = await billingFactory.CreateInvoiceAsync(
          billingItem,
          nowLastMonthStart,
          nowLastMonthEnd
        );
        invoiceItem.Alter<InvoicePart>(invoicePart =>
        {
          invoicePart.Receipt = new ContentPickerField();
          invoicePart.Date = new DateField
          {
            Value = DateTime.UtcNow
          };
        });
        await contentManager.CreateAsync(invoiceItem);
      }
      catch (Exception exception)
      {
        logger.LogError(
          "Failed to create invoice for item '{}' of type '{}': {}",
          billingItem.ContentItemId,
          billingItem.ContentType,
          exception
        );
      }
    }

    await session.SaveChangesAsync();
  }
}
