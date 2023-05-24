namespace Mess.Chart.Test.E2e;

public record ChartE2eTests(IE2eFixture e2eFixture)
{
  [Fact]
  public async Task CreatesChartTest()
  {
    await e2eFixture.Page.GotoAsync("/admin");
  }

  [Fact]
  public async Task OtherCreatesChartTest()
  {
    await e2eFixture.Page.GotoAsync("/admin");
  }
}
