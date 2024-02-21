using System.ComponentModel.DataAnnotations.Schema;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Ozds.Timeseries;

public class AbbMeasurementEntity : HypertableEntity
{
  [Column(TypeName = "double precision")] public decimal VoltageL1_V { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal VoltageL2_V { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal VoltageL3_V { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal CurrentL1_A { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal CurrentL2_A { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal CurrentL3_A { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActivePowerL1_W { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActivePowerL2_W { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActivePowerL3_W { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactivePowerL1_VAR { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactivePowerL2_VAR { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactivePowerL3_VAR { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActivePowerImportL1_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActivePowerImportL2_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActivePowerImportL3_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActivePowerExportL1_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActivePowerExportL2_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActivePowerExportL3_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactivePowerImportL1_VARh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactivePowerImportL2_VARh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactivePowerImportL3_VARh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactivePowerExportL1_VARh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactivePowerExportL2_VARh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactivePowerExportL3_VARh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActiveEnergyImportTotal_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActiveEnergyExportTotal_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactiveEnergyImportTotal_VARh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactiveEnergyExportTotal_VARh { get; set; } = default;
}

public static class AbbMeasurementEntityExtensions
{
  public static AbbMeasurementEntity ToEntity(this AbbMeasurement model)
  {
    return new AbbMeasurementEntity
    {
      Timestamp = model.Timestamp,
      Source = model.Source,
      VoltageL1_V = model.VoltageL1_V,
      VoltageL2_V = model.VoltageL2_V,
      VoltageL3_V = model.VoltageL3_V,
      CurrentL1_A = model.CurrentL1_A,
      CurrentL2_A = model.CurrentL2_A,
      CurrentL3_A = model.CurrentL3_A,
      ActivePowerL1_W = model.ActivePowerL1_W,
      ActivePowerL2_W = model.ActivePowerL2_W,
      ActivePowerL3_W = model.ActivePowerL3_W,
      ReactivePowerL1_VAR = model.ReactivePowerL1_VAR,
      ReactivePowerL2_VAR = model.ReactivePowerL2_VAR,
      ReactivePowerL3_VAR = model.ReactivePowerL3_VAR,
      ActivePowerImportL1_Wh = model.ActivePowerImportL1_Wh,
      ActivePowerImportL2_Wh = model.ActivePowerImportL2_Wh,
      ActivePowerImportL3_Wh = model.ActivePowerImportL3_Wh,
      ActivePowerExportL1_Wh = model.ActivePowerExportL1_Wh,
      ActivePowerExportL2_Wh = model.ActivePowerExportL2_Wh,
      ActivePowerExportL3_Wh = model.ActivePowerExportL3_Wh,
      ReactivePowerImportL1_VARh = model.ReactivePowerImportL1_VARh,
      ReactivePowerImportL2_VARh = model.ReactivePowerImportL2_VARh,
      ReactivePowerImportL3_VARh = model.ReactivePowerImportL3_VARh,
      ReactivePowerExportL1_VARh = model.ReactivePowerExportL1_VARh,
      ReactivePowerExportL2_VARh = model.ReactivePowerExportL2_VARh,
      ReactivePowerExportL3_VARh = model.ReactivePowerExportL3_VARh,
      ActiveEnergyImportTotal_Wh = model.ActiveEnergyImportTotal_Wh,
      ActiveEnergyExportTotal_Wh = model.ActiveEnergyExportTotal_Wh,
      ReactiveEnergyImportTotal_VARh = model.ReactiveEnergyImportTotal_VARh,
      ReactiveEnergyExportTotal_VARh = model.ReactiveEnergyExportTotal_VARh
    };
  }

  public static AbbMeasurement ToModel(this AbbMeasurementEntity entity)
  {
    return new AbbMeasurement(
      entity.Source,
      entity.Timestamp,
      entity.VoltageL1_V,
      entity.VoltageL2_V,
      entity.VoltageL3_V,
      entity.CurrentL1_A,
      entity.CurrentL2_A,
      entity.CurrentL3_A,
      entity.ActivePowerL1_W,
      entity.ActivePowerL2_W,
      entity.ActivePowerL3_W,
      entity.ReactivePowerL1_VAR,
      entity.ReactivePowerL2_VAR,
      entity.ReactivePowerL3_VAR,
      entity.ActivePowerImportL1_Wh,
      entity.ActivePowerImportL2_Wh,
      entity.ActivePowerImportL3_Wh,
      entity.ActivePowerExportL1_Wh,
      entity.ActivePowerExportL2_Wh,
      entity.ActivePowerExportL3_Wh,
      entity.ReactivePowerImportL1_VARh,
      entity.ReactivePowerImportL2_VARh,
      entity.ReactivePowerImportL3_VARh,
      entity.ReactivePowerExportL1_VARh,
      entity.ReactivePowerExportL2_VARh,
      entity.ReactivePowerExportL3_VARh,
      entity.ActiveEnergyImportTotal_Wh,
      entity.ActiveEnergyExportTotal_Wh,
      entity.ReactiveEnergyImportTotal_VARh,
      entity.ReactiveEnergyExportTotal_VARh
    );
  }
}
