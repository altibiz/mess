using Mess.Billing.Abstractions.Models;
using Mess.Billing.Abstractions.Services;
using Mess.Prelude.Extensions.Timestamps;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Extensions;

public static class IServiceProviderExtensions
{
  public static async Task<string?> CreateInvoicesUntilAsync(
    this IServiceProvider services,
    ILogger logger,
    ContentItem item,
    DateTimeOffset now,
    DateTimeOffset until
  )
  {
    var contentManager = services.GetRequiredService<IContentManager>();

    var billingFactory = services
      .GetServices<IBillingFactory>()
      .FirstOrDefault(factory => factory.IsApplicable(item));
    if (billingFactory is null)
    {
      logger.LogError("No billing factory for {}", item.ContentType);
      return default;
    }

    var billingPart = item.As<BillingPart>();
    if (billingPart is null)
    {
      logger.LogError("No billing part for {}", item.ContentType);
      return default;
    }

    var currentStart =
      billingPart.LastInvoiceTo is { } lastInvoiceEnd
        && lastInvoiceEnd != default
        ? lastInvoiceEnd
        : item.CreatedUtc is { } created && created != default ?
          new DateTimeOffset(created)
          : now.AddMonths(-1).GetStartOfMonth();

    string? lastInvoiceContentItemId = default;
    while (currentStart < until)
    {
      var nextMonthStart = currentStart.AddMonths(1).GetStartOfMonth();
      var currentEnd = until < nextMonthStart ? until : nextMonthStart;

      try
      {
        var invoiceItem = await billingFactory.CreateInvoiceAsync(
          item,
          currentStart,
          currentEnd
        );

        invoiceItem.Alter<InvoicePart>(invoicePart =>
        {
          invoicePart.Receipt = new ContentPickerField();
          invoicePart.Date = new DateField
          {
            Value = now.UtcDateTime
          };
        });
        await contentManager.CreateAsync(invoiceItem);
        lastInvoiceContentItemId = invoiceItem.ContentItemId;

        item.Alter<BillingPart>(part =>
        {
          part.LastInvoiceCreated = now;
          part.LastInvoiceFrom = currentStart;
          part.LastInvoiceTo = currentEnd;
        });
        await contentManager.UpdateAsync(item);
      }
      catch (Exception)
      {
        logger.LogError("Failed creating invoice for {}", item.ContentItemId);
        return lastInvoiceContentItemId;
      }

      logger.LogInformation(
        "Created invoice for {} from {} to {}",
        item.ContentItemId,
        currentStart,
        currentEnd
      );

      currentStart = currentEnd;
    }



    return lastInvoiceContentItemId;
  }
}
