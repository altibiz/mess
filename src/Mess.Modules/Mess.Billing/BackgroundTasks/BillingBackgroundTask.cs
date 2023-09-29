using Mess.Billing.Abstractions.Indexes;
using Mess.Billing.Abstractions.Invoices;
using Mess.Billing.Abstractions.Models;
using Mess.OrchardCore;
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
    var session = serviceProvider.GetRequiredService<ISession>();
    var logger = serviceProvider.GetRequiredService<
      ILogger<BillingBackgroundTask>
    >();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();

    var billables = await session
      .Query<ContentItem, BillableIndex>()
      .ListAsync();
    foreach (var billable in billables)
    {
      var invoiceFactory = serviceProvider
        .GetServices<IInvoiceFactory>()
        .Where(factory => factory.ContentType == billable.ContentType)
        .FirstOrDefault();
      if (invoiceFactory == null)
      {
        logger.LogError($"No receipt factory for {billable.ContentType}");
        continue;
      }

      var invoice = await invoiceFactory.CreateAsync(billable);
      var invoiceItem = await contentManager.NewContentAsync<InvoiceItem>();
      invoiceItem.Alter(
        invoiceItem => invoiceItem.InvoicePart,
        invoicePart =>
        {
          invoicePart.Invoice = invoice;
        }
      );
      await contentManager.CreateAsync(invoiceItem);
    }
  }
}
