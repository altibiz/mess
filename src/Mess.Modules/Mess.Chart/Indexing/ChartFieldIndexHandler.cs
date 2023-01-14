using OrchardCore.Indexing;
using Mess.Chart.Fields;

namespace Mess.Chart.Indexing;

public class ChartFieldIndexHandler : ContentFieldIndexHandler<ChartField>
{
  public override Task BuildIndexAsync(
    ChartField field,
    BuildFieldIndexContext context
  )
  {
    var options = context.Settings.ToOptions();

    // TODO: processing
    // foreach (var key in context.Keys)
    // {
    //   context.DocumentIndex.Set(key, field.Html, options);
    // }

    return Task.CompletedTask;
  }
}
