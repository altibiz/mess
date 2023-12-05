using System.ComponentModel.DataAnnotations.Schema;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Ozds.Timeseries;

public class AbbMeasurementEntity : HypertableEntity
{
  [Column(TypeName = "int4")] public int? VoltageL1_V { get; set; }

  [Column(TypeName = "int4")] public int? VoltageL2_V { get; set; }

  [Column(TypeName = "int4")] public int? VoltageL3_V { get; set; }

  [Column(TypeName = "int4")] public int? CurrentL1_A { get; set; }

  [Column(TypeName = "int4")] public int? CurrentL2_A { get; set; }

  [Column(TypeName = "int4")] public int? CurrentL3_A { get; set; }

  [Column(TypeName = "int4")] public int? ActivePowerTotal_W { get; set; }

  [Column(TypeName = "int4")] public int? ActivePowerL1_W { get; set; }

  [Column(TypeName = "int4")] public int? ActivePowerL2_W { get; set; }

  [Column(TypeName = "int4")] public int? ActivePowerL3_W { get; set; }

  [Column(TypeName = "int4")] public int? ReactivePowerTotal_VAR { get; set; }

  [Column(TypeName = "int4")] public int? ReactivePowerL1_VAR { get; set; }

  [Column(TypeName = "int4")] public int? ReactivePowerL2_VAR { get; set; }

  [Column(TypeName = "int4")] public int? ReactivePowerL3_VAR { get; set; }

  [Column(TypeName = "int4")] public int? ApparentPowerTotal_VA { get; set; }

  [Column(TypeName = "int4")] public int? ApparentPowerL1_VA { get; set; }

  [Column(TypeName = "int4")] public int? ApparentPowerL2_VA { get; set; }

  [Column(TypeName = "int4")] public int? ApparentPowerL3_VA { get; set; }

  [Column(TypeName = "int2")] public short? PowerFactorTotal { get; set; }

  [Column(TypeName = "int2")] public short? PowerFactorL1 { get; set; }

  [Column(TypeName = "int2")] public short? PowerFactorL2 { get; set; }

  [Column(TypeName = "int2")] public short? PowerFactorL3 { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyImportTotal_kWh { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyExportTotal_kWh { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyNetTotal_kWh { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyImportTariff1_kWh { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyImportTariff2_kWh { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyExportTariff1_kWh { get; set; }

  [Column(TypeName = "int8")]
  public long? ActiveEnergyExportTariff2_kWh { get; set; }
}

// TODO: safer conversions

public static class AbbMeasurementEntityExtensions
{
  public static AbbMeasurementEntity ToEntity(this AbbMeasurement model)
  {
    return new AbbMeasurementEntity
    {
      Tenant = model.Tenant,
      Timestamp = model.Timestamp,
      Source = model.DeviceId,
      VoltageL1_V = (int?)model.VoltageL1_V,
      VoltageL2_V = (int?)model.VoltageL2_V,
      VoltageL3_V = (int?)model.VoltageL3_V,
      CurrentL1_A = (int?)model.CurrentL1_A,
      CurrentL2_A = (int?)model.CurrentL2_A,
      CurrentL3_A = (int?)model.CurrentL3_A,
      ActivePowerTotal_W = (int?)model.ActivePowerTotal_W,
      ActivePowerL1_W = (int?)model.ActivePowerL1_W,
      ActivePowerL2_W = (int?)model.ActivePowerL2_W,
      ActivePowerL3_W = (int?)model.ActivePowerL3_W,
      ReactivePowerTotal_VAR = (int?)model.ReactivePowerTotal_VAR,
      ReactivePowerL1_VAR = (int?)model.ReactivePowerL1_VAR,
      ReactivePowerL2_VAR = (int?)model.ReactivePowerL2_VAR,
      ReactivePowerL3_VAR = (int?)model.ReactivePowerL3_VAR,
      ApparentPowerTotal_VA = (int?)model.ApparentPowerTotal_VA,
      ApparentPowerL1_VA = (int?)model.ApparentPowerL1_VA,
      ApparentPowerL2_VA = (int?)model.ApparentPowerL2_VA,
      ApparentPowerL3_VA = (int?)model.ApparentPowerL3_VA,
      PowerFactorTotal = (short?)model.PowerFactorTotal,
      PowerFactorL1 = (short?)model.PowerFactorL1,
      PowerFactorL2 = (short?)model.PowerFactorL2,
      PowerFactorL3 = (short?)model.PowerFactorL3,
      ActiveEnergyImportTotal_kWh = (long?)model.ActiveEnergyImportTotal_kWh,
      ActiveEnergyExportTotal_kWh = (long?)model.ActiveEnergyExportTotal_kWh,
      ActiveEnergyNetTotal_kWh = (long?)model.ActiveEnergyNetTotal_kWh,
      ActiveEnergyImportTariff1_kWh = (long?)
        model.ActiveEnergyImportTariff1_kWh,
      ActiveEnergyImportTariff2_kWh = (long?)
        model.ActiveEnergyImportTariff2_kWh,
      ActiveEnergyExportTariff1_kWh = (long?)
        model.ActiveEnergyExportTariff1_kWh,
      ActiveEnergyExportTariff2_kWh = (long?)model.ActiveEnergyExportTariff2_kWh
    };
  }

  public static AbbMeasurement ToModel(this AbbMeasurementEntity entity)
  {
    return new AbbMeasurement(
      entity.Tenant,
      entity.Source,
      entity.Timestamp,
      entity.VoltageL1_V,
      entity.VoltageL2_V,
      entity.VoltageL3_V,
      entity.CurrentL1_A,
      entity.CurrentL2_A,
      entity.CurrentL3_A,
      entity.ActivePowerTotal_W,
      entity.ActivePowerL1_W,
      entity.ActivePowerL2_W,
      entity.ActivePowerL3_W,
      entity.ReactivePowerTotal_VAR,
      entity.ReactivePowerL1_VAR,
      entity.ReactivePowerL2_VAR,
      entity.ReactivePowerL3_VAR,
      entity.ApparentPowerTotal_VA,
      entity.ApparentPowerL1_VA,
      entity.ApparentPowerL2_VA,
      entity.ApparentPowerL3_VA,
      entity.PowerFactorTotal,
      entity.PowerFactorL1,
      entity.PowerFactorL2,
      entity.PowerFactorL3,
      entity.ActiveEnergyImportTotal_kWh,
      entity.ActiveEnergyExportTotal_kWh,
      entity.ActiveEnergyNetTotal_kWh,
      entity.ActiveEnergyImportTariff1_kWh,
      entity.ActiveEnergyImportTariff2_kWh,
      entity.ActiveEnergyExportTariff1_kWh,
      entity.ActiveEnergyExportTariff2_kWh
    );
  }
}
