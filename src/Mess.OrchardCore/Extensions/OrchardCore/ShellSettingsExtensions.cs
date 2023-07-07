using OrchardCore.Environment.Shell;

namespace Mess.OrchardCore.Extensions.OrchardCore;

public static class ShellSettingsExtensions
{
  public static string GetTenantName(this ShellSettings shellSettings) =>
    shellSettings.Name ?? ShellHelper.DefaultShellName;

  public static string GetDatabaseConnectionString(
    this ShellSettings shellSettings
  ) =>
    shellSettings.ShellConfiguration["ConnectionString"]
    ?? throw new InvalidOperationException(
      "ConnectionString not found in shell configuration"
    );

  public static string GetDatabaseTablePrefix(
    this ShellSettings shellSettings
  ) => shellSettings.ShellConfiguration["TablePrefix"] ?? "";

  public static string GetRequestUrlPrefix(this ShellSettings shellSettings) =>
    shellSettings.RequestUrlPrefix ?? "";
}
