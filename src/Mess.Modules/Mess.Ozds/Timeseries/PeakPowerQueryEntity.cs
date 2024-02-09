using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

[Keyless]
public class PeakPowerQueryEntity
{
  public decimal ActivePower_W { get; set; }
};
