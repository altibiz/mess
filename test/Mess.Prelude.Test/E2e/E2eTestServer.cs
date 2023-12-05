using System.Diagnostics;

namespace Mess.Prelude.Test.E2e;

internal class E2eTestServer : IAsyncDisposable, IDisposable
{
  public E2eTestServer(string _)
  {
    ExecutionLock.Wait(TimeSpan.FromMilliseconds(100));

    AppCancellationTokenSource = new CancellationTokenSource();
    ServerTask = MakeServerTask(AppCancellationTokenSource.Token);
  }

  private static SemaphoreSlim ExecutionLock { get; } = new(0, 1);

  private CancellationTokenSource AppCancellationTokenSource { get; } =
    default!;

  private Task ServerTask { get; } = default!;

  async ValueTask IAsyncDisposable.DisposeAsync()
  {
    if (ServerTask is not null)
    {
      AppCancellationTokenSource.Cancel();
      await ServerTask;
    }

    ExecutionLock.Release();
  }

  void IDisposable.Dispose()
  {
    if (ServerTask is not null)
    {
      AppCancellationTokenSource.Cancel();
      // TODO: better
      ServerTask.RunSynchronously();
    }

    ExecutionLock.Release();
  }

  private static Task MakeServerTask(CancellationToken token)
  {
    var serverTask = StartTestServer("http://localhost:5000", token);
    WaitForTestServerToStart("http://localhost:5000", token).Wait(token);
    return serverTask;
  }

  private static async Task StartTestServer(
    string baseUrl,
    CancellationToken token
  )
  {
    var solutionDirectory = FindSolutionDirectory(
      new DirectoryInfo(Directory.GetCurrentDirectory())
    );
    var projectPath = Path.Combine(
      solutionDirectory.FullName,
      "src/Mess.Web/Mess.Web.csproj"
    );
    var orchardAppDataPath = Path.Combine(
      solutionDirectory.FullName,
      "App_Data/test"
    );

    var processStartInfo = new ProcessStartInfo
    {
      FileName = "dotnet",
      Arguments = $"run --project {projectPath}",
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      UseShellExecute = false,
      CreateNoWindow = false,
      WorkingDirectory = solutionDirectory.FullName
    };
    processStartInfo.Environment.Add("ASPNETCORE_ENVIRONMENT", "Development");
    processStartInfo.Environment.Add("DOTNET_ENVIRONMENT", "Development");
    processStartInfo.Environment.Add("ASPNETCORE_URLS", baseUrl);
    processStartInfo.Environment.Add("ORCHARD_APP_DATA", orchardAppDataPath);

    var process = new Process
    {
      StartInfo = processStartInfo,
      EnableRaisingEvents = true
    };

    process.Exited += (sender, args) =>
    {
      Console.WriteLine($"App exited with code: {process.ExitCode}");
    };

    process.Start();

    token.Register(process.Kill);

    try
    {
      await process.WaitForExitAsync(token);
    }
    catch (OperationCanceledException)
    {
      Console.WriteLine("The operation was cancelled.");
    }
  }

  private static async Task WaitForTestServerToStart(
    string url,
    CancellationToken? token = null
  )
  {
    token ??= CancellationToken.None;

    using var client = new HttpClient();

    var startTime = DateTimeOffset.UtcNow;
    while (DateTimeOffset.UtcNow.Subtract(startTime).TotalSeconds < 30)
    {
      if (token.Value.IsCancellationRequested) return;

      try
      {
        var response = await client.GetAsync(url);
        return;
      }
      catch
      {
        await Task.Delay(1000, token.Value);
      }
    }

    throw new TimeoutException(
      $"Server at {url} did not respond within 30 seconds!"
    );
  }

  private static DirectoryInfo FindSolutionDirectory(DirectoryInfo? directory)
  {
    while (directory != null)
    {
      if (directory.GetFiles("*.sln").Length != 0) return directory;

      directory = directory.Parent;
    }

    throw new InvalidOperationException("No solution file found.");
  }
}
