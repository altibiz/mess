using Mess.Ozds.Abstractions.Services;

namespace Mess.Ozds.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static void AddOzdsIotDeviceBillingFactory<TOzdsIotDeviceBillingFactory>(
    this IServiceCollection services
  )
    where TOzdsIotDeviceBillingFactory : class, IOzdsIotDeviceBillingFactory
  {
    services.AddScoped<IOzdsIotDeviceBillingFactory, TOzdsIotDeviceBillingFactory>();
  }
}
