using System.ComponentModel.DataAnnotations.Schema;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Ozds.Timeseries;

public class SchneiderMeasurementEntity : HypertableEntity
{
  [Column(TypeName = "float4")] public float? VoltageL1_V { get; set; }

  [Column(TypeName = "float4")] public float? VoltageL2_V { get; set; }

  [Column(TypeName = "float4")] public float? VoltageL3_V { get; set; }

  [Column(TypeName = "float4")] public float? VoltageAvg_V { get; set; }

  [Column(TypeName = "float4")] public float? CurrentL1_A { get; set; }

  [Column(TypeName = "float4")] public float? CurrentL2_A { get; set; }

  [Column(TypeName = "float4")] public float? CurrentL3_A { get; set; }

  [Column(TypeName = "float4")] public float? CurrentAvg_A { get; set; }

  [Column(TypeName = "float4")] public float? ActivePowerL1_kW { get; set; }

  [Column(TypeName = "float4")] public float? ActivePowerL2_kW { get; set; }

  [Column(TypeName = "float4")] public float? ActivePowerL3_kW { get; set; }

  [Column(TypeName = "float4")] public float? ActivePowerTotal_kW { get; set; }

  [Column(TypeName = "float4")]
  public float? ReactivePowerTotal_kVAR { get; set; }

  [Column(TypeName = "float4")]
  public float? ApparentPowerTotal_kVA { get; set; }

  [Column(TypeName = "float4")] public float? PowerFactorTotal { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyImportTotal_Wh { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyExportTotal_Wh { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyImportRateA_Wh { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyImportRateB_Wh { get; set; }
}

// TODO: safer conversions

public static class SchneiderMeasurementEntityExtensions
{
  public static SchneiderMeasurementEntity ToEntity(
    this SchneiderMeasurement model
  )
  {
    return new SchneiderMeasurementEntity
    {
      Tenant = model.Tenant,
      Timestamp = model.Timestamp,
      Source = model.DeviceId,
      VoltageL1_V = (float?)model.VoltageL1_V,
      VoltageL2_V = (float?)model.VoltageL2_V,
      VoltageL3_V = (float?)model.VoltageL3_V,
      VoltageAvg_V = (float?)model.VoltageAvg_V,
      CurrentL1_A = (float?)model.CurrentL1_A,
      CurrentL2_A = (float?)model.CurrentL2_A,
      CurrentL3_A = (float?)model.CurrentL3_A,
      CurrentAvg_A = (float?)model.CurrentAvg_A,
      ActivePowerL1_kW = (float?)model.ActivePowerL1_kW,
      ActivePowerL2_kW = (float?)model.ActivePowerL2_kW,
      ActivePowerL3_kW = (float?)model.ActivePowerL3_kW,
      ActivePowerTotal_kW = (float?)model.ActivePowerTotal_kW,
      ReactivePowerTotal_kVAR = (float?)model.ReactivePowerTotal_kVAR,
      ApparentPowerTotal_kVA = (float?)model.ApparentPowerTotal_kVA,
      PowerFactorTotal = (float?)model.PowerFactorTotal,
      ActiveEnergyImportTotal_Wh = (long?)model.ActiveEnergyImportTotal_Wh,
      ActiveEnergyExportTotal_Wh = (long?)model.ActiveEnergyExportTotal_Wh,
      ActiveEnergyImportRateA_Wh = (long?)model.ActiveEnergyImportRateA_Wh,
      ActiveEnergyImportRateB_Wh = (long?)model.ActiveEnergyImportRateB_Wh
    };
  }

  public static SchneiderMeasurement ToModel(
    this SchneiderMeasurementEntity entity
  )
  {
    return new SchneiderMeasurement(
      entity.Tenant,
      entity.Source,
      entity.Timestamp,
      (decimal?)entity.VoltageL1_V,
      (decimal?)entity.VoltageL2_V,
      (decimal?)entity.VoltageL3_V,
      (decimal?)entity.VoltageAvg_V,
      (decimal?)entity.CurrentL1_A,
      (decimal?)entity.CurrentL2_A,
      (decimal?)entity.CurrentL3_A,
      (decimal?)entity.CurrentAvg_A,
      (decimal?)entity.ActivePowerL1_kW,
      (decimal?)entity.ActivePowerL2_kW,
      (decimal?)entity.ActivePowerL3_kW,
      (decimal?)entity.ActivePowerTotal_kW,
      (decimal?)entity.ReactivePowerTotal_kVAR,
      (decimal?)entity.ApparentPowerTotal_kVA,
      (decimal?)entity.PowerFactorTotal,
      entity.ActiveEnergyImportTotal_Wh,
      entity.ActiveEnergyExportTotal_Wh,
      entity.ActiveEnergyImportRateA_Wh,
      entity.ActiveEnergyImportRateB_Wh
    );
  }
}
