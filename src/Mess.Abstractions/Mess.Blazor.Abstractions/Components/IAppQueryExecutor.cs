namespace Mess.Blazor.Abstractions.Components;

public interface IAppQueryExecutor
{
  Task ExecuteAsync();

  void Execute() => ExecuteAsync().Wait();
}
