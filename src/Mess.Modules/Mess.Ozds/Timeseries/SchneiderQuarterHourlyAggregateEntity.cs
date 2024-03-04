using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Prelude.Extensions.Timestamps;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

public class SchneiderQuarterHourlyAggregateEntity : QuarterHourlyContinuousAggregateEntity
{
  [Column(TypeName = "float8")]
  public decimal VoltageL1Avg_V { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal VoltageL2Avg_V { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal VoltageL3Avg_V { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal CurrentL1Avg_A { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal CurrentL2Avg_A { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal CurrentL3Avg_A { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActivePowerL1Avg_W { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActivePowerL2Avg_W { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActivePowerL3Avg_W { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ReactivePowerTotalAvg_VAR { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ApparentPowerTotalAvg_VA { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyImportTotalMin_Wh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyImportTotalMax_Wh { get; set; } = default!;

  [NotMapped]
  public decimal ActiveEnergyImport_Wh =>
    ActiveEnergyImportTotalMax_Wh - ActiveEnergyImportTotalMin_Wh;

  [NotMapped]
  public decimal ActivePowerImportAvg_W =>
    ActiveEnergyImport_Wh / (decimal)TimeSpan.TotalHours;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyExportTotalMin_Wh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyExportTotalMax_Wh { get; set; } = default!;

  [NotMapped]
  public decimal ActiveEnergyExport_Wh =>
    ActiveEnergyExportTotalMax_Wh - ActiveEnergyExportTotalMin_Wh;

  [NotMapped]
  public decimal ActivePowerExportAvg_W =>
    ActiveEnergyExport_Wh / (decimal)TimeSpan.TotalHours;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyImportTotalMin_VARh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyImportTotalMax_VARh { get; set; } = default!;

  [NotMapped]
  public decimal ReactiveEnergyImport_VARh =>
    ReactiveEnergyImportTotalMax_VARh - ReactiveEnergyImportTotalMin_VARh;

  [NotMapped]
  public decimal ReactivePowerImportAvg_VAR =>
    ReactiveEnergyImport_VARh / (decimal)TimeSpan.TotalHours;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyExportTotalMin_VARh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyExportTotalMax_VARh { get; set; } = default!;

  [NotMapped]
  public decimal ReactiveEnergyExport_VARh =>
    ReactiveEnergyExportTotalMax_VARh - ReactiveEnergyExportTotalMin_VARh;

  [NotMapped]
  public decimal ReactivePowerExportAvg_VAR =>
    ReactiveEnergyExport_VARh / (decimal)TimeSpan.TotalHours;
};

public static class SchneiderQuarterHourlyAggregateEntityExtensions
{
  public static SchneiderQuarterHourlyAggregateEntity ToQuarterHourlyAggregateEntity(
    this SchneiderMeasurement measurement,
    string tenant
  )
  {
    return new SchneiderQuarterHourlyAggregateEntity
    {
      Tenant = tenant,
      Source = measurement.Source,
      Timestamp = measurement.Timestamp.GetStartOfQuarterHour(),
      AggregateCount = 1,
      VoltageL1Avg_V = measurement.VoltageL1_V,
      VoltageL2Avg_V = measurement.VoltageL2_V,
      VoltageL3Avg_V = measurement.VoltageL3_V,
      CurrentL1Avg_A = measurement.CurrentL1_A,
      CurrentL2Avg_A = measurement.CurrentL2_A,
      CurrentL3Avg_A = measurement.CurrentL3_A,
      ActivePowerL1Avg_W = measurement.ActivePowerL1_W,
      ActivePowerL2Avg_W = measurement.ActivePowerL2_W,
      ActivePowerL3Avg_W = measurement.ActivePowerL3_W,
      ReactivePowerTotalAvg_VAR = measurement.ReactivePowerTotal_VAR,
      ApparentPowerTotalAvg_VA = measurement.ApparentPowerTotal_VA,
      ActiveEnergyImportTotalMin_Wh = measurement.ActiveEnergyImportTotal_Wh,
      ActiveEnergyImportTotalMax_Wh = measurement.ActiveEnergyImportTotal_Wh,
      ActiveEnergyExportTotalMin_Wh = measurement.ActiveEnergyImportTotal_Wh,
      ActiveEnergyExportTotalMax_Wh = measurement.ActiveEnergyImportTotal_Wh,
      ReactiveEnergyImportTotalMin_VARh = measurement.ActiveEnergyImportTotal_Wh,
      ReactiveEnergyImportTotalMax_VARh = measurement.ActiveEnergyImportTotal_Wh,
      ReactiveEnergyExportTotalMin_VARh = measurement.ActiveEnergyImportTotal_Wh,
      ReactiveEnergyExportTotalMax_VARh = measurement.ActiveEnergyImportTotal_Wh,
    };
  }

  public static List<SchneiderQuarterHourlyAggregateEntity> UpsertRange(
    this IEnumerable<SchneiderQuarterHourlyAggregateEntity> aggregates
  ) =>
    aggregates
    .GroupBy(aggregate => (aggregate.Tenant, aggregate.Source))
    .SelectMany(group => group
      .OrderBy(range => range.Timestamp)
      .Aggregate(
        new List<SchneiderQuarterHourlyAggregateEntity>(),
        (list, next) =>
          list
            .Concat(
            list.LastOrDefault() is { } last
              ? last.Upsert(next)
              : new() { next }
            )
            .ToList()))
      .ToList();

  public static List<SchneiderQuarterHourlyAggregateEntity> Upsert(
    this SchneiderQuarterHourlyAggregateEntity previous,
    SchneiderQuarterHourlyAggregateEntity next
  )
  {
    if (previous.Tenant != next.Tenant || previous.Source != next.Source)
    {
      throw new InvalidOperationException($"Trying to upsert aggregate from {previous.Tenant} {previous.Source} with aggregate from {next.Tenant} {next.Source}");
    }

    return previous.Timestamp == next.Timestamp ?
      new()
      {
        new SchneiderQuarterHourlyAggregateEntity
        {
          Tenant = previous.Tenant,
          Source = previous.Source,
          Timestamp = previous.Timestamp,
        AggregateCount = previous.AggregateCount + next.AggregateCount,
        VoltageL1Avg_V =
            (previous.VoltageL1Avg_V * previous.AggregateCount
              + next.VoltageL1Avg_V * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        VoltageL2Avg_V =
            (previous.VoltageL2Avg_V * previous.AggregateCount
              + next.VoltageL2Avg_V * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        VoltageL3Avg_V =
            (previous.VoltageL3Avg_V * previous.AggregateCount
              + next.VoltageL3Avg_V * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        CurrentL1Avg_A =
            (previous.CurrentL1Avg_A * previous.AggregateCount
              + next.CurrentL1Avg_A * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        CurrentL2Avg_A =
            (previous.CurrentL2Avg_A * previous.AggregateCount
              + next.CurrentL2Avg_A * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        CurrentL3Avg_A =
            (previous.CurrentL3Avg_A * previous.AggregateCount
              + next.CurrentL3Avg_A * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        ActivePowerL1Avg_W =
            (previous.ActivePowerL1Avg_W * previous.AggregateCount
              + next.ActivePowerL1Avg_W * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        ActivePowerL2Avg_W =
            (previous.ActivePowerL2Avg_W * previous.AggregateCount
              + next.ActivePowerL2Avg_W * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        ActivePowerL3Avg_W =
            (previous.ActivePowerL3Avg_W * previous.AggregateCount
              + next.ActivePowerL3Avg_W * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        ReactivePowerTotalAvg_VAR =
            (previous.ReactivePowerTotalAvg_VAR * previous.AggregateCount
              + next.ReactivePowerTotalAvg_VAR * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        ApparentPowerTotalAvg_VA =
            (previous.ApparentPowerTotalAvg_VA * previous.AggregateCount
              + next.ApparentPowerTotalAvg_VA * next.AggregateCount)
              / (previous.AggregateCount + next.AggregateCount),
        ActiveEnergyImportTotalMin_Wh =
            previous.ActiveEnergyImportTotalMin_Wh < next.ActiveEnergyImportTotalMin_Wh
            ? previous.ActiveEnergyImportTotalMin_Wh
            : next.ActiveEnergyImportTotalMin_Wh,
        ActiveEnergyImportTotalMax_Wh =
            previous.ActiveEnergyImportTotalMax_Wh > next.ActiveEnergyImportTotalMax_Wh
              ? previous.ActiveEnergyImportTotalMax_Wh
              : next.ActiveEnergyImportTotalMax_Wh,
        ActiveEnergyExportTotalMin_Wh =
            previous.ActiveEnergyExportTotalMin_Wh < next.ActiveEnergyExportTotalMin_Wh
            ? previous.ActiveEnergyExportTotalMin_Wh
            : next.ActiveEnergyExportTotalMin_Wh,
        ActiveEnergyExportTotalMax_Wh =
            previous.ActiveEnergyExportTotalMax_Wh > next.ActiveEnergyExportTotalMax_Wh
              ? previous.ActiveEnergyExportTotalMax_Wh
              : next.ActiveEnergyExportTotalMax_Wh,
        ReactiveEnergyImportTotalMin_VARh =
            previous.ReactiveEnergyImportTotalMin_VARh < next.ReactiveEnergyImportTotalMin_VARh
            ? previous.ReactiveEnergyImportTotalMin_VARh
            : next.ReactiveEnergyImportTotalMin_VARh,
        ReactiveEnergyImportTotalMax_VARh =
            previous.ReactiveEnergyImportTotalMax_VARh > next.ReactiveEnergyImportTotalMax_VARh
              ? previous.ReactiveEnergyImportTotalMax_VARh
              : next.ReactiveEnergyImportTotalMax_VARh,
        ReactiveEnergyExportTotalMin_VARh =
            previous.ReactiveEnergyExportTotalMin_VARh < next.ReactiveEnergyExportTotalMin_VARh
            ? previous.ReactiveEnergyExportTotalMin_VARh
            : next.ReactiveEnergyExportTotalMin_VARh,
        ReactiveEnergyExportTotalMax_VARh =
            previous.ReactiveEnergyExportTotalMax_VARh > next.ReactiveEnergyExportTotalMax_VARh
              ? previous.ReactiveEnergyExportTotalMax_VARh
              : next.ReactiveEnergyExportTotalMax_VARh,
        }
      } :
      new()
      {
        previous,
        next
      };
  }

  public static readonly Expression<Func<SchneiderQuarterHourlyAggregateEntity, SchneiderQuarterHourlyAggregateEntity, SchneiderQuarterHourlyAggregateEntity>>
  UpsertRow = (
    SchneiderQuarterHourlyAggregateEntity previous,
    SchneiderQuarterHourlyAggregateEntity next
  ) =>
    new SchneiderQuarterHourlyAggregateEntity
    {
      Tenant = previous.Tenant,
      Source = previous.Source,
      Timestamp = previous.Timestamp,
      AggregateCount = previous.AggregateCount + next.AggregateCount,
      VoltageL1Avg_V =
          (previous.VoltageL1Avg_V * previous.AggregateCount
            + next.VoltageL1Avg_V * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      VoltageL2Avg_V =
          (previous.VoltageL2Avg_V * previous.AggregateCount
            + next.VoltageL2Avg_V * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      VoltageL3Avg_V =
          (previous.VoltageL3Avg_V * previous.AggregateCount
            + next.VoltageL3Avg_V * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      CurrentL1Avg_A =
          (previous.CurrentL1Avg_A * previous.AggregateCount
            + next.CurrentL1Avg_A * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      CurrentL2Avg_A =
          (previous.CurrentL2Avg_A * previous.AggregateCount
            + next.CurrentL2Avg_A * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      CurrentL3Avg_A =
          (previous.CurrentL3Avg_A * previous.AggregateCount
            + next.CurrentL3Avg_A * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      ActivePowerL1Avg_W =
          (previous.ActivePowerL1Avg_W * previous.AggregateCount
            + next.ActivePowerL1Avg_W * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      ActivePowerL2Avg_W =
          (previous.ActivePowerL2Avg_W * previous.AggregateCount
            + next.ActivePowerL2Avg_W * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      ActivePowerL3Avg_W =
          (previous.ActivePowerL3Avg_W * previous.AggregateCount
            + next.ActivePowerL3Avg_W * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      ReactivePowerTotalAvg_VAR =
          (previous.ReactivePowerTotalAvg_VAR * previous.AggregateCount
            + next.ReactivePowerTotalAvg_VAR * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      ApparentPowerTotalAvg_VA =
          (previous.ApparentPowerTotalAvg_VA * previous.AggregateCount
            + next.ApparentPowerTotalAvg_VA * next.AggregateCount)
            / (previous.AggregateCount + next.AggregateCount),
      ActiveEnergyImportTotalMin_Wh =
          previous.ActiveEnergyImportTotalMin_Wh < next.ActiveEnergyImportTotalMin_Wh
           ? previous.ActiveEnergyImportTotalMin_Wh
           : next.ActiveEnergyImportTotalMin_Wh,
      ActiveEnergyImportTotalMax_Wh =
          previous.ActiveEnergyImportTotalMax_Wh > next.ActiveEnergyImportTotalMax_Wh
            ? previous.ActiveEnergyImportTotalMax_Wh
            : next.ActiveEnergyImportTotalMax_Wh,
      ActiveEnergyExportTotalMin_Wh =
          previous.ActiveEnergyExportTotalMin_Wh < next.ActiveEnergyExportTotalMin_Wh
           ? previous.ActiveEnergyExportTotalMin_Wh
           : next.ActiveEnergyExportTotalMin_Wh,
      ActiveEnergyExportTotalMax_Wh =
          previous.ActiveEnergyExportTotalMax_Wh > next.ActiveEnergyExportTotalMax_Wh
            ? previous.ActiveEnergyExportTotalMax_Wh
            : next.ActiveEnergyExportTotalMax_Wh,
      ReactiveEnergyImportTotalMin_VARh =
          previous.ReactiveEnergyImportTotalMin_VARh < next.ReactiveEnergyImportTotalMin_VARh
           ? previous.ReactiveEnergyImportTotalMin_VARh
           : next.ReactiveEnergyImportTotalMin_VARh,
      ReactiveEnergyImportTotalMax_VARh =
          previous.ReactiveEnergyImportTotalMax_VARh > next.ReactiveEnergyImportTotalMax_VARh
            ? previous.ReactiveEnergyImportTotalMax_VARh
            : next.ReactiveEnergyImportTotalMax_VARh,
      ReactiveEnergyExportTotalMin_VARh =
          previous.ReactiveEnergyExportTotalMin_VARh < next.ReactiveEnergyExportTotalMin_VARh
           ? previous.ReactiveEnergyExportTotalMin_VARh
           : next.ReactiveEnergyExportTotalMin_VARh,
      ReactiveEnergyExportTotalMax_VARh =
          previous.ReactiveEnergyExportTotalMax_VARh > next.ReactiveEnergyExportTotalMax_VARh
            ? previous.ReactiveEnergyExportTotalMax_VARh
            : next.ReactiveEnergyExportTotalMax_VARh,
    };

  public static SchneiderAggregate ToModel(
    this SchneiderQuarterHourlyAggregateEntity entity
  )
  {
    return new SchneiderAggregate(
      entity.Source,
      entity.Timestamp,
      entity.TimeSpan,
      entity.AggregateCount,
      entity.VoltageL1Avg_V,
      entity.VoltageL2Avg_V,
      entity.VoltageL3Avg_V,
      entity.CurrentL1Avg_A,
      entity.CurrentL2Avg_A,
      entity.CurrentL3Avg_A,
      entity.ActivePowerL1Avg_W,
      entity.ActivePowerL2Avg_W,
      entity.ActivePowerL3Avg_W,
      entity.ReactivePowerTotalAvg_VAR,
      entity.ApparentPowerTotalAvg_VA,
      entity.ActiveEnergyImportTotalMin_Wh,
      entity.ActiveEnergyImportTotalMax_Wh,
      entity.ActiveEnergyImport_Wh,
      entity.ActivePowerImportAvg_W,
      entity.ActiveEnergyExportTotalMin_Wh,
      entity.ActiveEnergyExportTotalMax_Wh,
      entity.ActiveEnergyExport_Wh,
      entity.ActivePowerExportAvg_W,
      entity.ReactiveEnergyImportTotalMin_VARh,
      entity.ReactiveEnergyImportTotalMax_VARh,
      entity.ReactiveEnergyImport_VARh,
      entity.ReactivePowerImportAvg_VAR,
      entity.ReactiveEnergyExportTotalMin_VARh,
      entity.ReactiveEnergyExportTotalMax_VARh,
      entity.ReactiveEnergyExport_VARh,
      entity.ReactivePowerExportAvg_VAR
    );
  }
}
