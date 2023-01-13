using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Client;

namespace Mess.Timeseries.Entities;

public abstract class HypertableEntity
{
  [HypertableColumn]
  [Column(TypeName = "timestamp")]
  public DateTime Timestamp { get; set; } = default!;

  [Column(TypeName = "varchar")]
  public string Id { get; set; } = default!;

  [Column(TypeName = "varchar")]
  public string Tenant { get; set; } = default!;
}
