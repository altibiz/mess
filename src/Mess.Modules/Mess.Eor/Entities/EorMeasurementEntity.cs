using System.ComponentModel.DataAnnotations.Schema;
using Mess.Eor.Abstractions.Client;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Eor.Entities;

public class EorMeasurementEntity : HypertableEntity
{
  [Column(TypeName = "float4")]
  public float Voltage { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float Current { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float Temperature { get; set; } = default!;

  public bool CoolingFans { get; set; } = default!;

  public bool HatsinkFans { get; set; } = default!;
}

public static class EorMeasurementEntityExtensions
{
  public static EorMeasurementEntity ToEntity(this EorMeasurement model) =>
    new EorMeasurementEntity
    {
      Tenant = model.Tenant,
      Timestamp = model.Timestamp,
      Source = model.DeviceId,
      Voltage = model.Voltage,
      Current = model.Current,
      Temperature = model.Temperature,
      CoolingFans = model.CoolingFans,
      HatsinkFans = model.HeatsinkFans
    };

  public static EorMeasurement ToModel(this EorMeasurementEntity entity) =>
    new EorMeasurement(
      Tenant: entity.Tenant,
      DeviceId: entity.Source,
      Timestamp: entity.Timestamp,
      Voltage: entity.Voltage,
      Current: entity.Current,
      Temperature: entity.Temperature,
      CoolingFans: entity.CoolingFans,
      HeatsinkFans: entity.HatsinkFans
    );
}
