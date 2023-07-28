using OrchardCore.DisplayManagement.Razor;
using OrchardCore.Environment.Shell;

namespace Mess.OrchardCore.Extensions.OrchardCore;

public static class RazorPageExtensions
{
  public static string GetTenantName<TModel>(this RazorPage<TModel> razorPage)
  {
    var tenantName = razorPage.ViewContext.HttpContext.RequestServices
      .GetRequiredService<ShellSettings>()
      .GetTenantName();

    return tenantName;
  }

  public static string GetRequestUrlPrefix<TModel>(
    this RazorPage<TModel> razorPage
  )
  {
    var requestUrlPrefix = razorPage.ViewContext.HttpContext.RequestServices
      .GetRequiredService<ShellSettings>()
      .GetRequestUrlPrefix();

    return $"/{requestUrlPrefix}";
  }

  public static string GetAdminRequestUrlPrefix<TModel>(
    this RazorPage<TModel> razorPage
  )
  {
    var requestUrlPrefix = GetRequestUrlPrefix(razorPage);

    return $"/Admin{requestUrlPrefix}";
  }
}
