using Etch.OrchardCore.Fields.Colour.Fields;
using Mess.Chart.Abstractions.Models;
using Mess.Cms;
using Mess.Cms.Extensions.OrchardCore;
using Mess.Fields.Abstractions;
using Mess.Fields.Abstractions.ApiKeys;
using Mess.Fields.Abstractions.Extensions;
using Mess.Fields.Abstractions.Fields;
using Mess.Ozds.Abstractions.Models;
using Mess.Ozds.Abstractions.Timeseries;
using Mess.Population.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Security;
using OrchardCore.Users.Services;
using YesSql;

namespace Mess.Ozds;

public class Populations : IPopulation
{
  private readonly IServiceProvider _serviceProvider;

  public Populations(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  public async Task PopulateAsync()
  {
    var regulatoryAgencyCatalogueContentItemId =
      await CreateAsyncMigrations.PopulateRegulatoryAgencyCatalogue(
        _serviceProvider
      );

    var (whiteMediumVoltageOperatorCatalogueContentItemId,
        blueLowVoltageOperatorCatalogueContentItemId,
        whiteLowVoltageOperatorCatalogueContentItemId,
        redLowVoltageOperatorCatalogueContentItemId
        ) =
      await CreateAsyncMigrations.PopulateOperatorCatalogues(
        _serviceProvider
      );

    var (_, operatorContentItemId) =
      await CreateAsyncMigrations.PopulateOperator(
        _serviceProvider
      );

    var (_, systemContentItemId) =
      await CreateAsyncMigrations.PopulateSystem(
        _serviceProvider,
        operatorContentItemId!
      );

    var (_, unitContentItemId) =
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
      redLowVoltageOperatorCatalogueContentItemId!,
      regulatoryAgencyCatalogueContentItemId!
    );

    await CreateAsyncMigrations.PopulateSchneider(
      _serviceProvider,
      unitContentItemId!,
      blueLowVoltageOperatorCatalogueContentItemId!,
      regulatoryAgencyCatalogueContentItemId!
    );
  }
}

internal static partial class CreateAsyncMigrations
{
  internal static async Task<(
    string? UserId,
    string? ContentItemId
    )> PopulateOperator(
    IServiceProvider serviceProvider
  )
  {
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var session = serviceProvider.GetRequiredService<ISession>();

    var operatorUser = await userService.CreateDevUserAsync(
      "OperatorId",
      "Operator", "Distribution System Operator Representative",
      "Legall entity representative");

    var distributionSystemOperator =
      await contentManager.NewContentAsync<DistributionSystemOperatorItem>();
    distributionSystemOperator.Alter(
      distributionSystemOperator => distributionSystemOperator.TitlePart,
      titlePart => { titlePart.Title = "Operator"; }
    );
    distributionSystemOperator.Inner.DisplayText = "Operator";
    distributionSystemOperator.Alter(
      distributionSystemOperator => distributionSystemOperator.LegalEntityPart,
      legalEntityPart =>
      {
        legalEntityPart.Name = new TextField { Text = "Operator" };
        legalEntityPart.City = new TextField { Text = "City" };
        legalEntityPart.Address = new TextField { Text = "Address" };
        legalEntityPart.PostalCode = new TextField { Text = "Postal code" };
        legalEntityPart.Email = new TextField { Text = "Email" };
        legalEntityPart.SocialSecurityNumber = new TextField
        {
          Text = "Social security number"
        };
        legalEntityPart.Representatives = new UserPickerField
        {
          UserIds = new[] { operatorUser.UserId }
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
    string operatorContentItemId
  )
  {
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var session = serviceProvider.GetRequiredService<ISession>();

    var systemUser = await userService.CreateDevUserAsync(
      "SystemId",
      "System", "Closed Distribution System Representative",
      "Legal entity representative");

    var closedDistributionSystem =
      await contentManager.NewContentAsync<ClosedDistributionSystemItem>();
    closedDistributionSystem.Alter(
      closedDistributionSystem => closedDistributionSystem.TitlePart,
      titlePart => { titlePart.Title = "System"; }
    );
    closedDistributionSystem.Inner.DisplayText = "System";
    closedDistributionSystem.Alter(
      closedDistributionSystem => closedDistributionSystem.LegalEntityPart,
      legalEntityPart =>
      {
        legalEntityPart.Name = new TextField { Text = "System" };
        legalEntityPart.City = new TextField { Text = "City" };
        legalEntityPart.Address = new TextField { Text = "Address" };
        legalEntityPart.PostalCode = new TextField { Text = "Postal code" };
        legalEntityPart.Email = new TextField { Text = "Email" };
        legalEntityPart.SocialSecurityNumber = new TextField
        {
          Text = "Social security number"
        };
        legalEntityPart.Representatives = new UserPickerField
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
      "UnitId",
      "Unit", "Distribution System Unit Representative",
      "Legal entity representative");

    var distributionSystemUnit =
      await contentManager.NewContentAsync<DistributionSystemUnitItem>();
    distributionSystemUnit.Alter(
      distributionSystemUnit => distributionSystemUnit.TitlePart,
      titlePart => { titlePart.Title = "Unit"; }
    );
    distributionSystemUnit.Inner.DisplayText = "Unit";
    distributionSystemUnit.Alter(
      distributionSystemUnit => distributionSystemUnit.LegalEntityPart,
      legalEntityPart =>
      {
        legalEntityPart.Name = new TextField { Text = "Unit" };
        legalEntityPart.City = new TextField { Text = "City" };
        legalEntityPart.Address = new TextField { Text = "Address" };
        legalEntityPart.PostalCode = new TextField { Text = "Postal code" };
        legalEntityPart.Email = new TextField { Text = "Email" };
        legalEntityPart.SocialSecurityNumber = new TextField
        {
          Text = "Social security number"
        };
        legalEntityPart.Representatives = new UserPickerField
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
      titlePart => { titlePart.Title = "Pidgeon"; }
    );
    pidgeonIotDevice.Inner.DisplayText = "Pidgeon";
    pidgeonIotDevice.Alter(
      pidgeonIotDevice => pidgeonIotDevice.IotDevicePart,
      measurementDevicePart =>
      {
        measurementDevicePart.DeviceId = new TextField { Text = "pidgeon" };
        measurementDevicePart.IsMessenger = true;
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
      containedPart => { containedPart.ListContentItemId = unitContentItemId; }
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

    var abbPowerL1Dataset =
      await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    abbPowerL1Dataset.Alter(
      eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Power L1" };
        timeseriesChartDatasetPart.Property = nameof(
          AbbMeasurement.ActivePowerL1_W
        );
      }
    );
    var abbPowerL2Dataset =
      await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    abbPowerL2Dataset.Alter(
      eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Power L2" };
        timeseriesChartDatasetPart.Property = nameof(
          AbbMeasurement.ActivePowerL2_W
        );
      }
    );
    var abbPowerL3Dataset =
      await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    abbPowerL3Dataset.Alter(
      eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Power L3" };
        timeseriesChartDatasetPart.Property = nameof(
          AbbMeasurement.ActivePowerL3_W
        );
      }
    );
    var abbChart = await contentManager.NewContentAsync<TimeseriesChartItem>();
    abbChart.Alter(
      abbChart => abbChart.TitlePart,
      titlePart => { titlePart.Title = "Abb"; }
    );
    abbChart.Inner.DisplayText = "Abb";
    abbChart.Alter(
      abbChart => abbChart.TimeseriesChartPart,
      timeseriesChartPart =>
      {
        timeseriesChartPart.ChartContentType = "AbbIotDevice";
        timeseriesChartPart.History = new IntervalField
        {
          Value = new Interval(IntervalUnit.Minute, 10)
        };
        timeseriesChartPart.RefreshInterval = new IntervalField
        {
          Value = new Interval(IntervalUnit.Second, 10)
        };
        timeseriesChartPart.Datasets =
          new List<ContentItem> { abbPowerL1Dataset, abbPowerL2Dataset, abbPowerL3Dataset };
      }
    );
    await contentManager.CreateAsync(abbChart, VersionOptions.Latest);

    var abbIotDevice = await contentManager.NewContentAsync<AbbIotDeviceItem>();
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.TitlePart,
      titlePart => { titlePart.Title = "Abb"; }
    );
    abbIotDevice.Inner.DisplayText = "Abb";
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.IotDevicePart,
      measurementDevicePart =>
      {
        measurementDevicePart.DeviceId = new TextField { Text = "abb" };
      }
    );
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.ContainedPart,
      containedPart => { containedPart.ListContentItemId = unitContentItemId; }
    );
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.OzdsIotDevicePart,
      ozdsIotDevicePart =>
      {
        ozdsIotDevicePart.UsageCatalogue = new ContentPickerField
        {
          ContentItemIds = new[] { usageCatalogueContentItemId }
        };
        ozdsIotDevicePart.SupplyCatalogue = new ContentPickerField
        {
          ContentItemIds = new[] { supplyCatalogueContentItemId }
        };
      }
    );
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.ChartPart,
      chartPart => { chartPart.ChartContentItemId = abbChart.ContentItemId; }
    );
    abbIotDevice.Alter(
      abbIotDevice => abbIotDevice.AbbIotDevicePart,
      abbIotDevicePart =>
      {
        abbIotDevicePart.MinVoltage = new NumericField { Value = 161 };
        abbIotDevicePart.MaxVoltage = new NumericField { Value = 300 };
        abbIotDevicePart.MinCurrent = new NumericField { Value = 0 };
        abbIotDevicePart.MaxCurrent = new NumericField { Value = 80 };
        abbIotDevicePart.MinActivePower = new NumericField { Value = -24000 };
        abbIotDevicePart.MaxActivePower = new NumericField { Value = 24000 };
        abbIotDevicePart.MinReactivePower = new NumericField { Value = -24000 };
        abbIotDevicePart.MaxReactivePower = new NumericField { Value = 24000 };
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

    var schneiderPowerL1Dataset =
      await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    schneiderPowerL1Dataset.Alter(
      eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Power L1" };
        timeseriesChartDatasetPart.Property = nameof(
          SchneiderMeasurement.ActivePowerL1_W
        );
      }
    );
    var schneiderPowerL2Dataset =
      await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    schneiderPowerL2Dataset.Alter(
      eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Power L2" };
        timeseriesChartDatasetPart.Property = nameof(
          SchneiderMeasurement.ActivePowerL2_W
        );
      }
    );
    var schneiderPowerL3Dataset =
      await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    schneiderPowerL3Dataset.Alter(
      eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Power L3" };
        timeseriesChartDatasetPart.Property = nameof(
          SchneiderMeasurement.ActivePowerL3_W
        );
      }
    );
    var schneiderChart =
      await contentManager.NewContentAsync<TimeseriesChartItem>();
    schneiderChart.Alter(
      schneiderChart => schneiderChart.TitlePart,
      titlePart => { titlePart.Title = "Schneider"; }
    );
    schneiderChart.Inner.DisplayText = "Schneider";
    schneiderChart.Alter(
      schneiderChart => schneiderChart.TimeseriesChartPart,
      timeseriesChartPart =>
      {
        timeseriesChartPart.ChartContentType = "SchneiderIotDevice";
        timeseriesChartPart.History = new IntervalField
        {
          Value = new Interval(IntervalUnit.Minute, 10)
        };
        timeseriesChartPart.RefreshInterval = new IntervalField
        {
          Value = new Interval(IntervalUnit.Second, 10)
        };
        timeseriesChartPart.Datasets = new List<ContentItem>
          { schneiderPowerL1Dataset, schneiderPowerL2Dataset, schneiderPowerL3Dataset };
      }
    );
    await contentManager.CreateAsync(schneiderChart, VersionOptions.Latest);

    var schneiderIotDevice =
      await contentManager.NewContentAsync<SchneiderIotDeviceItem>();
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.TitlePart,
      titlePart => { titlePart.Title = "Schneider"; }
    );
    schneiderIotDevice.Inner.DisplayText = "Schneider";
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.IotDevicePart,
      measurementDevicePart =>
      {
        measurementDevicePart.DeviceId = new TextField { Text = "schneider" };
      }
    );
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.ContainedPart,
      containedPart => { containedPart.ListContentItemId = unitContentItemId; }
    );
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.OzdsIotDevicePart,
      ozdsIotDevicePart =>
      {
        ozdsIotDevicePart.UsageCatalogue = new ContentPickerField
        {
          ContentItemIds = new[] { usageCatalogueContentItemId }
        };
        ozdsIotDevicePart.SupplyCatalogue = new ContentPickerField
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
    schneiderIotDevice.Alter(
      schneiderIotDevice => schneiderIotDevice.SchneiderIotDevicePart,
      schneiderIotDevicePart =>
      {
        schneiderIotDevicePart.MinVoltage = new NumericField { Value = 161 };
        schneiderIotDevicePart.MaxVoltage = new NumericField { Value = 300 };
        schneiderIotDevicePart.MinCurrent = new NumericField { Value = 0 };
        schneiderIotDevicePart.MaxCurrent = new NumericField { Value = 80 };
        schneiderIotDevicePart.MinActivePower = new NumericField { Value = -24000 };
        schneiderIotDevicePart.MaxActivePower = new NumericField { Value = 24000 };
        schneiderIotDevicePart.MinReactivePower = new NumericField { Value = -24000 };
        schneiderIotDevicePart.MaxReactivePower = new NumericField { Value = 24000 };
        schneiderIotDevicePart.MinApparentPower = new NumericField { Value = -24000 };
        schneiderIotDevicePart.MaxApparentPower = new NumericField { Value = 24000 };
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
      titlePart => { titlePart.Title = "Regulatory agency catalogue"; }
    );
    regulatoryAgencyCatalogue.Inner.DisplayText = "Regulatory agency catalogue";
    regulatoryAgencyCatalogue.Alter(
      regulatoryAgencyCatalogue =>
        regulatoryAgencyCatalogue.RegulatoryAgencyCataloguePart,
      regulatoryAgencyCataloguePart =>
      {
        regulatoryAgencyCataloguePart.BusinessUsageFee =
          new NumericField { Value = 0.0005M };
        regulatoryAgencyCataloguePart.RenewableEnergyFee =
          new NumericField { Value = 0.013936M };
        regulatoryAgencyCataloguePart.TaxRate =
          new NumericField { Value = 0.13M };
        regulatoryAgencyCataloguePart.HighEnergyPrice =
          new NumericField { Value = 0.1M };
        regulatoryAgencyCataloguePart.LowEnergyPrice =
          new NumericField { Value = 0.06M };
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
    string WhiteMediumVoltageOperatorCatalogueContentItemId,
    string BlueOperatorCatalogueContentItemId,
    string WhiteLowVoltageOperatorCatalogueContentItemId,
    string RedOperatorCatalogueContentItemId
    )> PopulateOperatorCatalogues(IServiceProvider serviceProvider)
  {
    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
    var session = serviceProvider.GetRequiredService<ISession>();

    var whiteMediumVoltageOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    whiteMediumVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Bijeli SN";
      }
    );
    whiteMediumVoltageOperatorCatalogue.Inner.DisplayText =
      "Bijeli SN";
    whiteMediumVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new TextField { Text = "Srednji" };
        operatorCataloguePart.Model = new TextField { Text = "Bijeli" };
        operatorCataloguePart.HighEnergyPrice =
          new NumericField { Value = 0.018581M };
        operatorCataloguePart.LowEnergyPrice =
          new NumericField { Value = 0.009290M };
        operatorCataloguePart.MaxPowerPrice =
          new NumericField { Value = 3.451M };
        operatorCataloguePart.ReactiveEnergyPrice =
          new NumericField { Value = 0.021236M };
        operatorCataloguePart.IotDeviceFee =
          new NumericField { Value = 8.760M };
      }
    );
    await contentManager.CreateAsync(
      whiteMediumVoltageOperatorCatalogue,
      VersionOptions.Latest
    );

    var blueLowVoltageOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    blueLowVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart => { titlePart.Title = "Plavi NN"; }
    );
    blueLowVoltageOperatorCatalogue.Inner.DisplayText = "Plavi NN";
    blueLowVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new TextField { Text = "Low" };
        operatorCataloguePart.Model = new TextField { Text = "Blue" };
        operatorCataloguePart.EnergyPrice =
          new NumericField { Value = 0.041144M };
        operatorCataloguePart.ReactiveEnergyPrice =
          new NumericField { Value = 0.021236M };
        operatorCataloguePart.IotDeviceFee =
          new NumericField { Value = 5.481M };
      }
    );
    await contentManager.CreateAsync(
      blueLowVoltageOperatorCatalogue,
      VersionOptions.Latest
    );

    var whiteLowVoltageOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    whiteLowVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart => { titlePart.Title = "Bijeli NN"; }
    );
    whiteLowVoltageOperatorCatalogue.Inner.DisplayText =
      "Bijeli NN";
    whiteLowVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new TextField { Text = "Low" };
        operatorCataloguePart.Model = new TextField { Text = "White" };
        operatorCataloguePart.HighEnergyPrice =
          new NumericField { Value = 0.051762M };
        operatorCataloguePart.LowEnergyPrice =
          new NumericField { Value = 0.022563M };
        operatorCataloguePart.ReactiveEnergyPrice =
          new NumericField() { Value = 0.021236M };
        operatorCataloguePart.IotDeviceFee =
          new NumericField { Value = 5.481M };
      }
    );
    await contentManager.CreateAsync(
      whiteLowVoltageOperatorCatalogue,
      VersionOptions.Latest
    );

    var redLowVoltageOperatorCatalogue =
      await contentManager.NewContentAsync<OperatorCatalogueItem>();
    redLowVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.TitlePart,
      titlePart => { titlePart.Title = "Crveni NN"; }
    );
    redLowVoltageOperatorCatalogue.Inner.DisplayText = "Crveni NN";
    redLowVoltageOperatorCatalogue.Alter(
      operatorCatalogue => operatorCatalogue.OperatorCataloguePart,
      operatorCataloguePart =>
      {
        operatorCataloguePart.Voltage = new TextField { Text = "Niski" };
        operatorCataloguePart.Model = new TextField { Text = "Crveni" };
        operatorCataloguePart.HighEnergyPrice =
          new NumericField { Value = 0.029199M };
        operatorCataloguePart.LowEnergyPrice =
          new NumericField { Value = 0.013272M };
        operatorCataloguePart.MaxPowerPrice =
          new NumericField { Value = 5.176M };
        operatorCataloguePart.ReactiveEnergyPrice =
          new NumericField { Value = 0.021236M };
        operatorCataloguePart.IotDeviceFee =
          new NumericField { Value = 5.481M };
      }
    );
    await contentManager.CreateAsync(
      redLowVoltageOperatorCatalogue,
      VersionOptions.Latest
    );

    await session.SaveChangesAsync();

    return (
      whiteMediumVoltageOperatorCatalogue.ContentItemId,
      blueLowVoltageOperatorCatalogue.ContentItemId,
      whiteLowVoltageOperatorCatalogue.ContentItemId,
      redLowVoltageOperatorCatalogue.ContentItemId
    );
  }
}
