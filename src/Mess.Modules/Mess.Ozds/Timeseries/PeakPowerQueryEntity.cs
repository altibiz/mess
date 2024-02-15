using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

[Keyless]
public class PeakPowerQueryEntity
{
  [Column(TypeName = "text")]
  public decimal Source { get; set; } = default!;

  [Column(TypeName = "timestamptz")]
  public DateTimeOffset Interval { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActivePower_W { get; set; } = default!;
};
