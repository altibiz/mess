using System.ComponentModel.DataAnnotations;

namespace Mess.EventStore.Models;

public class EgaugeModel
{
  [Required]
  public string Id { get; set; } = default!;
}
