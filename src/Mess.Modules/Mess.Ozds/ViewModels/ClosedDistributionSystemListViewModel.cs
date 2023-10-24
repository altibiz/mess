using Mess.Ozds.Abstractions.Models;

namespace Mess.Ozds.ViewModels;

public class ClosedDistributionSystemListViewModel
{
  public List<ClosedDistributionSystemItem> ContentItems { get; set; } =
    default!;
}
