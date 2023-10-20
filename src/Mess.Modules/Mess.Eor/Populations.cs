using Mess.Chart.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Models;
using Mess.Iot.Abstractions.Indexes;
using Mess.Fields.Abstractions.Extensions;
using Mess.OrchardCore;
using OrchardCore.ContentManagement;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using YesSql;
using Mess.Fields.Abstractions.ApiKeys;
using Mess.Fields.Abstractions;
using Mess.Population.Abstractions;

namespace Mess.Eor;

public class Populations : IPopulation
{
  public async Task PopulateAsync()
  {
    var ownerId = "OwnerId";
    await _userService.CreateUserAsync(
      new User
      {
        UserId = ownerId,
        UserName = "Owner",
        Email = "owner@dev.com",
        RoleNames = new[] { "EOR measurement device owner" }
      },
      "Owner123!",
      (_, _) => { }
    );

    var adminId = "AdminId";
    await _userService.CreateUserAsync(
      new User
      {
        UserId = adminId,
        UserName = "Admin",
        Email = "admin@dev.com",
        RoleNames = new[] { "EOR measurement device administrator" }
      },
      "Admin123!",
      (_, _) => { }
    );

    var eorCurrentDataset =
      await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    eorCurrentDataset.Alter(
      eorCurrentDataset => eorCurrentDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color = new() { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new() { Text = "Current" };
        timeseriesChartDatasetPart.Property = nameof(EorMeasurement.Current);
      }
    );
    var eorVoltageDataset =
      await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    eorVoltageDataset.Alter(
      eorVoltageDataset => eorVoltageDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color = new() { Value = "#00ff00" };
        timeseriesChartDatasetPart.Label = new() { Text = "Voltage" };
        timeseriesChartDatasetPart.Property = nameof(EorMeasurement.Voltage);
      }
    );
    var eorModeDataset =
      await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    eorModeDataset.Alter(
      eorModeDataset => eorModeDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color = new() { Value = "#0000ff" };
        timeseriesChartDatasetPart.Label = new() { Text = "Mode" };
        timeseriesChartDatasetPart.Property = nameof(EorStatus.Mode);
      }
    );
    var eorChart = await _contentManager.NewContentAsync<TimeseriesChartItem>();
    eorChart.Alter(
      eorChart => eorChart.TitlePart,
      titlePart =>
      {
        titlePart.Title = "Eor";
      }
    );
    eorChart.Alter(
      eorChart => eorChart.TimeseriesChartPart,
      timeseriesChartPart =>
      {
        timeseriesChartPart.ChartContentType = "EorIotDevice";
        timeseriesChartPart.History = new()
        {
          Value = new(Unit: IntervalUnit.Minute, Count: 10)
        };
        timeseriesChartPart.RefreshInterval = new()
        {
          Value = new(Unit: IntervalUnit.Second, Count: 10)
        };
        timeseriesChartPart.Datasets = new()
        {
          eorCurrentDataset,
          eorVoltageDataset,
          eorModeDataset
        };
      }
    );
    await _contentManager.CreateAsync(eorChart, VersionOptions.Latest);

    var eorDeviceId = "eor";
    var eorApiKey = "eor";
    var eorIotDevice =
      (
        await _session
          .Query<ContentItem, IotDeviceIndex>()
          .Where(index => index.DeviceId == eorDeviceId)
          .FirstOrDefaultAsync()
      )?.AsContent<EorIotDeviceItem>()
      ?? await _contentManager.NewContentAsync<EorIotDeviceItem>();
    eorIotDevice.Inner.Owner = adminId;
    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.TitlePart,
      titlePart =>
      {
        titlePart.Title = eorDeviceId;
      }
    );
    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.IotDevicePart,
      measurementDevicePart =>
      {
        measurementDevicePart.DeviceId = new() { Text = eorDeviceId };
      }
    );
    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.ChartPart,
      chartPart =>
      {
        chartPart.ChartContentItemId = eorChart.ContentItemId;
      }
    );
    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.EorIotDevicePart,
      eorIotDevice =>
      {
        eorIotDevice.Owner = new() { UserIds = new[] { ownerId } };
        eorIotDevice.ManufactureDate = new() { Value = DateTime.UtcNow };
        eorIotDevice.Manufacturer = new() { Text = "Siemens" };
        eorIotDevice.CommisionDate = new() { Value = DateTime.UtcNow };
        eorIotDevice.ProductNumber = new() { Text = "123456789" };
        eorIotDevice.Longitude = new() { Value = -100.784430m };
        eorIotDevice.Latitude = new() { Value = 31.697256m };
        eorIotDevice.ApiKey = _apiKeyFieldService.HashApiKeyField(eorApiKey);
      }
    );
    await _contentManager.CreateAsync(eorIotDevice, VersionOptions.Latest);
  }

  public Populations(
    IContentManager contentManager,
    ISession session,
    IUserService userService,
    IApiKeyFieldService apiKeyFieldService
  )
  {
    _contentManager = contentManager;
    _session = session;
    _userService = userService;
    _apiKeyFieldService = apiKeyFieldService;
  }

  private readonly IContentManager _contentManager;

  private readonly ISession _session;

  private readonly IUserService _userService;

  private readonly IApiKeyFieldService _apiKeyFieldService;
}
