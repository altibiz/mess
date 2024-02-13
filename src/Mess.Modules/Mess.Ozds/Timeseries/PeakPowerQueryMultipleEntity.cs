using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

[Keyless]
public class PeakPowerQueryMultipleEntity
{
  [Column(TypeName = "text")]
  public string Source { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActivePower_W { get; set; }
};
