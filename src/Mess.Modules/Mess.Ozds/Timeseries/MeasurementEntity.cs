using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

[Keyless]
public class MeasurementEntity
{
  [Column(TypeName = "text")]
  public string Source { get; set; } = default!;

  [Column(TypeName = "timestamptz")]
  public DateTimeOffset Timestamp { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActiveEnergyImportTotal_Wh { get; set; } = default!;

  [Column(TypeName = "float8")]
  public decimal ActivePower_W { get; set; } = default!;
};
