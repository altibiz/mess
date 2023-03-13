// NOTE: for GetRandomUnusedPort
// using System.Net;
// using System.Net.Sockets;
using Microsoft.Playwright;

namespace Mess.Test.E2e;

internal class E2eFixture : IE2eFixture, IAsyncDisposable, IDisposable
{
  public IPage Page { get; } = default!;

  public E2eFixture()
  {
    Playwright = Microsoft.Playwright.Playwright.CreateAsync().Result;
    Browser = Playwright.Chromium.LaunchAsync().Result;
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
  }

  private IBrowser Browser { get; } = default!;

  private IPlaywright Playwright { get; } = default!;

  private string BaseUrl { get; } = $"http://localhost:5000";

  // NOTE: leaving this here if
  // https://github.com/pengweiqhca/Xunit.DependencyInjection/issues/67 gets
  // resolved
  // async Task IAsyncLifetime.InitializeAsync()
  // {
  //   Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
  //   Browser = await Playwright.Chromium.LaunchAsync();
  //   Page = await Browser.NewPageAsync();
  // }
  // Task IAsyncLifetime.DisposeAsync() =>
  //   (this as IAsyncDisposable).DisposeAsync().AsTask();

  // NOTE: might come in handy
  //
  // private static int GetRandomUnusedPort()
  // {
  //   var listener = new TcpListener(IPAddress.Any, 0);
  //   listener.Start();
  //   var port = ((IPEndPoint)listener.LocalEndpoint).Port;
  //   listener.Stop();
  //   return port;
  // }
}
