using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

public class AbbMonthlyEnergyBoundsEntity : MonthlyContinuousAggregateEntity
{
  [Column(TypeName = "float8")]
  public decimal ActiveEnergyImportTotalMin_Wh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyImportTotalMax_Wh { get; set; } = default!;

  [NotMapped]
  public decimal ActivePowerImportAverage_W =>
    (ActiveEnergyImportTotalMax_Wh - ActiveEnergyImportTotalMin_Wh) *
      (decimal)TimeSpan.TotalHours;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyExportTotalMin_Wh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyExportTotalMax_Wh { get; set; } = default!;

  [NotMapped]
  public decimal ActivePowerExportAverage_W =>
    (ActiveEnergyExportTotalMax_Wh - ActiveEnergyExportTotalMin_Wh) *
      (decimal)TimeSpan.TotalHours;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyImportTotalMin_VARh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyImportTotalMax_VARh { get; set; } = default!;

  [NotMapped]
  public decimal ReactivePowerImportAverage_VAR =>
    (ReactiveEnergyImportTotalMax_VARh - ReactiveEnergyImportTotalMin_VARh) *
      (decimal)TimeSpan.TotalHours;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyExportTotalMin_VARh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ReactiveEnergyExportTotalMax_VARh { get; set; } = default!;

  [NotMapped]
  public decimal ReactivePowerExportAverage_VAR =>
    (ReactiveEnergyExportTotalMax_VARh - ReactiveEnergyExportTotalMin_VARh) *
      (decimal)TimeSpan.TotalHours;
};
