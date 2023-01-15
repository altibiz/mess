using OrchardCore.Indexing;
using Mess.Chart.Fields;
using Mess.System.Extensions.Object;

namespace Mess.Chart.Indexing;

public class ChartFieldIndexHandler : ContentFieldIndexHandler<ChartField>
{
  public override Task BuildIndexAsync(
    ChartField field,
    BuildFieldIndexContext context
  )
  {
    var options = context.Settings.ToOptions();

    foreach (var key in context.Keys)
    {
      context.DocumentIndex.Set(
        key,
        field.Parameters.ToJson(pretty: false),
        options
      );
    }

    return Task.CompletedTask;
  }
}
