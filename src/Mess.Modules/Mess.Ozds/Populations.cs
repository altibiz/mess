using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using Microsoft.AspNetCore.Identity;
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
using YesSql;

namespace Mess.Ozds;

public class Populations : IPopulation
{
  public async Task PopulateAsync()
  {
    var regulatoryAgencyCatalogueContentItemId =
      await CreateAsyncMigrations.PopulateRegulatoryAgencyCatalogue(
        _serviceProvider
      );

    (
      string whiteHighVoltageOperatorCatalogueContentItemId,
      string whiteMediumVoltageOperatorCatalogueContentItemId,
      string blueOperatorCatalogueContentItemId,
      string whiteLowVoltageOperatorCatalogueContentItemId,
      string redOperatorCatalogueContentItemId,
      string yellowOperatorCatalogueContentItemId
    ) = await CreateAsyncMigrations.PopulateOperatorCatalogues(
      _serviceProvider
    );

    (string? _, string? operatorContentItemId) =
      await CreateAsyncMigrations.PopulateOperator(
        _serviceProvider,
        regulatoryAgencyCatalogueContentItemId,
        whiteHighVoltageOperatorCatalogueContentItemId!,
        whiteMediumVoltageOperatorCatalogueContentItemId!,
        blueOperatorCatalogueContentItemId!,
        whiteLowVoltageOperatorCatalogueContentItemId!,
        redOperatorCatalogueContentItemId!,
        yellowOperatorCatalogueContentItemId!
      );

    (string? _, string? systemContentItemId) =
      await CreateAsyncMigrations.PopulateSystem(
        _serviceProvider,
        operatorContentItemId!,
        whiteHighVoltageOperatorCatalogueContentItemId!,
        whiteMediumVoltageOperatorCatalogueContentItemId!,
        blueOperatorCatalogueContentItemId!,
        whiteLowVoltageOperatorCatalogueContentItemId!,
        redOperatorCatalogueContentItemId!,
        yellowOperatorCatalogueContentItemId!
      );

    (string? _, string? unitContentItemId) =
      await CreateAsyncMigrations.PopulateUnit(
        _serviceProvider,
        systemContentItemId!
      );

    await CreateAsyncMigrations.PopulatePidgeon(
      _serviceProvider,
      unitContentItemId!
    );

    await CreateAsyncMigrations.PopulateAbb(
      _serviceProvider,
      unitContentItemId!,
      whiteLowVoltageOperatorCatalogueContentItemId!,
      whiteLowVoltageOperatorCatalogueContentItemId!
    );

    await CreateAsyncMigrations.PopulateSchneider(
      _serviceProvider,
      unitContentItemId!,
      whiteLowVoltageOperatorCatalogueContentItemId!,
      whiteLowVoltageOperatorCatalogueContentItemId!
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
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var session = serviceProvider.GetRequiredService<ISession>();

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
    distributionSystemOperator.Inner.DisplayText = "Operator";
    distributionSystemOperator.Alter(
      distributionSystemOperator => distributionSystemOperator.LegalEntityPart,
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
        distributionSystemOperatorPart.RegulatoryAgencyCatalogue = new()
        {
          ContentItemIds = new[] { regulatoryAgencyCatalogueContentItemId }
        };

        distributionSystemOperatorPart.WhiteHighVoltageOperatorCatalogue = new()
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

        distributionSystemOperatorPart.WhiteLowVoltageOperatorCatalogue = new()
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

    await session.SaveChangesAsync();

    return (operatorUser.UserId, distributionSystemOperator.ContentItemId);
  }

  internal static async Task<(
    string? UserId,
    string? ContentItemId
  )> PopulateSystem(
    IServiceProvider serviceProvider,
    string operatorContentItemId,
    string whiteHighVoltageOperatorCatalogueContentItemId,
    string whiteMediumVoltageOperatorCatalogueContentItemId,
    string blueOperatorCatalogueContentItemId,
    string whiteLowVoltageOperatorCatalogueContentItemId,
    string redOperatorCatalogueContentItemId,
    string yellowOperatorCatalogueContentItemId
  )
  {
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var session = serviceProvider.GetRequiredService<ISession>();

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
    closedDistributionSystem.Inner.DisplayText = "System";
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
      closedDistributionSystem => closedDistributionSystem.ContainedPart,
      containedPart =>
      {
        containedPart.ListContentItemId = operatorContentItemId;
      }
    );
    closedDistributionSystem.Alter(
      closedDistributionSystem =>
        closedDistributionSystem.ClosedDistributionSystemPart,
      closedDistributionSystemPart =>
      {
        closedDistributionSystemPart.WhiteHighVoltageOperatorCatalogue = new()
        {
          ContentItemIds = new[]
          {
            whiteHighVoltageOperatorCatalogueContentItemId
          }
        };

        closedDistributionSystemPart.WhiteMediumVoltageOperatorCatalogue = new()
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

    await session.SaveChangesAsync();

    return (systemUser.UserId, closedDistributionSystem.ContentItemId);
  }

  internal static async Task<(
    string? UserId,
    string? ContentItemId
  )> PopulateUnit(
    IServiceProvider serviceProvider,
    string systemContentItemId
  )
  {
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var session = serviceProvider.GetRequiredService<ISession>();

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
    distributionSystemUnit.Inner.DisplayText = "Unit";
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
      distributionSystemUnit => distributionSystemUnit.ContainedPart,
      containedPart =>
      {
        containedPart.ListContentItemId = systemContentItemId;
      }
    );
    distributionSystemUnit.Alter(
      distributionSystemUnit =>
        distributionSystemUnit.DistributionSystemUnitPart,
      distributionSystemUnitPart => { }
    );

    await contentManager.CreateAsync(
      distributionSystemUnit,
      VersionOptions.Latest
    );

    await session.SaveChangesAsync();

    return (unitUser.UserId, distributionSystemUnit.ContentItemId);
  }

  internal static async Task PopulatePidgeon(
    IServiceProvider serviceProvider,
    string unitContentItemId
  )
  {
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var apiKeyFieldService =
      serviceProvider.GetRequiredService<IApiKeyFieldService>();
    var session = serviceProvider.GetRequiredService<ISession>();

    var pidgeonIotDevice =
      await contentManager.NewContentAsync<PidgeonIotDeviceItem>();
    pidgeonIotDevice.Alter(
      pidgeonIotDevice => pidgeonIotDevice.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Pidgeon";
      }
    );
    pidgeonIotDevice.Inner.DisplayText = "Pidgeon";
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
      ozdsIotDevicePart => { }
    );
    pidgeonIotDevice.Alter(
      pidgeonIotDevice => pidgeonIotDevice.ContainedPart,
      containedPart =>
      {
        containedPart.ListContentItemId = unitContentItemId;
      }
    );
    await contentManager.CreateAsync(pidgeonIotDevice, VersionOptions.Latest);

    await session.SaveChangesAsync();
  }

  internal static async Task PopulateAbb(
    IServiceProvider serviceProvider,
    string unitContentItemId,
    string usageCatalogueContentItemId,
    string supplyCatalogueContentItemId
  )
  {
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var apiKeyFieldService =
      serviceProvider.GetRequiredService<IApiKeyFieldService>();
    var session = serviceProvider.GetRequiredService<ISession>();

    var abbPowerDataset =
      await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    abbPowerDataset.Alter(
      eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color = new() { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new() { Text = "Power" };
        timeseriesChartDatasetPart.Property = nameof(
          AbbMeasurement.ActivePowerTotal_W
        );
      }
    );
    var abbChart = await contentManager.NewContentAsync<TimeseriesChartItem>();
    abbChart.Alter(
      abbChart => abbChart.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Abb";
      }
    );
    abbChart.Inner.DisplayText = "Abb";
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

    var abbIotDevice = await contentManager.NewContentAsync<AbbIotDeviceItem>();
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Abb";
      }
    );
    abbIotDevice.Inner.DisplayText = "Abb";
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.IotDevicePart,
      measurementDevicePart =>
      {
        measurementDevicePart.DeviceId = new() { Text = "abb" };
      }
    );
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.ContainedPart,
      containedPart =>
      {
        containedPart.ListContentItemId = unitContentItemId;
      }
    );
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.OzdsIotDevicePart,
      ozdsIotDevicePart =>
      {
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

    await session.SaveChangesAsync();
  }

  internal static async Task PopulateSchneider(
    IServiceProvider serviceProvider,
    string unitContentItemId,
    string usageCatalogueContentItemId,
    string supplyCatalogueContentItemId
  )
  {
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var apiKeyFieldService =
      serviceProvider.GetRequiredService<IApiKeyFieldService>();
    var session = serviceProvider.GetRequiredService<ISession>();

    var schneiderPowerDataset =
      await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    schneiderPowerDataset.Alter(
      eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color = new() { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new() { Text = "Power" };
        timeseriesChartDatasetPart.Property = nameof(
          SchneiderMeasurement.ActivePowerTotal_kW
        );
      }
    );
    var schneiderChart =
      await contentManager.NewContentAsync<TimeseriesChartItem>();
    schneiderChart.Alter(
      schneiderChart => schneiderChart.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Schneider";
      }
    );
    schneiderChart.Inner.DisplayText = "Schneider";
    schneiderChart.Alter(
      schneiderChart => schneiderChart.TimeseriesChartPart,
      timeseriesChartPart =>
      {
        timeseriesChartPart.ChartContentType = "SchneiderIotDevice";
        timeseriesChartPart.History = new()
        {
          Value = new(Unit: IntervalUnit.Minute, Count: 10)
        };
        timeseriesChartPart.RefreshInterval = new()
        {
          Value = new(Unit: IntervalUnit.Second, Count: 10)
        };
        timeseriesChartPart.Datasets = new() { schneiderPowerDataset };
      }
    );
    await contentManager.CreateAsync(schneiderChart, VersionOptions.Latest);

    var schneiderIotDevice =
      await contentManager.NewContentAsync<SchneiderIotDeviceItem>();
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Schneider";
      }
    );
    schneiderIotDevice.Inner.DisplayText = "Schneider";
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.IotDevicePart,
      measurementDevicePart =>
      {
        measurementDevicePart.DeviceId = new() { Text = "schneider" };
      }
    );
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.ContainedPart,
      containedPart =>
      {
        containedPart.ListContentItemId = unitContentItemId;
      }
    );
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.OzdsIotDevicePart,
      ozdsIotDevicePart =>
      {
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
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.ChartPart,
      chartPart =>
      {
        chartPart.ChartContentItemId = schneiderChart.ContentItemId;
      }
    );
    await contentManager.CreateAsync(schneiderIotDevice, VersionOptions.Latest);

    await session.SaveChangesAsync();
  }

  internal static async Task<string> PopulateRegulatoryAgencyCatalogue(
    IServiceProvider serviceProvider
  )
  {
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var session = serviceProvider.GetRequiredService<ISession>();

    var regulatoryAgencyCatalogue =
      await contentManager.NewContentAsync<RegulatoryAgencyCatalogueItem>();
    regulatoryAgencyCatalogue.Alter(
      regulatoryAgencyCatalogue => regulatoryAgencyCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Regulatory agency catalogue";
      }
    );
    regulatoryAgencyCatalogue.Inner.DisplayText = "Regulatory agency catalogue";
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

    await session.SaveChangesAsync();

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
    var session = serviceProvider.GetRequiredService<ISession>();

    var whiteHighVoltageOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    whiteHighVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "White high voltage operator catalogue";
      }
    );
    whiteHighVoltageOperatorCatalogue.Inner.DisplayText =
      "White high voltage operator catalogue";
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
    whiteMediumVoltageOperatorCatalogue.Inner.DisplayText =
      "White medium voltage operator catalogue";
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
    blueOperatorCatalogue.Inner.DisplayText = "Blue operator catalogue";
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
    whiteLowVoltageOperatorCatalogue.Inner.DisplayText =
      "White low voltage operator catalogue";
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
    redOperatorCatalogue.Inner.DisplayText = "Red operator catalogue";
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
    yellowOperatorCatalogue.Inner.DisplayText = "Yellow operator catalogue";
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

    await session.SaveChangesAsync();

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
