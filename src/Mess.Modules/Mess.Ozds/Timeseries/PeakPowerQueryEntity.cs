using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

[Keyless]
public class PeakPowerQueryEntity
{
  [Column(TypeName = "float8")]
  public decimal ActivePower_W { get; set; }
};
