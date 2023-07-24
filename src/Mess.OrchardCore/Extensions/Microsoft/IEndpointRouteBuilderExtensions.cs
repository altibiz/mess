using Mess.OrchardCore.Extensions.OrchardCore;
using OrchardCore.Environment.Shell;

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
    return Redirect(endpoints, new Redirective(from, to, true));
  }

  public static IEndpointRouteBuilder Redirect(
    this IEndpointRouteBuilder endpoints,
    params Redirective[] paths
  )
  {
    var requestUrlPrefix = endpoints.ServiceProvider
      .GetRequiredService<ShellSettings>()
      .GetRequestUrlPrefix();

    foreach (var (from, to, permanent) in paths)
    {
      var normalizedFrom = from.Trim('/');
      var normalizedTo = to.Trim('/');
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
}

public record Redirective(string From, string To, bool Permanent = false);
