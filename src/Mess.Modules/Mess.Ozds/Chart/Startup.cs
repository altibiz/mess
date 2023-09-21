using Mess.Chart.Abstractions.Extensions.Microsoft;
using Mess.MeasurementDevice.Abstractions;
using Mess.MeasurementDevice.Chart.Providers;
using Mess.OrchardCore.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace Mess.MeasurementDevice.Chart;

[RequireFeatures("Mess.Chart")]
public class Startup : StartupBase
{
  public override void ConfigureServices(IServiceCollection services)
  {
    services.AddDataMigration<Migrations>();
    services.AddPermissionProvider<ChartPermissions>();

    services.AddChartProvider<AbbChartProvider>();
  }
}
