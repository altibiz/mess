using OrchardCore.ContentManagement;
using YesSql;

namespace Mess.Blazor.Components;

public class AppQueryExecutor
{
  private readonly ISession _session;

  public AppQueryExecutor(
    IServiceProvider serviceProvider,
    ISession session
  )
  {

  }
}
