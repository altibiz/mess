using System.ComponentModel.DataAnnotations.Schema;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Eor.Iot;

public class EorMeasurementEntity : HypertableEntity
{
  [Column(TypeName = "float4")] public float Voltage { get; set; }

  [Column(TypeName = "float4")] public float Current { get; set; }

  [Column(TypeName = "float4")] public float Temperature { get; set; }

  public bool CoolingFans { get; set; }

  public bool HatsinkFans { get; set; }
}

public static class EorMeasurementEntityExtensions
{
  public static EorMeasurementEntity ToEntity(this EorMeasurement model, string tenant)
  {
    return new EorMeasurementEntity
    {
      Tenant = tenant,
      Timestamp = model.Timestamp,
      Source = model.DeviceId,
      Voltage = model.Voltage,
      Current = model.Current,
      Temperature = model.Temperature,
      CoolingFans = model.CoolingFans,
      HatsinkFans = model.HeatsinkFans
    };
  }

  public static EorMeasurement ToModel(this EorMeasurementEntity entity)
  {
    return new EorMeasurement(
      entity.Source,
      entity.Timestamp,
      entity.Voltage,
      entity.Current,
      entity.Temperature,
      entity.CoolingFans,
      entity.HatsinkFans
    );
  }
}
