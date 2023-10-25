using System.ComponentModel.DataAnnotations.Schema;
using Mess.Ozds.Abstractions.Timeseries;

namespace Mess.Ozds.Timeseries;

public class SchneiderMeasurementEntity : BillingEntity
{
  [Column(TypeName = "float4")]
  public float? CurrentL1 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? CurrentL2 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? CurrentL3 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? VoltageL1 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? VoltageL2 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? VoltageL3 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? ActivePowerL1 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? ActivePowerL2 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? ActivePowerL3 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? ReactivePowerL1 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? ReactivePowerL2 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? ReactivePowerL3 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? ApparentPowerL1 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? ApparentPowerL2 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? ApparentPowerL3 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? PowerFactorL1 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? PowerFactorL2 { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float? PowerFactorL3 { get; set; } = default!;
}

public static class SchneiderMeasurementEntityExtensions
{
  public static SchneiderMeasurementEntity ToEntity(
    this SchneiderMeasurement model
  ) =>
    new SchneiderMeasurementEntity
    {
      Tenant = model.Tenant,
      Timestamp = model.Timestamp,
      Source = model.DeviceId,
      CurrentL1 = model.CurrentL1,
      CurrentL2 = model.CurrentL2,
      CurrentL3 = model.CurrentL3,
      VoltageL1 = model.VoltageL1,
      VoltageL2 = model.VoltageL2,
      VoltageL3 = model.VoltageL3,
      ActivePowerL1 = model.ActivePowerL1,
      ActivePowerL2 = model.ActivePowerL2,
      ActivePowerL3 = model.ActivePowerL3,
      ReactivePowerL1 = model.ReactivePowerL1,
      ReactivePowerL2 = model.ReactivePowerL2,
      ReactivePowerL3 = model.ReactivePowerL3,
      ApparentPowerL1 = model.ApparentPowerL1,
      ApparentPowerL2 = model.ApparentPowerL2,
      ApparentPowerL3 = model.ApparentPowerL3,
      PowerFactorL1 = model.PowerFactorL1,
      PowerFactorL2 = model.PowerFactorL2,
      PowerFactorL3 = model.PowerFactorL3,
      Energy = model.Energy,
      LowEnergy = model.LowEnergy,
      HighEnergy = model.HighEnergy,
      Power = model.Power
    };

  public static SchneiderMeasurement ToModel(
    this SchneiderMeasurementEntity entity
  ) =>
    new SchneiderMeasurement(
      Tenant: entity.Tenant,
      DeviceId: entity.Source,
      Timestamp: entity.Timestamp,
      CurrentL1: entity.CurrentL1,
      CurrentL2: entity.CurrentL2,
      CurrentL3: entity.CurrentL3,
      VoltageL1: entity.VoltageL1,
      VoltageL2: entity.VoltageL2,
      VoltageL3: entity.VoltageL3,
      ActivePowerL1: entity.ActivePowerL1,
      ActivePowerL2: entity.ActivePowerL2,
      ActivePowerL3: entity.ActivePowerL3,
      ReactivePowerL1: entity.ReactivePowerL1,
      ReactivePowerL2: entity.ReactivePowerL2,
      ReactivePowerL3: entity.ReactivePowerL3,
      ApparentPowerL1: entity.ApparentPowerL1,
      ApparentPowerL2: entity.ApparentPowerL2,
      ApparentPowerL3: entity.ApparentPowerL3,
      PowerFactorL1: entity.PowerFactorL1,
      PowerFactorL2: entity.PowerFactorL2,
      PowerFactorL3: entity.PowerFactorL3,
      Energy: entity.Energy,
      LowEnergy: entity.LowEnergy,
      HighEnergy: entity.HighEnergy,
      Power: entity.Power
    );
}
