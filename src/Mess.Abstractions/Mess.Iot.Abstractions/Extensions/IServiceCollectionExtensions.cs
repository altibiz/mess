using Mess.Iot.Abstractions.Pushing;
using Mess.Iot.Abstractions.Polling;
using Mess.Iot.Abstractions.Updating;
using Mess.Iot.Abstractions.Security;

namespace Mess.Iot.Abstractions.Extensions;

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
