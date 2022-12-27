using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Entities;

public class Measurement : HypertableEntity
{
  [Column(TypeName = "float")]
  public float Power { get; set; } = default!;
}
