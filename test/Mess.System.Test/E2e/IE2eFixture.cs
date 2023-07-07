using Microsoft.Playwright;

namespace Mess.System.Test.E2e;

public interface IE2eFixture
{
  public IPage Page { get; }
}
