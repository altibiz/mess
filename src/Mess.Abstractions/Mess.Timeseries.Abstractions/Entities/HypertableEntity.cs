using System.ComponentModel.DataAnnotations.Schema;

namespace Mess.Timeseries.Abstractions.Entities;

public abstract class HypertableEntity
{
  [Column(TypeName = "text")]
  public string Tenant { get; set; } = default!;

  [Column(TypeName = "text")]
  public string Source { get; set; } = default!;

  [HypertableColumn]
  [Column(TypeName = "timestamptz")]
  public DateTime Timestamp
  {
    get => _timestamp.ToUniversalTime();
    set => _timestamp = value.ToUniversalTime();
  }

  [NotMapped]
  private DateTime _timestamp = default!;
}
