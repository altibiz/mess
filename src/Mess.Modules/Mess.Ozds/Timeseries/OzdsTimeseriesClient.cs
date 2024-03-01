using FlexLabs.EntityFrameworkCore.Upsert;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Iot.Abstractions.Caches;
using Mess.Ozds.Abstractions.Billing;
using Mess.Ozds.Abstractions.Models;
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

  private readonly IIotDeviceContentItemCache _itemCache;

  public OzdsTimeseriesClient(
    IServiceProvider services,
    ShellSettings shellSettings,
    IIotDeviceContentItemCache itemCache
  )
  {
    _services = services;
    _shellSettings = shellSettings;
    _itemCache = itemCache;
  }

  public void AddAbbMeasurement(AbbMeasurement model)
  {
    var item = _itemCache.GetIotDeviceContent<AbbIotDeviceItem>(model.Source);
    if (item is null || !model.IsValid(item))
    {
      return;
    }

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

  public async Task AddAbbMeasurementAsync(AbbMeasurement model)
  {
    var item = await _itemCache.GetIotDeviceContentAsync<AbbIotDeviceItem>(model.Source);
    if (item is null || !model.IsValid(item))
    {
      return;
    }

    await _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
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
    var item = _itemCache.GetIotDeviceContent<SchneiderIotDeviceItem>(model.Source);
    if (item is null || !model.IsValid(item))
    {
      return;
    }

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

  public async Task AddSchneiderMeasurementAsync(SchneiderMeasurement model)
  {
    var item = await _itemCache.GetIotDeviceContentAsync<SchneiderIotDeviceItem>(model.Source);
    if (item is null || !model.IsValid(item))
    {
      return;
    }

    await _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
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
    var filtered = new List<AbbMeasurement>();
    foreach (var measurement in measurements)
    {

      var item = _itemCache.GetIotDeviceContent<AbbIotDeviceItem>(measurement.Source);
      if (item is null || !measurement.IsValid(item))
      {
        continue;
      }

      filtered.Add(measurement);
    }

    if (filtered.Count is 0)
    {
      return;
    }

    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.AbbMeasurements
        .UpsertRange(filtered
          .Select(measurement => measurement
            .ToEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();

      context.AbbQuarterHourlyAggregate
        .UpsertRange(filtered
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
        .UpsertRange(filtered
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
        .UpsertRange(filtered
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

  public async Task AddBulkAbbMeasurementAsync(IEnumerable<AbbMeasurement> measurements)
  {
    var filtered = new List<AbbMeasurement>();
    foreach (var measurement in measurements)
    {

      var item = await _itemCache.GetIotDeviceContentAsync<AbbIotDeviceItem>(measurement.Source);
      if (item is null || !measurement.IsValid(item))
      {
        continue;
      }

      filtered.Add(measurement);
    }

    if (filtered.Count is 0)
    {
      return;
    }

    await _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.AbbMeasurements
          .UpsertRange(filtered
            .Select(measurement => measurement
              .ToEntity(_shellSettings.GetDatabaseTablePrefix())))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();

        await context.AbbQuarterHourlyAggregate
        .UpsertRange(
        filtered
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
        .UpsertRange(filtered
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
        .UpsertRange(filtered
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
    var filtered = new List<SchneiderMeasurement>();
    foreach (var measurement in measurements)
    {

      var item = _itemCache.GetIotDeviceContent<SchneiderIotDeviceItem>(measurement.Source);
      if (item is null || !measurement.IsValid(item))
      {
        continue;
      }

      filtered.Add(measurement);
    }

    if (filtered.Count is 0)
    {
      return;
    }

    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.SchneiderMeasurements
        .UpsertRange(filtered
          .Select(measurement => measurement
            .ToEntity(_shellSettings.GetDatabaseTablePrefix())))
        .On(v => new { v.Tenant, v.Source, v.Timestamp })
        .NoUpdate()
        .Run();

      context.SchneiderQuarterHourlyAggregate
        .UpsertRange(filtered
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
        .UpsertRange(filtered
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
        .UpsertRange(filtered
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

  public async Task AddBulkSchneiderMeasurementAsync(IEnumerable<SchneiderMeasurement> measurements)
  {
    var filtered = new List<SchneiderMeasurement>();
    foreach (var measurement in measurements)
    {

      var item = await _itemCache.GetIotDeviceContentAsync<SchneiderIotDeviceItem>(measurement.Source);
      if (item is null || !measurement.IsValid(item))
      {
        continue;
      }

      filtered.Add(measurement);
    }

    if (filtered.Count is 0)
    {
      return;
    }

    await _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(
      async context =>
      {
        await context.SchneiderMeasurements
          .UpsertRange(filtered
            .Select(measurement => measurement
              .ToEntity(_shellSettings.GetDatabaseTablePrefix())))
          .On(v => new { v.Tenant, v.Source, v.Timestamp })
          .NoUpdate()
          .RunAsync();

        await context.SchneiderQuarterHourlyAggregate
        .UpsertRange(filtered
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
        .UpsertRange(filtered
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
        .UpsertRange(filtered
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
