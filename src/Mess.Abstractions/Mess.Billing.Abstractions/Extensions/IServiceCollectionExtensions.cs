using Mess.Billing.Abstractions.Factory;

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
}
