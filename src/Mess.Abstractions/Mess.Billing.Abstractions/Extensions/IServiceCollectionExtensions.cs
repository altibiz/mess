using Mess.Billing.Abstractions.Invoices;
using Mess.Billing.Abstractions.Receipts;

namespace Mess.Billing.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddBillingFactories<TInvoiceFactory, TReceiptFactory>(
    this IServiceCollection services
  )
    where TInvoiceFactory : class, IInvoiceFactory
    where TReceiptFactory : class, IReceiptFactory
  {
    services.AddScoped<IInvoiceFactory, TInvoiceFactory>();
    services.AddScoped<IReceiptFactory, TReceiptFactory>();
  }
}
