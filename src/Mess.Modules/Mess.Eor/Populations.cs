using Etch.OrchardCore.Fields.Colour.Fields;
using Mess.Chart.Abstractions.Models;
using Mess.Cms;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Fields.Abstractions;
using Mess.Fields.Abstractions.ApiKeys;
using Mess.Fields.Abstractions.Extensions;
using Mess.Fields.Abstractions.Fields;
using Mess.Population.Abstractions;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;

namespace Mess.Eor;

public class Populations : IPopulation
{
  private readonly IApiKeyFieldService _apiKeyFieldService;

  private readonly IContentManager _contentManager;

  private readonly IUserService _userService;

  public Populations(
    IContentManager contentManager,
    IUserService userService,
    IApiKeyFieldService apiKeyFieldService
  )
  {
    _contentManager = contentManager;
    _userService = userService;
    _apiKeyFieldService = apiKeyFieldService;
  }

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
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#ff0000" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Current" };
        timeseriesChartDatasetPart.Property = nameof(EorMeasurement.Current);
      }
    );
    var eorVoltageDataset =
      await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    eorVoltageDataset.Alter(
      eorVoltageDataset => eorVoltageDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#00ff00" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Voltage" };
        timeseriesChartDatasetPart.Property = nameof(EorMeasurement.Voltage);
      }
    );
    var eorModeDataset =
      await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
    eorModeDataset.Alter(
      eorModeDataset => eorModeDataset.TimeseriesChartDatasetPart,
      timeseriesChartDatasetPart =>
      {
        timeseriesChartDatasetPart.Color =
          new ColourField { Value = "#0000ff" };
        timeseriesChartDatasetPart.Label = new TextField { Text = "Mode" };
        timeseriesChartDatasetPart.Property = nameof(EorStatus.Mode);
      }
    );
    var eorChart = await _contentManager.NewContentAsync<TimeseriesChartItem>();
    eorChart.Alter(
      eorChart => eorChart.TitlePart,
      titlePart => { titlePart.Title = "Eor"; }
    );
    eorChart.Inner.DisplayText = "Eor";
    eorChart.Alter(
      eorChart => eorChart.TimeseriesChartPart,
      timeseriesChartPart =>
      {
        timeseriesChartPart.ChartContentType = "EorIotDevice";
        timeseriesChartPart.History = new IntervalField
        {
          Value = new Interval(IntervalUnit.Minute, 10)
        };
        timeseriesChartPart.RefreshInterval = new IntervalField
        {
          Value = new Interval(IntervalUnit.Second, 10)
        };
        timeseriesChartPart.Datasets = new List<ContentItem>
        {
          eorCurrentDataset,
          eorVoltageDataset,
          eorModeDataset
        };
      }
    );
    await _contentManager.CreateAsync(eorChart, VersionOptions.Latest);

    var eorIotDevice =
      await _contentManager.NewContentAsync<EorIotDeviceItem>();
    eorIotDevice.Inner.Owner = adminId;
    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.TitlePart,
      titlePart => { titlePart.Title = "Eor"; }
    );
    eorIotDevice.Inner.DisplayText = "Eor";
    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.IotDevicePart,
      measurementDevicePart =>
      {
        measurementDevicePart.DeviceId = new TextField { Text = "eor" };
      }
    );
    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.ChartPart,
      chartPart => { chartPart.ChartContentItemId = eorChart.ContentItemId; }
    );
    eorIotDevice.Alter(
      eorIotDevice => eorIotDevice.EorIotDevicePart,
      eorIotDevice =>
      {
        eorIotDevice.Owner = new UserPickerField
          { UserIds = new[] { ownerId } };
        eorIotDevice.ManufactureDate =
          new DateField { Value = DateTime.UtcNow };
        eorIotDevice.Manufacturer = new TextField { Text = "Siemens" };
        eorIotDevice.CommisionDate = new DateField { Value = DateTime.UtcNow };
        eorIotDevice.ProductNumber = new TextField { Text = "123456789" };
        eorIotDevice.Longitude = new NumericField { Value = -100.784430m };
        eorIotDevice.Latitude = new NumericField { Value = 31.697256m };
        eorIotDevice.ApiKey = _apiKeyFieldService.HashApiKeyField("eor");
      }
    );
    await _contentManager.CreateAsync(eorIotDevice, VersionOptions.Latest);
  }
}
