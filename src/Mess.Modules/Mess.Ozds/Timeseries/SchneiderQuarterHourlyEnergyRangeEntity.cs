using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Prelude.Extensions.Timestamps;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

public class SchneiderQuarterHourlyEnergyRangeEntity : QuarterHourlyContinuousAggregateEntity
{
  [Column(TypeName = "float8")]
  public decimal ActiveEnergyImportTotalMin_Wh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyImportTotalMax_Wh { get; set; } = default!;

  [NotMapped]
  public decimal ActiveEnergyImport_Wh =>
    ActiveEnergyImportTotalMax_Wh - ActiveEnergyImportTotalMin_Wh;

  [NotMapped]
  public decimal ActivePowerImportAverage_W =>
    ActiveEnergyImport_Wh / (decimal)TimeSpan.TotalHours;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyExportTotalMin_Wh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyExportTotalMax_Wh { get; set; } = default!;

  [NotMapped]
  public decimal ActiveEnergyExport_Wh =>
    ActiveEnergyExportTotalMax_Wh - ActiveEnergyExportTotalMin_Wh;

  [NotMapped]
  public decimal ActivePowerExportAverage_W =>
    ActiveEnergyExport_Wh / (decimal)TimeSpan.TotalHours;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyImportTotalMin_VARh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyImportTotalMax_VARh { get; set; } = default!;

  [NotMapped]
  public decimal ReactiveEnergyImport_VARh =>
    ReactiveEnergyImportTotalMax_VARh - ReactiveEnergyImportTotalMin_VARh;

  [NotMapped]
  public decimal ReactivePowerImportAverage_VAR =>
    ReactiveEnergyImport_VARh / (decimal)TimeSpan.TotalHours;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyExportTotalMin_VARh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyExportTotalMax_VARh { get; set; } = default!;

  [NotMapped]
  public decimal ReactiveEnergyExport_VARh =>
    ReactiveEnergyExportTotalMax_VARh - ReactiveEnergyExportTotalMin_VARh;

  [NotMapped]
  public decimal ReactivePowerExportAverage_VAR =>
    ReactiveEnergyExport_VARh / (decimal)TimeSpan.TotalHours;
};

public static class SchneiderQuarterHourlyEnergyRangeEntityExtensions
{
  public static SchneiderQuarterHourlyEnergyRangeEntity ToQuarterHourlyEnergyRangeEntity(
    this SchneiderMeasurement measurement,
    string tenant
  )
  {
    return new SchneiderQuarterHourlyEnergyRangeEntity
    {
      Tenant = tenant,
      Source = measurement.Source,
      Timestamp = measurement.Timestamp.GetStartOfMonth(),
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

  public static List<SchneiderQuarterHourlyEnergyRangeEntity> Upsert(
    this SchneiderQuarterHourlyEnergyRangeEntity previous,
    SchneiderQuarterHourlyEnergyRangeEntity next
  ) =>
    previous.Timestamp == next.Timestamp ?
    new()
    {
      new SchneiderQuarterHourlyEnergyRangeEntity
      {
        Tenant = previous.Tenant,
        Source = previous.Source,
        Timestamp = previous.Timestamp,
        ActiveEnergyImportTotalMin_Wh = previous.ActiveEnergyExportTotalMin_Wh,
        ActiveEnergyImportTotalMax_Wh = next.ActiveEnergyImportTotalMax_Wh,
        ActiveEnergyExportTotalMin_Wh = previous.ActiveEnergyImportTotalMin_Wh,
        ActiveEnergyExportTotalMax_Wh = next.ActiveEnergyImportTotalMax_Wh,
        ReactiveEnergyImportTotalMin_VARh = previous.ActiveEnergyImportTotalMin_Wh,
        ReactiveEnergyImportTotalMax_VARh = next.ActiveEnergyImportTotalMax_Wh,
        ReactiveEnergyExportTotalMin_VARh = previous.ActiveEnergyImportTotalMin_Wh,
        ReactiveEnergyExportTotalMax_VARh = next.ActiveEnergyImportTotalMax_Wh,
      }
    } :
    new()
    {
      previous,
      next
    };

  public static readonly Expression<Func<SchneiderQuarterHourlyEnergyRangeEntity, SchneiderQuarterHourlyEnergyRangeEntity, SchneiderQuarterHourlyEnergyRangeEntity>>
  UpsertRow = (
    SchneiderQuarterHourlyEnergyRangeEntity previous,
    SchneiderQuarterHourlyEnergyRangeEntity next
  ) =>
    new SchneiderQuarterHourlyEnergyRangeEntity
    {
      Tenant = previous.Tenant,
      Source = previous.Source,
      Timestamp = previous.Timestamp,
      ActiveEnergyImportTotalMin_Wh = previous.ActiveEnergyExportTotalMin_Wh,
      ActiveEnergyImportTotalMax_Wh = next.ActiveEnergyImportTotalMax_Wh,
      ActiveEnergyExportTotalMin_Wh = previous.ActiveEnergyImportTotalMin_Wh,
      ActiveEnergyExportTotalMax_Wh = next.ActiveEnergyImportTotalMax_Wh,
      ReactiveEnergyImportTotalMin_VARh = previous.ActiveEnergyImportTotalMin_Wh,
      ReactiveEnergyImportTotalMax_VARh = next.ActiveEnergyImportTotalMax_Wh,
      ReactiveEnergyExportTotalMin_VARh = previous.ActiveEnergyImportTotalMin_Wh,
      ReactiveEnergyExportTotalMax_VARh = next.ActiveEnergyImportTotalMax_Wh,
    };

  public static SchneiderEnergyRange ToModel(
    this SchneiderQuarterHourlyEnergyRangeEntity entity
  )
  {
    return new SchneiderEnergyRange(
      entity.Source,
      entity.Timestamp,
      entity.TimeSpan,
      entity.ActiveEnergyImportTotalMin_Wh,
      entity.ActiveEnergyImportTotalMax_Wh,
      entity.ActiveEnergyImport_Wh,
      entity.ActivePowerImportAverage_W,
      entity.ActiveEnergyExportTotalMin_Wh,
      entity.ActiveEnergyExportTotalMax_Wh,
      entity.ActiveEnergyExport_Wh,
      entity.ActivePowerExportAverage_W,
      entity.ReactiveEnergyImportTotalMin_VARh,
      entity.ReactiveEnergyImportTotalMax_VARh,
      entity.ReactiveEnergyImport_VARh,
      entity.ReactivePowerImportAverage_VAR,
      entity.ReactiveEnergyExportTotalMin_VARh,
      entity.ReactiveEnergyExportTotalMax_VARh,
      entity.ReactiveEnergyExport_VARh,
      entity.ReactivePowerExportAverage_VAR
    );
  }
}
