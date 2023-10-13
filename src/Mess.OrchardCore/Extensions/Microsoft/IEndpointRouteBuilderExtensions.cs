using Mess.OrchardCore.Extensions.OrchardCore;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.Environment.Shell;
using OrchardCore.Mvc.Core.Utilities;

namespace Mess.OrchardCore.Extensions.Microsoft;

public static class IEndpointRouteBuilderExtensions
{
  public static IEndpointRouteBuilder Redirect(
    this IEndpointRouteBuilder endpoints,
    string from,
    string to
  )
  {
    return Redirect(endpoints, new Redirective(from, to));
  }

  public static IEndpointRouteBuilder RedirectPermanent(
    this IEndpointRouteBuilder endpoints,
    string from,
    string to
  )
  {
    return Redirect(endpoints, new Redirective(from, to, Permanent: true));
  }

  public static IEndpointRouteBuilder RedirectAdmin(
    this IEndpointRouteBuilder endpoints,
    string from,
    string to
  )
  {
    return Redirect(endpoints, new Redirective(from, to, Admin: true));
  }

  public static IEndpointRouteBuilder RedirectAdminPermanent(
    this IEndpointRouteBuilder endpoints,
    string from,
    string to
  )
  {
    return Redirect(
      endpoints,
      new Redirective(from, to, Admin: true, Permanent: true)
    );
  }

  public static IEndpointRouteBuilder Redirect(
    this IEndpointRouteBuilder endpoints,
    params Redirective[] paths
  )
  {
    var requestUrlPrefix = endpoints.ServiceProvider
      .GetRequiredService<ShellSettings>()
      .GetRequestUrlPrefix();
    var adminUrlPrefix = endpoints.ServiceProvider
      .GetRequiredService<IOptions<AdminOptions>>()
      .Value.AdminUrlPrefix.Trim('/');

    foreach (var (from, to, admin, permanent) in paths)
    {
      var normalizedFrom = from.Trim('/');
      var normalizedTo = to.Trim('/');

      if (admin)
      {
        normalizedFrom = $"{adminUrlPrefix}/{normalizedFrom}";
        normalizedTo = $"{adminUrlPrefix}/{normalizedTo}";
      }

      endpoints.MapGet(
        normalizedFrom,
        async http =>
        {
          http.Response.Redirect(
            $"/{requestUrlPrefix}/{normalizedTo}",
            permanent
          );
        }
      );
    }

    return endpoints;
  }

  public static void MapMessControllerRoute<T>(
    this IEndpointRouteBuilder endpoints,
    string action,
    string pattern
  )
  {
    var type = typeof(T);
    var @namespace =
      type.Namespace
      ?? throw new NullReferenceException(
        $"{typeof(T).Name} is not in a namespace is null"
      );
    var controller = type.ControllerName();

    endpoints.MapAreaControllerRoute(
      name: $"{@namespace}.{controller}.{action}",
      pattern: pattern,
      areaName: @namespace,
      defaults: new { controller, action }
    );
  }

  public static void MapControllerNamespaceRoutes(
    this IEndpointRouteBuilder endpoints
  ) { }
}

public record Redirective(
  string From,
  string To,
  bool Permanent = false,
  bool Admin = false
);
