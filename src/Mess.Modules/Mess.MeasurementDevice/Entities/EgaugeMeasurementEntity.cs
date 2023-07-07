using System.ComponentModel.DataAnnotations.Schema;
using Mess.MeasurementDevice.Abstractions.Client;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.MeasurementDevice.Entities;

public class EgaugeMeasurementEntity : HypertableEntity
{
  [Column(TypeName = "float4")]
  public float Power { get; set; } = default!;

  [Column(TypeName = "float4")]
  public float Voltage { get; set; } = default!;
}

public static class EgaugeMeasurementEntityExtensions
{
  public static EgaugeMeasurementEntity ToEntity(
    this EgaugeMeasurement model
  ) =>
    new EgaugeMeasurementEntity
    {
      Tenant = model.Tenant,
      Timestamp = model.Timestamp,
      Source = model.DeviceId,
      Voltage = model.Voltage,
      Power = model.Power
    };

  public static EgaugeMeasurement ToModel(
    this EgaugeMeasurementEntity entity
  ) =>
    new EgaugeMeasurement(
      Tenant: entity.Tenant,
      DeviceId: entity.Source,
      Timestamp: entity.Timestamp,
      Power: entity.Power,
      Voltage: entity.Voltage
    );
}
