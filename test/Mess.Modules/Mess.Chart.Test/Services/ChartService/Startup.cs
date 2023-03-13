using Mess.Chart.Abstractions.Providers;
using Microsoft.AspNetCore.Authorization;
using Moq;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;

namespace Mess.Chart.Test.Services.ChartService;

public class Startup : Test.Startup
{
  public const string TestDataProviderId = "TestDataProviderId";

  public override void ConfigureServices(
    IServiceCollection services,
    HostBuilderContext hostBuilderContext
  )
  {
    base.ConfigureServices(services, hostBuilderContext);

    var authorizationService = new Mock<IAuthorizationService>();
    var contentManager = new Mock<IContentManager>();
    var contentDefinitionManager = new Mock<IContentDefinitionManager>();
    var chartDataProviderLookup = new Mock<IChartDataProviderLookup>();

    chartDataProviderLookup
      .Setup(
        chartDataProviderLookup =>
          chartDataProviderLookup.Exists(TestDataProviderId)
      )
      .Returns(true);

    services.AddScoped<IAuthorizationService>(
      services => authorizationService.Object
    );
    services.AddScoped<IContentManager>(services => contentManager.Object);
    services.AddScoped<IContentDefinitionManager>(
      services => contentDefinitionManager.Object
    );
    services.AddScoped<IChartDataProviderLookup>(
      services => chartDataProviderLookup.Object
    );

    services.AddScoped<Mock<IAuthorizationService>>(
      services => authorizationService
    );
    services.AddScoped<Mock<IContentManager>>(services => contentManager);
    services.AddScoped<Mock<IContentDefinitionManager>>(
      services => contentDefinitionManager
    );
    services.AddScoped<Mock<IChartDataProviderLookup>>(
      services => chartDataProviderLookup
    );

    services.AddScoped<Mess.Chart.Services.ChartService>();
  }
}
