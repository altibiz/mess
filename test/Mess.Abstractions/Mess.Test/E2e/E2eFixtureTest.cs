using Xunit;

namespace Mess.Test.E2e;

public record class E2eFixtureTest(IE2eFixture E2eFixture)
{
  [Fact]
  public async Task HomeTest()
  {
    await E2eFixture.Page.GotoAsync("/admin");
  }
}
