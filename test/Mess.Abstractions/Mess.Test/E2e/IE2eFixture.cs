using Microsoft.Playwright;

namespace Mess.Test.E2e;

public interface IE2eFixture
{
  public IPage Page { get; }
}
