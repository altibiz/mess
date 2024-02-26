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
        .WhenMatched(AbbQuarterHourlyEnergyRangeEntityExtensions.UpsertRow)
        .Run();

      context.AbbDailyEnergyRange
        .Upsert(model.ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbDailyEnergyRangeEntityExtensions.UpsertRow)
        .Run();

      context.AbbMonthlyEnergyRange
        .Upsert(model.ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbMonthlyEnergyRangeEntityExtensions.UpsertRow)
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
          .WhenMatched(AbbQuarterHourlyEnergyRangeEntityExtensions.UpsertRow)
          .RunAsync();

        await context.AbbDailyEnergyRange
          .Upsert(model.ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(AbbDailyEnergyRangeEntityExtensions.UpsertRow)
          .RunAsync();

        await context.AbbMonthlyEnergyRange
          .Upsert(model.ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(AbbMonthlyEnergyRangeEntityExtensions.UpsertRow)
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
        .WhenMatched(SchneiderQuarterHourlyEnergyRangeEntityExtensions.UpsertRow)
        .Run();

      context.SchneiderDailyEnergyRange
        .Upsert(model.ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderDailyEnergyRangeEntityExtensions.UpsertRow)
        .Run();

      context.SchneiderMonthlyEnergyRange
        .Upsert(model.ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderMonthlyEnergyRangeEntityExtensions.UpsertRow)
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
          .WhenMatched(SchneiderQuarterHourlyEnergyRangeEntityExtensions.UpsertRow)
          .RunAsync();

        await context.SchneiderDailyEnergyRange
          .Upsert(model.ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(SchneiderDailyEnergyRangeEntityExtensions.UpsertRow)
          .RunAsync();

        await context.SchneiderMonthlyEnergyRange
          .Upsert(model.ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(SchneiderMonthlyEnergyRangeEntityExtensions.UpsertRow)
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
            .ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbQuarterHourlyEnergyRangeEntity>(),
            (list, next) =>
              list
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbQuarterHourlyEnergyRangeEntityExtensions.UpsertRow)
        .Run();

      context.AbbDailyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbDailyEnergyRangeEntity>(),
            (list, next) =>
              list
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbDailyEnergyRangeEntityExtensions.UpsertRow)
        .Run();

      context.AbbMonthlyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbMonthlyEnergyRangeEntity>(),
            (list, next) =>
              list
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbMonthlyEnergyRangeEntityExtensions.UpsertRow)
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
        .UpsertRange(
        measurements
                  .Select(measurement => measurement
                    .ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
                  .OrderBy(range => range.Timestamp)
                  .Aggregate(
                    new List<AbbQuarterHourlyEnergyRangeEntity>(),
                    (list, next) =>
                      list
                        .Take(list.Count == 0 ? 0 : list.Count - 1)
                        .Concat(
                        list.LastOrDefault() is { } last
                          ? last.Upsert(next)
                          : new() { next }
                        )
                        .ToList())
)
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(AbbQuarterHourlyEnergyRangeEntityExtensions.UpsertRow)
                .RunAsync();

        await context.AbbDailyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbDailyEnergyRangeEntity>(),
            (list, next) =>
              list
                        .Take(list.Count == 0 ? 0 : list.Count - 1)
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(AbbDailyEnergyRangeEntityExtensions.UpsertRow)
                .RunAsync();

        await context.AbbMonthlyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbMonthlyEnergyRangeEntity>(),
            (list, next) =>
              list
                        .Take(list.Count == 0 ? 0 : list.Count - 1)
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(AbbMonthlyEnergyRangeEntityExtensions.UpsertRow)
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
            .ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderQuarterHourlyEnergyRangeEntity>(),
            (list, next) =>
              list
                        .Take(list.Count == 0 ? 0 : list.Count - 1)
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderQuarterHourlyEnergyRangeEntityExtensions.UpsertRow)
        .Run();

      context.SchneiderDailyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderDailyEnergyRangeEntity>(),
            (list, next) =>
              list
                        .Take(list.Count == 0 ? 0 : list.Count - 1)
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderDailyEnergyRangeEntityExtensions.UpsertRow)
        .Run();

      context.SchneiderMonthlyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderMonthlyEnergyRangeEntity>(),
            (list, next) =>
              list
                        .Take(list.Count == 0 ? 0 : list.Count - 1)
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderMonthlyEnergyRangeEntityExtensions.UpsertRow)
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
            .ToQuarterHourlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderQuarterHourlyEnergyRangeEntity>(),
            (list, next) =>
              list
                        .Take(list.Count == 0 ? 0 : list.Count - 1)
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(SchneiderQuarterHourlyEnergyRangeEntityExtensions.UpsertRow)
                .RunAsync();

        await context.SchneiderDailyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderDailyEnergyRangeEntity>(),
            (list, next) =>
              list
                        .Take(list.Count == 0 ? 0 : list.Count - 1)
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(SchneiderDailyEnergyRangeEntityExtensions.UpsertRow)
                .RunAsync();

        await context.SchneiderMonthlyEnergyRange
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyEnergyRangeEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderMonthlyEnergyRangeEntity>(),
            (list, next) =>
              list
                        .Take(list.Count == 0 ? 0 : list.Count - 1)
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
                .On(v => new { v.Tenant, v.Source, v.Timestamp })
                .WhenMatched(SchneiderMonthlyEnergyRangeEntityExtensions.UpsertRow)
                .RunAsync();
      });
  }
}
