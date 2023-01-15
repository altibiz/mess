using OrchardCore.Indexing;
using Mess.Chart.Models;
using Mess.System.Extensions.Object;

namespace Mess.Chart.Indexing;

public class ChartPartIndexHandler : ContentPartIndexHandler<ChartPart>
{
  public override Task BuildIndexAsync(
    ChartPart part,
    BuildPartIndexContext context
  )
  {
    var options = context.Settings.ToOptions() | DocumentIndexOptions.Sanitize;

    foreach (var key in context.Keys)
    {
      context.DocumentIndex.Set(
        key,
        part.Parameters.ToJson(pretty: false),
        options
      );
    }

    return Task.CompletedTask;
  }
}
