using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

public class IntervalAveragePowerQuery : HypertableEntity
{
  [Column(TypeName = "float8")]
  public decimal ActivePower_W { get; set; } = default!;
};
