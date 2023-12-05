using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using OrchardCore.BackgroundTasks;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.ResourceManagement;
using OrchardCore.Security.Permissions;
using YesSql.Indexes;

namespace Mess.Cms.Extensions.Microsoft;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddIndexProvider<TIndexProvider>(
    this IServiceCollection services
  )
    where TIndexProvider : class, IIndexProvider
  {
    if (typeof(IScopedIndexProvider).IsAssignableFrom(typeof(TIndexProvider)))
      services.AddScoped(typeof(IScopedIndexProvider), typeof(TIndexProvider));
    else
      services.AddSingleton(typeof(IIndexProvider), typeof(TIndexProvider));
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
    services.AddScoped<IModularTenantEvents, TModularTenantEvents>();
    return services;
  }

  public static IServiceCollection AddHostedServiceModularTenantEvents<
    THostedService>(
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

  public static IServiceCollection AddNavigationProvider<TAdminMenu>(
    this IServiceCollection services
  )
    where TAdminMenu : class, INavigationProvider
  {
    services.AddScoped<INavigationProvider, TAdminMenu>();
    return services;
  }

  public static IServiceCollection AddContentTypePartDefinitionDisplayDriver<
    TContentTypePartDefinitionDisplayDriver>(
    this IServiceCollection services
  )
    where TContentTypePartDefinitionDisplayDriver : class,
    IContentTypePartDefinitionDisplayDriver
  {
    services.AddScoped<
      IContentTypePartDefinitionDisplayDriver,
      TContentTypePartDefinitionDisplayDriver
    >();
    return services;
  }

  public static IServiceCollection AddContentPartFieldDefinitionDisplayDriver<
    TContentPartFieldDefinitionDisplayDriver>(
    this IServiceCollection services
  )
    where TContentPartFieldDefinitionDisplayDriver : class,
    IContentPartFieldDefinitionDisplayDriver
  {
    services.AddScoped<
      IContentPartFieldDefinitionDisplayDriver,
      TContentPartFieldDefinitionDisplayDriver
    >();
    return services;
  }

  public static IServiceCollection AddContentHandler<TContentHandler>(
    this IServiceCollection services
  )
    where TContentHandler : class, IContentHandler
  {
    services.AddScoped<IContentHandler, TContentHandler>();
    return services;
  }

  public static IServiceCollection AddAuthorizationHandler<
    TAuthorizationHandler>(
    this IServiceCollection services
  )
    where TAuthorizationHandler : class, IAuthorizationHandler
  {
    services.AddScoped<IAuthorizationHandler, TAuthorizationHandler>();
    return services;
  }
}
