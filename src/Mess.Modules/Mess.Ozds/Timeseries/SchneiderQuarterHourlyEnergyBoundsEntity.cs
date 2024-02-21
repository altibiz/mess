using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

public class SchneiderQuarterHourlyEnergyBoundsEntity : QuarterHourlyContinuousAggregateEntity
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
