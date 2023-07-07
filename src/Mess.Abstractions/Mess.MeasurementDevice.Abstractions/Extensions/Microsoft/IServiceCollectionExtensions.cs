using Mess.MeasurementDevice.Abstractions.Pushing;
using Mess.MeasurementDevice.Abstractions.Polling;
using Mess.MeasurementDevice.Abstractions.Updating;

namespace Mess.MeasurementDevice.Abstractions.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddPushHandler<T>(
    this IServiceCollection services
  )
    where T : class, IPushHandler
  {
    services.AddScoped<IPushHandler, T>();
    return services;
  }

  public static IServiceCollection AddPollHandler<T>(
    this IServiceCollection services
  )
    where T : class, IPollHandler
  {
    services.AddScoped<IPollHandler, T>();
    return services;
  }

  public static IServiceCollection AddUpdateHandler<T>(
    this IServiceCollection services
  )
    where T : class, IUpdateHandler
  {
    services.AddScoped<IUpdateHandler, T>();
    return services;
  }
}
