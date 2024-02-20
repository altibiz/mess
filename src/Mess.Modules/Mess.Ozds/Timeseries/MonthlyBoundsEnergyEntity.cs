using System.ComponentModel.DataAnnotations.Schema;
using Mess.Timeseries.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mess.Ozds.Timeseries;

public class MonthlyBoundsEnergyEntity : HypertableEntity
{
  [Column(TypeName = "float8")]
  public decimal ActiveEnergyImportTotal_Wh { get; set; } = default!;
};
