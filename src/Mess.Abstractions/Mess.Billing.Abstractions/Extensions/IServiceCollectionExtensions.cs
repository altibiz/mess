using Mess.Billing.Abstractions.Services;

namespace Mess.Billing.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddBillingFactory<TBillingFactory>(
    this IServiceCollection services
  )
    where TBillingFactory : class, IBillingFactory
  {
    services.AddScoped<IBillingFactory, TBillingFactory>();
  }

  public static void AddPaymentIndexer<TPaymentIndexer>(
    this IServiceCollection services
  )
    where TPaymentIndexer : class, IPaymentIndexer
  {
    services.AddScoped<IPaymentIndexer, TPaymentIndexer>();
  }

  public static void AddBillingIndexer<TBillingIndexer>(
    this IServiceCollection services
  )
    where TBillingIndexer : class, IBillingIndexer
  {
    services.AddScoped<IBillingIndexer, TBillingIndexer>();
  }
}
