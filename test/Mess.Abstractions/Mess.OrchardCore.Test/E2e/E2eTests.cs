namespace Mess.OrchardCore.Test.E2e;

public record E2eTests(IE2eFixture e2eFixture)
{
  [Fact]
  public async Task HomeTest()
  {
    await e2eFixture.Page.GotoAsync("/admin");
  }
}
