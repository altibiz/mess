using System.ComponentModel.DataAnnotations.Schema;
using Mess.MeasurementDevice.Abstractions.Dispatchers;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Timeseries.Entities;

public class EgaugeMeasurementEntity : HypertableEntity
{
  [Column(TypeName = "float")]
  public float Power { get; set; } = default!;

  [Column(TypeName = "float")]
  public float Voltage { get; set; } = default!;
}

public static class EgaugeMeasurementEntityExtensions
{
  public static EgaugeMeasurementEntity ToEntity(
    this DispatchedEgaugeMeasurement model
  ) =>
    new EgaugeMeasurementEntity
    {
      Tenant = model.Tenant,
      Timestamp = model.Timestamp,
      Source = model.Source,
      Voltage = model.Voltage,
      Power = model.Power
    };

  public static DispatchedEgaugeMeasurement ToModel(
    this EgaugeMeasurementEntity entity
  ) =>
    new DispatchedEgaugeMeasurement(
      Tenant: entity.Tenant,
      Source: entity.Source,
      Timestamp: entity.Timestamp,
      Power: entity.Power,
      Voltage: entity.Voltage
    );
}
