using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.ViewModels;

public class DistributionSystemOperatorListViewModel
{
  public List<DistributionSystemOperatorItem> ContentItems { get; set; } =
    default!;
}
