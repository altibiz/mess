using Microsoft.Extensions.Options;
using OrchardCore.BackgroundTasks;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Security.Permissions;
using YesSql.Indexes;

namespace Mess.OrchardCore.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddIndexProvider<TIndexProvider>(
    this IServiceCollection services
  )
    where TIndexProvider : class, IIndexProvider
  {
    services.AddSingleton<IIndexProvider, TIndexProvider>();
    return services;
  }

  public static IServiceCollection AddPermissionProvider<TPermissionProvider>(
    this IServiceCollection services
  )
    where TPermissionProvider : class, IPermissionProvider
  {
    services.AddScoped<IPermissionProvider, TPermissionProvider>();
    return services;
  }

  public static IServiceCollection AddResources<TResources>(
    this IServiceCollection services
  )
    where TResources : class, IConfigureOptions<ResourceManagementOptions>
  {
    services.AddTransient<
      IConfigureOptions<ResourceManagementOptions>,
      TResources
    >();
    return services;
  }

  public static IServiceCollection AddModularTenantEvents<TModularTenantEvents>(
    this IServiceCollection services
  )
    where TModularTenantEvents : class, IModularTenantEvents
  {
    services.AddSingleton<IModularTenantEvents, TModularTenantEvents>();
    return services;
  }

  public static IServiceCollection AddHostedServiceModularTenantEvents<THostedService>(
    this IServiceCollection services
  )
    where THostedService : class, IHostedService
  {
    services.AddModularTenantEvents<
      HostedServiceModularTenantEvents<THostedService>
    >();
    return services;
  }

  public static IServiceCollection AddBackgroundTask<TBackgroundTask>(
    this IServiceCollection services
  )
    where TBackgroundTask : class, IBackgroundTask
  {
    services.AddSingleton<IBackgroundTask, TBackgroundTask>();
    return services;
  }
}
