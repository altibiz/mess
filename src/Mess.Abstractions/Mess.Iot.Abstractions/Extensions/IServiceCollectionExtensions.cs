using Mess.Iot.Abstractions.Services;

namespace Mess.Iot.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddIotPushHandler<T>(
    this IServiceCollection services
  )
    where T : class, IIotPushHandler
  {
    services.AddScoped<IIotPushHandler, T>();
    return services;
  }

  public static IServiceCollection AddIotPollHandler<T>(
    this IServiceCollection services
  )
    where T : class, IIotPollHandler
  {
    services.AddScoped<IIotPollHandler, T>();
    return services;
  }

  public static IServiceCollection AddIotUpdateHandler<T>(
    this IServiceCollection services
  )
    where T : class, IIotUpdateHandler
  {
    services.AddScoped<IIotUpdateHandler, T>();
    return services;
  }

  public static IServiceCollection AddIotAuthorizationHandler<T>(
    this IServiceCollection services
  )
    where T : class, IIotAuthorizationHandler
  {
    services.AddScoped<IIotAuthorizationHandler, T>();
    return services;
  }
}
