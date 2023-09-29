using Mess.Billing.Abstractions.Invoices;
using Mess.Billing.Abstractions.Receipts;

namespace Mess.Billing.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddBillingFactories<TReceiptFactory, TInvoiceFactory>(
    this IServiceCollection services
  )
    where TReceiptFactory : class, IReceiptFactory
    where TInvoiceFactory : class, IInvoiceFactory
  {
    services.AddScoped<IInvoiceFactory, TInvoiceFactory>();
    services.AddScoped<IReceiptFactory, TReceiptFactory>();
  }
}
