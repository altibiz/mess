using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Entities;

public class Measurement : MeasurementEntity
{
  [Column(TypeName = "float")]
  public float Power { get; set; } = default!;

  [Column(TypeName = "float")]
  public float Voltage { get; set; } = default!;
}
