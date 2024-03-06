using System.ComponentModel.DataAnnotations.Schema;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Ozds.Timeseries;

public class SchneiderMeasurementEntity : HypertableEntity
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
  [Column(TypeName = "double precision")] public decimal ReactivePowerTotal_VAR { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ApparentPowerTotal_VA { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActiveEnergyImportL1_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActiveEnergyImportL2_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActiveEnergyImportL3_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActiveEnergyImportTotal_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActiveEnergyExportTotal_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactiveEnergyImportTotal_VARh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ReactiveEnergyExportTotal_VARh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActiveEnergyImportTotalT1_Wh { get; set; } = default;
  [Column(TypeName = "double precision")] public decimal ActiveEnergyImportTotalT2_Wh { get; set; } = default;
}

public static class SchneiderMeasurementEntityExtensions
{
  public static SchneiderMeasurementEntity ToEntity(
    this SchneiderMeasurement model,
    string tenant
  )
  {
    return new SchneiderMeasurementEntity
    {
      Tenant = tenant,
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
      ReactivePowerTotal_VAR = model.ReactivePowerTotal_VAR,
      ApparentPowerTotal_VA = model.ApparentPowerTotal_VA,
      ActiveEnergyImportL1_Wh = model.ActiveEnergyImportL1_Wh,
      ActiveEnergyImportL2_Wh = model.ActiveEnergyImportL2_Wh,
      ActiveEnergyImportL3_Wh = model.ActiveEnergyImportL3_Wh,
      ActiveEnergyImportTotal_Wh = model.ActiveEnergyImportTotal_Wh,
      ActiveEnergyExportTotal_Wh = model.ActiveEnergyExportTotal_Wh,
      ReactiveEnergyImportTotal_VARh = model.ReactiveEnergyImportTotal_VARh,
      ReactiveEnergyExportTotal_VARh = model.ReactiveEnergyExportTotal_VARh,
      ActiveEnergyImportTotalT1_Wh = model.ActiveEnergyImportTotalT1_Wh,
      ActiveEnergyImportTotalT2_Wh = model.ActiveEnergyImportTotalT2_Wh
    };
  }

  public static SchneiderMeasurement ToModel(
    this SchneiderMeasurementEntity entity
  )
  {
    return new SchneiderMeasurement(
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
      entity.ReactivePowerTotal_VAR,
      entity.ApparentPowerTotal_VA,
      entity.ActiveEnergyImportL1_Wh,
      entity.ActiveEnergyImportL2_Wh,
      entity.ActiveEnergyImportL3_Wh,
      entity.ActiveEnergyImportTotal_Wh,
      entity.ActiveEnergyExportTotal_Wh,
      entity.ReactiveEnergyImportTotal_VARh,
      entity.ReactiveEnergyExportTotal_VARh,
      entity.ActiveEnergyImportTotalT1_Wh,
      entity.ActiveEnergyImportTotalT2_Wh
    );
  }
}
