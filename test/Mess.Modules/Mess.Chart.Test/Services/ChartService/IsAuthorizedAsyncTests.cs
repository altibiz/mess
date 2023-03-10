using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Xunit.DependencyInjection;

namespace Mess.Chart.Test.Services.ChartService;

[Startup(typeof(Startup))]
public record IsAuthorizedAsyncTests(
  Mess.Chart.Services.ChartService chartService,
  Mock<IAuthorizationService> authorizationService
)
{
  [Fact]
  public async Task ReturnsFalseWhenAuthorizationFails()
  {
    SetupAuthorizationService(false);

    var result = await chartService.IsAuthorizedAsync(new ClaimsPrincipal());

    Assert.False(result);
  }

  [Fact]
  public async Task ReturnsTrueWhenAuthorizationSucceeds()
  {
    SetupAuthorizationService(true);

    var result = await chartService.IsAuthorizedAsync(new ClaimsPrincipal());

    Assert.True(result);
  }

  private void SetupAuthorizationService(bool result)
  {
    var authorizationResult = result
      ? AuthorizationResult.Success()
      : AuthorizationResult.Failed();

    authorizationService
      .Setup(
        x =>
          x.AuthorizeAsync(
            It.IsAny<ClaimsPrincipal>(),
            It.IsAny<object>(),
            It.IsAny<string>()
          )
      )
      .ReturnsAsync(authorizationResult);

    authorizationService
      .Setup(
        x =>
          x.AuthorizeAsync(
            It.IsAny<ClaimsPrincipal>(),
            It.IsAny<object>(),
            It.IsAny<IEnumerable<IAuthorizationRequirement>>()
          )
      )
      .ReturnsAsync(authorizationResult);
  }
}
