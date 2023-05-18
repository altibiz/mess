namespace Mess.Test.Tenants;

public record class TenantFixtureTest(ITenantFixture TenantFixture)
{
  [Fact]
  public void CurrentTenantTest()
  {
    Assert.Equal(
      "mess.test.tenants.tenant-fixture-test.current-tenant Test",
      TenantFixture.Tenants.Current.Name
    );

    Assert.Equal(
      "mess.test.tenants.tenant-fixture-test.current-tenant-test",
      TenantFixture.Tenants.Current.TablePrefix
    );
  }

  [Fact]
  public void OtherTenantTest()
  {
    Assert.Equal(
      "mess.test.tenants.tenant-fixture-test.other-tenant Test",
      TenantFixture.Tenants.Current.Name
    );

    Assert.Equal(
      "mess.test.tenants.tenant-fixture-test.other-tenant-test",
      TenantFixture.Tenants.Current.TablePrefix
    );
  }
}
