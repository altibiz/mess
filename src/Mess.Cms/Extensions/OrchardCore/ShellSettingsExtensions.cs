using OrchardCore.Environment.Shell;

namespace Mess.Cms.Extensions.OrchardCore;

public static class ShellSettingsExtensions
{
  public static string GetTenantName(this ShellSettings shellSettings)
  {
    return shellSettings.Name ?? ShellSettings.DefaultShellName;
  }

  public static string GetDatabaseConnectionString(
    this ShellSettings shellSettings
  )
  {
    return shellSettings.ShellConfiguration["ConnectionString"]
           ?? throw new InvalidOperationException(
             "ConnectionString not found in shell configuration"
           );
  }

  public static string GetDatabaseTablePrefix(
    this ShellSettings shellSettings
  )
  {
    return shellSettings.ShellConfiguration["TablePrefix"] ?? "";
  }

  public static string GetRequestUrlPrefix(this ShellSettings shellSettings)
  {
    return (shellSettings.RequestUrlPrefix ?? "").Trim('/');
  }
}
