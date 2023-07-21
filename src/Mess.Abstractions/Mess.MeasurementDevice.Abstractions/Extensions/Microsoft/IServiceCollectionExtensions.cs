using Mess.MeasurementDevice.Abstractions.Pushing;
using Mess.MeasurementDevice.Abstractions.Polling;
using Mess.MeasurementDevice.Abstractions.Updating;
using Mess.MeasurementDevice.Abstractions.Security;

namespace Mess.MeasurementDevice.Abstractions.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddMeasurementDevicePushHandler<T>(
    this IServiceCollection services
  )
    where T : class, IMeasurementDevicePushHandler
  {
    services.AddScoped<IMeasurementDevicePushHandler, T>();
    return services;
  }

  public static IServiceCollection AddMeasurementDevicePollHandler<T>(
    this IServiceCollection services
  )
    where T : class, IMeasurementDevicePollHandler
  {
    services.AddScoped<IMeasurementDevicePollHandler, T>();
    return services;
  }

  public static IServiceCollection AddMeasurementDeviceUpdateHandler<T>(
    this IServiceCollection services
  )
    where T : class, IMeasurementDeviceUpdateHandler
  {
    services.AddScoped<IMeasurementDeviceUpdateHandler, T>();
    return services;
  }

  public static IServiceCollection AddMeasurementDeviceAuthorizationHandler<T>(
    this IServiceCollection services
  )
    where T : class, IMeasurementDeviceAuthorizationHandler
  {
    services.AddScoped<IMeasurementDeviceAuthorizationHandler, T>();
    return services;
  }
}
