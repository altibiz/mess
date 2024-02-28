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

      context.AbbQuarterHourlyAggregate
        .Upsert(model.ToQuarterHourlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbQuarterHourlyAggregateEntityExtensions.UpsertRow)
        .Run();

      context.AbbDailyAggregate
        .Upsert(model.ToDailyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbDailyAggregateEntityExtensions.UpsertRow)
        .Run();

      context.AbbMonthlyAggregate
        .Upsert(model.ToMonthlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbMonthlyAggregateEntityExtensions.UpsertRow)
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

        await context.AbbQuarterHourlyAggregate
          .Upsert(model.ToQuarterHourlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(AbbQuarterHourlyAggregateEntityExtensions.UpsertRow)
          .RunAsync();

        await context.AbbDailyAggregate
          .Upsert(model.ToDailyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(AbbDailyAggregateEntityExtensions.UpsertRow)
          .RunAsync();

        await context.AbbMonthlyAggregate
          .Upsert(model.ToMonthlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(AbbMonthlyAggregateEntityExtensions.UpsertRow)
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

      context.SchneiderQuarterHourlyAggregate
        .Upsert(model.ToQuarterHourlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderQuarterHourlyAggregateEntityExtensions.UpsertRow)
        .Run();

      context.SchneiderDailyAggregate
        .Upsert(model.ToDailyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderDailyAggregateEntityExtensions.UpsertRow)
        .Run();

      context.SchneiderMonthlyAggregate
        .Upsert(model.ToMonthlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(SchneiderMonthlyAggregateEntityExtensions.UpsertRow)
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

        await context.SchneiderQuarterHourlyAggregate
          .Upsert(model.ToQuarterHourlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(SchneiderQuarterHourlyAggregateEntityExtensions.UpsertRow)
          .RunAsync();

        await context.SchneiderDailyAggregate
          .Upsert(model.ToDailyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(SchneiderDailyAggregateEntityExtensions.UpsertRow)
          .RunAsync();

        await context.SchneiderMonthlyAggregate
          .Upsert(model.ToMonthlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .WhenMatched(SchneiderMonthlyAggregateEntityExtensions.UpsertRow)
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

      context.AbbQuarterHourlyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToQuarterHourlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbQuarterHourlyAggregateEntity>(),
            (list, next) =>
              list
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbQuarterHourlyAggregateEntityExtensions.UpsertRow)
        .Run();

      context.AbbDailyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbDailyAggregateEntity>(),
            (list, next) =>
              list
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbDailyAggregateEntityExtensions.UpsertRow)
        .Run();

      context.AbbMonthlyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbMonthlyAggregateEntity>(),
            (list, next) =>
              list
                .Concat(
                list.LastOrDefault() is { } last
                  ? last.Upsert(next)
                  : new() { next }
                )
                .ToList()))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .WhenMatched(AbbMonthlyAggregateEntityExtensions.UpsertRow)
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

        await context.AbbQuarterHourlyAggregate
        .UpsertRange(
        measurements
                  .Select(measurement => measurement
                    .ToQuarterHourlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
                  .OrderBy(range => range.Timestamp)
                  .Aggregate(
                    new List<AbbQuarterHourlyAggregateEntity>(),
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
                .WhenMatched(AbbQuarterHourlyAggregateEntityExtensions.UpsertRow)
                .RunAsync();

        await context.AbbDailyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbDailyAggregateEntity>(),
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
                .WhenMatched(AbbDailyAggregateEntityExtensions.UpsertRow)
                .RunAsync();

        await context.AbbMonthlyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<AbbMonthlyAggregateEntity>(),
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
                .WhenMatched(AbbMonthlyAggregateEntityExtensions.UpsertRow)
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

      context.SchneiderQuarterHourlyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToQuarterHourlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderQuarterHourlyAggregateEntity>(),
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
        .WhenMatched(SchneiderQuarterHourlyAggregateEntityExtensions.UpsertRow)
        .Run();

      context.SchneiderDailyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderDailyAggregateEntity>(),
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
        .WhenMatched(SchneiderDailyAggregateEntityExtensions.UpsertRow)
        .Run();

      context.SchneiderMonthlyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderMonthlyAggregateEntity>(),
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
        .WhenMatched(SchneiderMonthlyAggregateEntityExtensions.UpsertRow)
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

        await context.SchneiderQuarterHourlyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToQuarterHourlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderQuarterHourlyAggregateEntity>(),
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
                .WhenMatched(SchneiderQuarterHourlyAggregateEntityExtensions.UpsertRow)
                .RunAsync();

        await context.SchneiderDailyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToDailyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderDailyAggregateEntity>(),
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
                .WhenMatched(SchneiderDailyAggregateEntityExtensions.UpsertRow)
                .RunAsync();

        await context.SchneiderMonthlyAggregate
        .UpsertRange(measurements
          .Select(measurement => measurement
            .ToMonthlyAggregateEntity(_shellSettings.GetDatabaseTablePrefix()))
          .OrderBy(range => range.Timestamp)
          .Aggregate(
            new List<SchneiderMonthlyAggregateEntity>(),
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
                .WhenMatched(SchneiderMonthlyAggregateEntityExtensions.UpsertRow)
                .RunAsync();
      });
  }
}
