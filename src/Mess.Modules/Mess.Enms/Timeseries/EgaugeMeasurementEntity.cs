using System.ComponentModel.DataAnnotations.Schema;
using Mess.Enms.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Enms.Timeseries;

public class EgaugeMeasurementEntity : HypertableEntity
{
  [Column(TypeName = "float4")] public float Power { get; set; }

  [Column(TypeName = "float4")] public float Voltage { get; set; }
}

public static class EgaugeMeasurementEntityExtensions
{
  public static EgaugeMeasurementEntity ToEntity(
    this EgaugeMeasurement model
  )
  {
    return new EgaugeMeasurementEntity
    {
      Tenant = model.Tenant,
      Timestamp = model.Timestamp,
      Source = model.DeviceId,
      Voltage = model.Voltage,
      Power = model.Power
    };
  }

  public static EgaugeMeasurement ToModel(
    this EgaugeMeasurementEntity entity
  )
  {
    return new EgaugeMeasurement(
      entity.Tenant,
      entity.Source,
      entity.Timestamp,
      Power: entity.Power,
      Voltage: entity.Voltage
    );
  }
}
