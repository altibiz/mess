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

      context.AbbQuarterHourlyEnergyRange
        .Upsert(model.ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbQuarterHourlyEnergyRangeEntityExtensions.Upsert)
        .Run();

      context.AbbDailyEnergyRange
        .Upsert(model.ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbDailyEnergyRangeEntityExtensions.Upsert)
        .Run();

      context.AbbMonthlyEnergyRange
        .Upsert(model.ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbMonthlyEnergyRangeEntityExtensions.Upsert)
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

        await context.AbbQuarterHourlyEnergyRange
          .Upsert(model.ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(AbbQuarterHourlyEnergyRangeEntityExtensions.Upsert)
          .RunAsync();

        await context.AbbDailyEnergyRange
          .Upsert(model.ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(AbbDailyEnergyRangeEntityExtensions.Upsert)
          .RunAsync();

        await context.AbbMonthlyEnergyRange
          .Upsert(model.ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(AbbMonthlyEnergyRangeEntityExtensions.Upsert)
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

      context.SchneiderQuarterHourlyEnergyRange
        .Upsert(model.ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderQuarterHourlyEnergyRangeEntityExtensions.Upsert)
        .Run();

      context.SchneiderDailyEnergyRange
        .Upsert(model.ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderDailyEnergyRangeEntityExtensions.Upsert)
        .Run();

      context.SchneiderMonthlyEnergyRange
        .Upsert(model.ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderMonthlyEnergyRangeEntityExtensions.Upsert)
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

        await context.SchneiderQuarterHourlyEnergyRange
          .Upsert(model.ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(SchneiderQuarterHourlyEnergyRangeEntityExtensions.Upsert)
          .RunAsync();

        await context.SchneiderDailyEnergyRange
          .Upsert(model.ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(SchneiderDailyEnergyRangeEntityExtensions.Upsert)
          .RunAsync();

        await context.SchneiderMonthlyEnergyRange
          .Upsert(model.ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(SchneiderMonthlyEnergyRangeEntityExtensions.Upsert)
          .RunAsync();
      });
  }

  public void AddBulkAbbMeasurement(IEnumerable<AbbMeasurement> measurements)
  {
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.AbbMeasurements
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();

      context.AbbQuarterHourlyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbQuarterHourlyEnergyRangeEntityExtensions.Upsert)
        .Run();

      context.AbbDailyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbDailyEnergyRangeEntityExtensions.Upsert)
        .Run();

      context.AbbMonthlyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbMonthlyEnergyRangeEntityExtensions.Upsert)
        .Run();
    });
  }

  public Task AddBulkAbbMeasurementAsync(IEnumerable<AbbMeasurement> measurements)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.AbbMeasurements
          .UpsertRange(measurements
            .Select(measurement => measurement
              .ToEntity(_shellSettings.GetDatabaseTablePrefix())))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();

        await context.AbbQuarterHourlyEnergyRange
                .UpsertRange(measurements
                  .Select(measurement => measurement
                    .ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(AbbQuarterHourlyEnergyRangeEntityExtensions.Upsert)
                .RunAsync();

        await context.AbbDailyEnergyRange
                .UpsertRange(measurements
                  .Select(measurement => measurement
                    .ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(AbbDailyEnergyRangeEntityExtensions.Upsert)
                .RunAsync();

        await context.AbbMonthlyEnergyRange
                .UpsertRange(measurements
                  .Select(measurement => measurement
                    .ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(AbbMonthlyEnergyRangeEntityExtensions.Upsert)
                .RunAsync();
      });
  }

  public void AddBulkSchneiderMeasurement(IEnumerable<SchneiderMeasurement> measurements)
  {
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.SchneiderMeasurements
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();

      context.SchneiderQuarterHourlyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderQuarterHourlyEnergyRangeEntityExtensions.Upsert)
        .Run();

      context.SchneiderDailyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderDailyEnergyRangeEntityExtensions.Upsert)
        .Run();

      context.SchneiderMonthlyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderMonthlyEnergyRangeEntityExtensions.Upsert)
        .Run();
    });
  }

  public Task AddBulkSchneiderMeasurementAsync(IEnumerable<SchneiderMeasurement> measurements)
  {
    return _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.SchneiderMeasurements
          .UpsertRange(measurements
            .Select(measurement => measurement
              .ToEntity(_shellSettings.GetDatabaseTablePrefix())))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();

        await context.SchneiderQuarterHourlyEnergyRange
                .UpsertRange(measurements
                  .Select(measurement => measurement
                    .ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(SchneiderQuarterHourlyEnergyRangeEntityExtensions.Upsert)
                .RunAsync();

        await context.SchneiderDailyEnergyRange
                .UpsertRange(measurements
                  .Select(measurement => measurement
                    .ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(SchneiderDailyEnergyRangeEntityExtensions.Upsert)
                .RunAsync();

        await context.SchneiderMonthlyEnergyRange
                .UpsertRange(measurements
                  .Select(measurement => measurement
                    .ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix())))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(SchneiderMonthlyEnergyRangeEntityExtensions.Upsert)
                .RunAsync();
      });
  }
}
