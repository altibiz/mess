namespace Mess.System.Test.E2e;

public record class E2eTests(IE2eFixture E2eFixture)
{
  [Fact]
  public async Task AdminTest()
  {
    await E2eFixture.Page.GotoAsync("/admin");
  }
}
