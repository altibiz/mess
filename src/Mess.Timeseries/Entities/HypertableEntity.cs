using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Client;

namespace Mess.Timeseries.Entities;

public abstract class HypertableEntity
{
  [HypertableColumn]
  [Column(TypeName = "timestamp")]
  public DateTime Timestamp { get; set; } = default!;

  [Column(TypeName = "string")]
  public string Id { get; set; } = default!;

  [Column(TypeName = "string")]
  public string Tenant { get; set; } = default!;
}
