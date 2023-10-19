using Mess.System.Extensions.Microsoft;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using OrchardCore.Users.Services;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using Mess.OrchardCore;
using Mess.Ozds.Abstractions.Models;
using Mess.OrchardCore.Extensions.OrchardCore;
using Mess.Fields.Abstractions.ApiKeys;
using Mess.Fields.Abstractions.Extensions;
using Mess.Chart.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Fields.Abstractions;
using Mess.Population.Abstractions;

namespace Mess.Ozds;

public class Populations : IPopulation
{
  public async Task PopulateAsync()
  {
    var regulatoryAgencyCatalogueContentItemId =
      await _serviceProvider.AwaitScopeAsync(
        CreateAsyncMigrations.PopulateRegulatoryAgencyCatalogue
      );

    (
      string whiteHighVoltageOperatorCatalogueContentItemId,
      string whiteMediumVoltageOperatorCatalogueContentItemId,
      string blueOperatorCatalogueContentItemId,
      string whiteLowVoltageOperatorCatalogueContentItemId,
      string redOperatorCatalogueContentItemId,
      string yellowOperatorCatalogueContentItemId
    ) = await _serviceProvider.AwaitScopeAsync(
      CreateAsyncMigrations.PopulateOperatorCatalogues
    );

    (
      string whiteHighVoltageSystemCatalogueContentItemId,
      string whiteMediumVoltageSystemCatalogueContentItemId,
      string blueSystemCatalogueContentItemId,
      string whiteLowVoltageSystemCatalogueContentItemId,
      string redSystemCatalogueContentItemId,
      string yellowSystemCatalogueContentItemId
    ) = await _serviceProvider.AwaitScopeAsync(
      CreateAsyncMigrations.PopulateOperatorCatalogues
    );

    (string? operatorUserId, string? operatorContentItemId) =
      await _serviceProvider.AwaitScopeAsync(
        async serviceProvider =>
          await CreateAsyncMigrations.PopulateOperator(
            serviceProvider,
            regulatoryAgencyCatalogueContentItemId,
            whiteHighVoltageOperatorCatalogueContentItemId!,
            whiteMediumVoltageOperatorCatalogueContentItemId!,
            blueOperatorCatalogueContentItemId!,
            whiteLowVoltageOperatorCatalogueContentItemId!,
            redOperatorCatalogueContentItemId!,
            yellowOperatorCatalogueContentItemId!
          )
      );

    (string? systemUserId, string? systemContentItemId) =
      await _serviceProvider.AwaitScopeAsync(
        async serviceProvider =>
          await CreateAsyncMigrations.PopulateSystem(
            serviceProvider,
            operatorUserId!,
            operatorContentItemId!,
            whiteHighVoltageSystemCatalogueContentItemId!,
            whiteMediumVoltageSystemCatalogueContentItemId!,
            blueSystemCatalogueContentItemId!,
            whiteLowVoltageSystemCatalogueContentItemId!,
            redSystemCatalogueContentItemId!,
            yellowSystemCatalogueContentItemId!
          )
      );

    (string? unitUserId, string? unitContentItemId) =
      await _serviceProvider.AwaitScopeAsync(
        async serviceProvider =>
          await CreateAsyncMigrations.PopulateUnit(
            serviceProvider,
            operatorUserId!,
            operatorContentItemId!,
            systemUserId!,
            systemContentItemId!
          )
      );

    await _serviceProvider.AwaitScopeAsync(
      async serviceProvider =>
        await CreateAsyncMigrations.PopulatePidgeon(
          serviceProvider,
          unitContentItemId!
        )
    );

    await _serviceProvider.AwaitScopeAsync(
      async serviceProvider =>
        await CreateAsyncMigrations.PopulateAbb(
          serviceProvider,
          unitContentItemId!,
          whiteLowVoltageOperatorCatalogueContentItemId!,
          whiteLowVoltageSystemCatalogueContentItemId!
        )
    );
  }

  public Populations(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}

internal static partial class CreateAsyncMigrations
{
  internal static async Task<(
    string? UserId,
    string? ContentItemId
  )> PopulateOperator(
    IServiceProvider serviceProvider,
    string regulatoryAgencyCatalogueContentItemId,
    string whiteHighVoltageOperatorCatalogueContentItemId,
    string whiteMediumVoltageOperatorCatalogueContentItemId,
    string blueOperatorCatalogueContentItemId,
    string whiteLowVoltageOperatorCatalogueContentItemId,
    string redOperatorCatalogueContentItemId,
    string yellowOperatorCatalogueContentItemId
  )
  {
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var hostEnvironment =
      serviceProvider.GetRequiredService<IHostEnvironment>();
    var userService = serviceProvider.GetRequiredService<IUserService>();

    await roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "DistributionSystemOperatorRepresentative",
        RoleName = "Distribution System Operator Representative",
        RoleDescription =
          "Representative of closed distribution systems operator.",
        RoleClaims = new()
        {
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AccessAdminPanel"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "View Users"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue =
              "ManageUsersInRole_Closed distribution system representative"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_Closed distribution system representative"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "Listing payments"
          },
        }
      }
    );

    if (hostEnvironment.IsDevelopment())
    {
      var operatorUser = await userService.CreateDevUserAsync(
        id: "OperatorId",
        userName: "Operator",
        roleNames: new[]
        {
          "DistributionSystemOperatorRepresentative",
          "LegalEntityRepresentative",
        }
      );

      var distributionSystemOperator =
        await contentManager.NewContentAsync<DistributionSystemOperatorItem>();
      distributionSystemOperator.Alter(
        distributionSystemOperator => distributionSystemOperator.TitlePart,
        titlePart =>
        {
          titlePart.Title = "Operator";
        }
      );
      distributionSystemOperator.Alter(
        distributionSystemOperator =>
          distributionSystemOperator.LegalEntityPart,
        legalEntityPart =>
        {
          legalEntityPart.Name = new() { Text = "Operator" };
          legalEntityPart.City = new() { Text = "City" };
          legalEntityPart.Address = new() { Text = "Address" };
          legalEntityPart.PostalCode = new() { Text = "Postal code" };
          legalEntityPart.Email = new() { Text = "Email" };
          legalEntityPart.SocialSecurityNumber = new()
          {
            Text = "Social security number"
          };
          legalEntityPart.Representatives = new()
          {
            UserIds = new[] { operatorUser.UserId }
          };
        }
      );
      distributionSystemOperator.Alter(
        distributionSystemOperator =>
          distributionSystemOperator.DistributionSystemOperatorPart,
        distributionSystemOperatorPart =>
        {
          distributionSystemOperatorPart.RegulatoryCatalogue = new()
          {
            ContentItemIds = new[] { regulatoryAgencyCatalogueContentItemId }
          };

          distributionSystemOperatorPart.WhiteHighVoltageOperatorCatalogue =
            new()
            {
              ContentItemIds = new[]
              {
                whiteHighVoltageOperatorCatalogueContentItemId
              }
            };

          distributionSystemOperatorPart.WhiteMediumVoltageOperatorCatalogue =
            new()
            {
              ContentItemIds = new[]
              {
                whiteMediumVoltageOperatorCatalogueContentItemId
              }
            };

          distributionSystemOperatorPart.BlueOperatorCatalogue = new()
          {
            ContentItemIds = new[] { blueOperatorCatalogueContentItemId }
          };

          distributionSystemOperatorPart.WhiteLowVoltageOperatorCatalogue =
            new()
            {
              ContentItemIds = new[]
              {
                whiteLowVoltageOperatorCatalogueContentItemId
              }
            };

          distributionSystemOperatorPart.RedOperatorCatalogue = new()
          {
            ContentItemIds = new[] { redOperatorCatalogueContentItemId }
          };

          distributionSystemOperatorPart.YellowOperatorCatalogue = new()
          {
            ContentItemIds = new[] { yellowOperatorCatalogueContentItemId }
          };
        }
      );

      await contentManager.CreateAsync(
        distributionSystemOperator,
        VersionOptions.Latest
      );

      return (operatorUser.UserId, distributionSystemOperator.ContentItemId);
    }

    return (null, null);
  }

  internal static async Task<(
    string? UserId,
    string? ContentItemId
  )> PopulateSystem(
    IServiceProvider serviceProvider,
    string operatorUserId,
    string operatorContentItemId,
    string whiteHighVoltageOperatorCatalogueContentItemId,
    string whiteMediumVoltageOperatorCatalogueContentItemId,
    string blueOperatorCatalogueContentItemId,
    string whiteLowVoltageOperatorCatalogueContentItemId,
    string redOperatorCatalogueContentItemId,
    string yellowOperatorCatalogueContentItemId
  )
  {
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();
    var hostEnvironment =
      serviceProvider.GetRequiredService<IHostEnvironment>();
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();

    await roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "ClosedDistributionSystemRepresentative",
        RoleName = "Closed Distribution System Representative",
        RoleDescription = "Representative of a closed distribution system.",
        RoleClaims = new()
        {
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AccessAdminPanel"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "View Users"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue =
              "ManageUsersInRole_Distribution system unit representative"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_Distribution system unit representative"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "Listing own payments"
          },
        }
      }
    );

    if (hostEnvironment.IsDevelopment())
    {
      var systemUser = await userService.CreateDevUserAsync(
        id: "SystemId",
        userName: "System",
        roleNames: new[]
        {
          "ClosedDistributionSystemRepresentative",
          "LegalEntityRepresentative",
        }
      );

      var closedDistributionSystem =
        await contentManager.NewContentAsync<ClosedDistributionSystemItem>();
      closedDistributionSystem.Alter(
        closedDistributionSystem => closedDistributionSystem.TitlePart,
        titlePart =>
        {
          titlePart.Title = "System";
        }
      );
      closedDistributionSystem.Alter(
        closedDistributionSystem => closedDistributionSystem.LegalEntityPart,
        legalEntityPart =>
        {
          legalEntityPart.Name = new() { Text = "System" };
          legalEntityPart.City = new() { Text = "City" };
          legalEntityPart.Address = new() { Text = "Address" };
          legalEntityPart.PostalCode = new() { Text = "Postal code" };
          legalEntityPart.Email = new() { Text = "Email" };
          legalEntityPart.SocialSecurityNumber = new()
          {
            Text = "Social security number"
          };
          legalEntityPart.Representatives = new()
          {
            UserIds = new[] { systemUser.UserId }
          };
        }
      );
      closedDistributionSystem.Alter(
        closedDistributionSystem =>
          closedDistributionSystem.ClosedDistributionSystemPart,
        closedDistributionSystemPart =>
        {
          closedDistributionSystemPart.DistributionSystemOperator = new()
          {
            ContentItemIds = new[] { operatorContentItemId }
          };

          closedDistributionSystemPart.WhiteHighVoltageOperatorCatalogue = new()
          {
            ContentItemIds = new[]
            {
              whiteHighVoltageOperatorCatalogueContentItemId
            }
          };

          closedDistributionSystemPart.WhiteMediumVoltageOperatorCatalogue =
            new()
            {
              ContentItemIds = new[]
              {
                whiteMediumVoltageOperatorCatalogueContentItemId
              }
            };

          closedDistributionSystemPart.BlueOperatorCatalogue = new()
          {
            ContentItemIds = new[] { blueOperatorCatalogueContentItemId }
          };

          closedDistributionSystemPart.WhiteLowVoltageOperatorCatalogue = new()
          {
            ContentItemIds = new[]
            {
              whiteLowVoltageOperatorCatalogueContentItemId
            }
          };

          closedDistributionSystemPart.RedOperatorCatalogue = new()
          {
            ContentItemIds = new[] { redOperatorCatalogueContentItemId }
          };

          closedDistributionSystemPart.YellowOperatorCatalogue = new()
          {
            ContentItemIds = new[] { yellowOperatorCatalogueContentItemId }
          };
        }
      );

      await contentManager.CreateAsync(
        closedDistributionSystem,
        VersionOptions.Latest
      );

      return (systemUser.UserId, closedDistributionSystem.ContentItemId);
    }

    return (null, null);
  }

  internal static async Task<(
    string? UserId,
    string? ContentItemId
  )> PopulateUnit(
    IServiceProvider serviceProvider,
    string operatorUserId,
    string operatorContentItemId,
    string systemUserId,
    string systemContentItemId
  )
  {
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();
    var hostEnvironment =
      serviceProvider.GetRequiredService<IHostEnvironment>();
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();

    await roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "DistributionSystemUnitRepresentative",
        RoleName = "Distribution System Unit Representative",
        RoleDescription = "Representative of a distribution system unit.",
        RoleClaims = new()
        {
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "Listing own payments"
          },
        }
      }
    );

    if (hostEnvironment.IsDevelopment())
    {
      var unitUser = await userService.CreateDevUserAsync(
        id: "UnitId",
        userName: "Unit",
        roleNames: new[]
        {
          "DistributionSystemUnitRepresentative",
          "LegalEntityRepresentative",
        }
      );

      var distributionSystemUnit =
        await contentManager.NewContentAsync<DistributionSystemUnitItem>();
      distributionSystemUnit.Alter(
        distributionSystemUnit => distributionSystemUnit.TitlePart,
        titlePart =>
        {
          titlePart.Title = "Unit";
        }
      );
      distributionSystemUnit.Alter(
        distributionSystemUnit => distributionSystemUnit.LegalEntityPart,
        legalEntityPart =>
        {
          legalEntityPart.Name = new() { Text = "Unit" };
          legalEntityPart.City = new() { Text = "City" };
          legalEntityPart.Address = new() { Text = "Address" };
          legalEntityPart.PostalCode = new() { Text = "Postal code" };
          legalEntityPart.Email = new() { Text = "Email" };
          legalEntityPart.SocialSecurityNumber = new()
          {
            Text = "Social security number"
          };
          legalEntityPart.Representatives = new()
          {
            UserIds = new[] { unitUser.UserId }
          };
        }
      );
      distributionSystemUnit.Alter(
        distributionSystemUnit =>
          distributionSystemUnit.DistributionSystemUnitPart,
        distributionSystemUnitPart =>
        {
          distributionSystemUnitPart.ClosedDistributionSystem = new()
          {
            ContentItemIds = new[] { systemContentItemId }
          };
        }
      );

      await contentManager.CreateAsync(
        distributionSystemUnit,
        VersionOptions.Latest
      );

      return (unitUser.UserId, distributionSystemUnit.ContentItemId);
    }

    return (null, null);
  }

  internal static async Task PopulatePidgeon(
    IServiceProvider serviceProvider,
    string unitContentItemId
  )
  {
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();
    var hostEnvironment =
      serviceProvider.GetRequiredService<IHostEnvironment>();
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var apiKeyFieldService =
      serviceProvider.GetRequiredService<IApiKeyFieldService>();

    if (hostEnvironment.IsDevelopment())
    {
      var pidgeonIotDevice =
        await contentManager.NewContentAsync<PidgeonIotDeviceItem>();
      pidgeonIotDevice.Alter(
        pidgeonIotDevice => pidgeonIotDevice.TitlePart,
        titlePart =>
        {
          titlePart.Title = "pidgeon";
        }
      );
      pidgeonIotDevice.Alter(
        pidgeonIotDevice => pidgeonIotDevice.IotDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "pidgeon" };
        }
      );
      pidgeonIotDevice.Alter(
        pidgeonIotDevice => pidgeonIotDevice.PidgeonIotDevicePart,
        pidgeonIotDevicePart =>
        {
          pidgeonIotDevicePart.ApiKey = apiKeyFieldService.HashApiKeyField(
            "pidgeon"
          );
        }
      );
      pidgeonIotDevice.Alter(
        pidgeonIotDevice => pidgeonIotDevice.OzdsIotDevicePart,
        ozdsIotDevicePart =>
        {
          ozdsIotDevicePart.DistributionSystemUnit = new()
          {
            ContentItemIds = new[] { unitContentItemId }
          };
        }
      );
      await contentManager.CreateAsync(pidgeonIotDevice, VersionOptions.Latest);
    }
  }

  internal static async Task PopulateAbb(
    IServiceProvider serviceProvider,
    string unitContentItemId,
    string usageCatalogueContentItemId,
    string supplyCatalogueContentItemId
  )
  {
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();
    var hostEnvironment =
      serviceProvider.GetRequiredService<IHostEnvironment>();
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var apiKeyFieldService =
      serviceProvider.GetRequiredService<IApiKeyFieldService>();

    if (hostEnvironment.IsDevelopment())
    {
      var abbPowerDataset =
        await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
      abbPowerDataset.Alter(
        eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
        timeseriesChartDatasetPart =>
        {
          timeseriesChartDatasetPart.Color = new() { Value = "#ff0000" };
          timeseriesChartDatasetPart.Label = new() { Text = "Power" };
          timeseriesChartDatasetPart.Property = nameof(AbbMeasurement.Power);
        }
      );
      var abbChart =
        await contentManager.NewContentAsync<TimeseriesChartItem>();
      abbChart.Alter(
        abbChart => abbChart.TitlePart,
        titlePart =>
        {
          titlePart.Title = "abb";
        }
      );
      abbChart.Alter(
        abbChart => abbChart.TimeseriesChartPart,
        timeseriesChartPart =>
        {
          timeseriesChartPart.ChartContentType = "AbbIotDevice";
          timeseriesChartPart.History = new()
          {
            Value = new(Unit: IntervalUnit.Minute, Count: 10)
          };
          timeseriesChartPart.RefreshInterval = new()
          {
            Value = new(Unit: IntervalUnit.Second, Count: 10)
          };
          timeseriesChartPart.Datasets = new() { abbPowerDataset };
        }
      );
      await contentManager.CreateAsync(abbChart, VersionOptions.Latest);

      var abbIotDevice =
        await contentManager.NewContentAsync<AbbIotDeviceItem>();
      abbIotDevice.Alter(
        abbIotDevice => abbIotDevice.TitlePart,
        titlePart =>
        {
          titlePart.Title = "Abb";
        }
      );
      abbIotDevice.Alter(
        abbIotDevice => abbIotDevice.IotDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "abb" };
        }
      );
      abbIotDevice.Alter(
        abbIotDevice => abbIotDevice.OzdsIotDevicePart,
        ozdsIotDevicePart =>
        {
          ozdsIotDevicePart.DistributionSystemUnit = new()
          {
            ContentItemIds = new[] { unitContentItemId }
          };
          ozdsIotDevicePart.UsageCatalogue = new()
          {
            ContentItemIds = new[] { usageCatalogueContentItemId }
          };
          ozdsIotDevicePart.SupplyCatalogue = new()
          {
            ContentItemIds = new[] { supplyCatalogueContentItemId }
          };
        }
      );
      abbIotDevice.Alter(
        abbIotDevice => abbIotDevice.ChartPart,
        chartPart =>
        {
          chartPart.ChartContentItemId = abbChart.ContentItemId;
        }
      );
      await contentManager.CreateAsync(abbIotDevice, VersionOptions.Latest);
    }
  }

  internal static async Task<string> PopulateRegulatoryAgencyCatalogue(
    IServiceProvider serviceProvider
  )
  {
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();

    var regulatoryAgencyCatalogue =
      await contentManager.NewContentAsync<RegulatoryAgencyCatalogueItem>();
    regulatoryAgencyCatalogue.Alter(
      regulatoryAgencyCatalogue => regulatoryAgencyCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Regulatory agency catalogue";
      }
    );
    regulatoryAgencyCatalogue.Alter(
      regulatoryAgencyCatalogue =>
        regulatoryAgencyCatalogue.RegulatoryAgencyCataloguePart,
      regulatoryAgencyCataloguePart =>
      {
        regulatoryAgencyCataloguePart.BusinessUsageFee = new()
        {
          Value = 0.00375M
        };
        regulatoryAgencyCataloguePart.RenewableEnergyFee = new()
        {
          Value = 0.1050M
        };
        regulatoryAgencyCataloguePart.TaxRate = new() { Value = 0.13M };
      }
    );

    await contentManager.CreateAsync(
      regulatoryAgencyCatalogue,
      VersionOptions.Latest
    );

    return regulatoryAgencyCatalogue.ContentItemId;
  }

  internal static async Task<(
    string WhiteHighVoltageOperatorCatalogueContentItemId,
    string WhiteMediumVoltageOperatorCatalogueContentItemId,
    string BlueOperatorCatalogueContentItemId,
    string WhiteLowVoltageOperatorCatalogueContentItemId,
    string RedOperatorCatalogueContentItemId,
    string YellowOperatorCatalogueContentItemId
  )> PopulateOperatorCatalogues(IServiceProvider serviceProvider)
  {
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();

    var whiteHighVoltageOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    whiteHighVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "White high voltage operator catalogue";
      }
    );
    whiteHighVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new() { Text = "High" };
        operatorCataloguePart.Model = new() { Text = "White" };
        operatorCataloguePart.HighEnergyPrice = new() { Value = 0.04M };
        operatorCataloguePart.LowEnergyPrice = new() { Value = 0.02M };
        operatorCataloguePart.MaxPowerPrice = new() { Value = 14.00M };
        operatorCataloguePart.IotDeviceFee = new() { Value = 68.00M };
      }
    );
    await contentManager.CreateAsync(
      whiteHighVoltageOperatorCatalogue,
      VersionOptions.Latest
    );

    var whiteMediumVoltageOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    whiteMediumVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "White medium voltage operator catalogue";
      }
    );
    whiteMediumVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new() { Text = "Medium" };
        operatorCataloguePart.Model = new() { Text = "White" };
        operatorCataloguePart.HighEnergyPrice = new() { Value = 0.14M };
        operatorCataloguePart.LowEnergyPrice = new() { Value = 0.07M };
        operatorCataloguePart.MaxPowerPrice = new() { Value = 26.00M };
        operatorCataloguePart.IotDeviceFee = new() { Value = 66.00M };
      }
    );
    await contentManager.CreateAsync(
      whiteMediumVoltageOperatorCatalogue,
      VersionOptions.Latest
    );

    var blueOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    blueOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Blue operator catalogue";
      }
    );
    blueOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new() { Text = "Low" };
        operatorCataloguePart.Model = new() { Text = "Blue" };
        operatorCataloguePart.EnergyPrice = new() { Value = 0.31M };
        operatorCataloguePart.IotDeviceFee = new() { Value = 41.30M };
      }
    );
    await contentManager.CreateAsync(
      blueOperatorCatalogue,
      VersionOptions.Latest
    );

    var whiteLowVoltageOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    whiteLowVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "White low voltage operator catalogue";
      }
    );
    whiteLowVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new() { Text = "Low" };
        operatorCataloguePart.Model = new() { Text = "White" };
        operatorCataloguePart.HighEnergyPrice = new() { Value = 0.39M };
        operatorCataloguePart.LowEnergyPrice = new() { Value = 0.17M };
        operatorCataloguePart.IotDeviceFee = new() { Value = 41.30M };
      }
    );
    await contentManager.CreateAsync(
      whiteLowVoltageOperatorCatalogue,
      VersionOptions.Latest
    );

    var redOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    redOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Red operator catalogue";
      }
    );
    redOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new() { Text = "Low" };
        operatorCataloguePart.Model = new() { Text = "Red" };
        operatorCataloguePart.HighEnergyPrice = new() { Value = 0.22M };
        operatorCataloguePart.LowEnergyPrice = new() { Value = 0.1M };
        operatorCataloguePart.MaxPowerPrice = new() { Value = 39.00M };
        operatorCataloguePart.IotDeviceFee = new() { Value = 41.30M };
      }
    );
    await contentManager.CreateAsync(
      redOperatorCatalogue,
      VersionOptions.Latest
    );

    var yellowOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    yellowOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Yellow operator catalogue";
      }
    );
    yellowOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new() { Text = "Low" };
        operatorCataloguePart.Model = new() { Text = "Yellow" };
        operatorCataloguePart.EnergyPrice = new() { Value = 0.24M };
        operatorCataloguePart.IotDeviceFee = new() { Value = 15.45M };
      }
    );
    await contentManager.CreateAsync(
      yellowOperatorCatalogue,
      VersionOptions.Latest
    );

    return (
      whiteHighVoltageOperatorCatalogue.ContentItemId,
      whiteMediumVoltageOperatorCatalogue.ContentItemId,
      blueOperatorCatalogue.ContentItemId,
      whiteLowVoltageOperatorCatalogue.ContentItemId,
      redOperatorCatalogue.ContentItemId,
      yellowOperatorCatalogue.ContentItemId
    );
  }
}
