using YesSql.Indexes;

namespace Mess.Ozds.Abstractions.Indexes;

public class OperatorCatalogueIndex : MapIndex
{
  public string Voltage { get; set; } = default!;

  public string Model { get; set; } = default!;
}
