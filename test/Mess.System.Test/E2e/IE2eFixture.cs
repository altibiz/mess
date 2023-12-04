using Microsoft.Playwright;

namespace Mess.Prelude.Test.E2e;

public interface IE2eFixture
{
  public IPage Page { get; }
}
