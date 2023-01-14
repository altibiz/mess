using OrchardCore.Indexing;
using Mess.Chart.Models;

namespace Mess.Chart.Indexing;

public class ChartPartIndexHandler : ContentPartIndexHandler<ChartPart>
{
  public override Task BuildIndexAsync(
    ChartPart part,
    BuildPartIndexContext context
  )
  {
    var options = context.Settings.ToOptions() | DocumentIndexOptions.Sanitize;

    // TODO: processing
    // foreach (var key in context.Keys)
    // {
    //   context.DocumentIndex.Set(key, part.Html, options);
    // }

    return Task.CompletedTask;
  }
}
