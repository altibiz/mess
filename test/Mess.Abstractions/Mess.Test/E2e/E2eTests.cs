namespace Mess.Test.E2e;

public record class E2eTests(IE2eFixture e2eFixture)
{
  [Fact]
  public async Task AdminTest()
  {
    await e2eFixture.Page.GotoAsync("/admin");
  }
}
