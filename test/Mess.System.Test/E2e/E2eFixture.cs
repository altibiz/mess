using Microsoft.Playwright;

namespace Mess.System.Test.E2e;

internal class E2eFixture : IE2eFixture, IAsyncDisposable, IDisposable
{
  public IPage Page { get; } = default!;

  public E2eFixture(string baseUrl, E2eTestServer testServer)
  {
    TestServer = testServer;
    BaseUrl = baseUrl;

    ExecutionLock.Wait(TimeSpan.FromMilliseconds(100));

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

    ExecutionLock.Release();
  }

  private static SemaphoreSlim ExecutionLock { get; } = new(0, 1);

  private static bool IsCi => Environment.GetEnvironmentVariable("CI") != null;

  private IBrowser Browser { get; } = default!;

  private IPlaywright Playwright { get; } = default!;

  private string BaseUrl { get; } = default!;

  private E2eTestServer TestServer { get; } = default!;
}