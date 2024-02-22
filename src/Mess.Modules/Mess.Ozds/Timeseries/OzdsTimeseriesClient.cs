using FlexLabs.EntityFrameworkCore.Upsert;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Prelude.Extensions.Objects;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using OrchardCore.Environment.Shell;
using Z.EntityFramework.Plus;

namespace Mess.Ozds.Timeseries;

public partial class OzdsTimeseriesClient : IOzdsTimeseriesClient
{
  private readonly IServiceProvider _services;

  private readonly ShellSettings _shellSettings;

  public OzdsTimeseriesClient(IServiceProvider services, ShellSettings shellSettings)
  {
    _services = services;
    _shellSettings = shellSettings;
  }

  public void AddAbbMeasurement(AbbMeasurement model)
  {
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.AbbMeasurements
        .Upsert(model.ToEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();
    });
  }

  public Task AddAbbMeasurementAsync(AbbMeasurement model)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.AbbMeasurements
          .Upsert(model.ToEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();
      });
  }

  public void AddSchneiderMeasurement(SchneiderMeasurement model)
  {
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.SchneiderMeasurements
        .Upsert(model.ToEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();
    });
  }

  public Task AddSchneiderMeasurementAsync(SchneiderMeasurement model)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.SchneiderMeasurements
          .Upsert(model.ToEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();
      });
  }

  public void AddBulkAbbMeasurement(IEnumerable<AbbMeasurement> measurements)
  {
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.AbbMeasurements
        .UpsertRange(measurements.Select(measurement => measurement.ToEntity(_shellSettings.GetDatabaseTablePrefix())).ToArray())
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();
    });
  }

  public Task AddBulkAbbMeasurementAsync(IEnumerable<AbbMeasurement> measurements)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.AbbMeasurements
          .UpsertRange(measurements.Select(measurement => measurement.ToEntity(_shellSettings.GetDatabaseTablePrefix())).ToArray())
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();
      });
  }

  public void AddBulkSchneiderMeasurement(IEnumerable<SchneiderMeasurement> measurements)
  {
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.SchneiderMeasurements
        .UpsertRange(measurements.Select(measurement => measurement.ToEntity(_shellSettings.GetDatabaseTablePrefix())).ToArray())
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();
    });
  }

  public Task AddBulkSchneiderMeasurementAsync(IEnumerable<SchneiderMeasurement> measurements)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.SchneiderMeasurements
          .UpsertRange(measurements.Select(measurement => measurement.ToEntity(_shellSettings.GetDatabaseTablePrefix())).ToArray())
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();
      });
  }
}
