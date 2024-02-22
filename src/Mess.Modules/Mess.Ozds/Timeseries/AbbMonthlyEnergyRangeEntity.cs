using System.ComponentModel.DataAnnotations.Schema;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

public class AbbMonthlyEnergyRangeEntity : MonthlyContinuousAggregateEntity
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

public static class AbbMonthlyEnergyRangeEntityExtensions
{
  public static AbbEnergyRange ToModel(
    this AbbMonthlyEnergyRangeEntity entity
  )
  {
    return new AbbEnergyRange(
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
