using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Entities;

namespace Mess.Timeseries.Entities;

public class EgaugeMeasurementEntity : HypertableEntity
{
  [Column(TypeName = "float")]
  public float Power { get; set; } = default!;

  [Column(TypeName = "float")]
  public float Voltage { get; set; } = default!;
}
