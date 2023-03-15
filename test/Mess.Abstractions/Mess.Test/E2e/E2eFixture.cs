using Microsoft.Playwright;

namespace Mess.Test.E2e;

internal class E2eFixture : IE2eFixture, IAsyncDisposable, IDisposable
{
  public IPage Page { get; } = default!;

  public E2eFixture(string baseUrl, Func<CancellationToken, Task> makeAppTask)
  {
    BaseUrl = baseUrl;

    ExecutionLock.Wait(TimeSpan.FromMilliseconds(100));

    AppCancellationTokenSource = new();
    AppTask = makeAppTask(AppCancellationTokenSource.Token);

    Playwright = Microsoft.Playwright.Playwright.CreateAsync().Result;
    Browser = Playwright.Chromium
      .LaunchAsync(new BrowserTypeLaunchOptions { Headless = IsCi, })
      .Result;
    Page = Browser
      .NewPageAsync(new BrowserNewPageOptions { BaseURL = BaseUrl })
      .Result;
  }

  async ValueTask IAsyncDisposable.DisposeAsync()
  {
    if (Browser is not null)
    {
      await Browser.DisposeAsync();
    }

    if (Playwright is not null)
    {
      Playwright.Dispose();
    }

    if (AppTask is not null)
    {
      AppCancellationTokenSource.Cancel();
      await AppTask;
    }

    ExecutionLock.Release();
  }

  void IDisposable.Dispose()
  {
    if (Browser is not null)
    {
      // TODO: better
      Browser.DisposeAsync().AsTask().RunSynchronously();
    }

    if (Playwright is not null)
    {
      Playwright.Dispose();
    }

    if (AppTask is not null)
    {
      AppCancellationTokenSource.Cancel();
      // TODO: better
      AppTask.RunSynchronously();
    }

    ExecutionLock.Release();
  }

  // NOTE: https://playwright.dev/dotnet/docs/test-runners#xunit-support
  private static SemaphoreSlim ExecutionLock { get; } = new(0, 1);

  private static bool IsCi => Environment.GetEnvironmentVariable("CI") != null;

  private CancellationTokenSource AppCancellationTokenSource { get; } =
    default!;

  private Task AppTask { get; } = default!;

  private IBrowser Browser { get; } = default!;

  private IPlaywright Playwright { get; } = default!;

  private string BaseUrl { get; } = default!;
}
