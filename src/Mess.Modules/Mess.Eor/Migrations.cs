using Mess.Chart.Abstractions.Models;
using Mess.Eor.Abstractions.Timeseries;
using Mess.Eor.Abstractions.Indexes;
using Mess.Eor.Abstractions.Models;
using Mess.Iot.Abstractions.Indexes;
using Mess.Fields.Abstractions.Extensions;
using Mess.OrchardCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using OrchardCore.Title.Models;
using OrchardCore.Users.Models;
using OrchardCore.Users.Services;
using YesSql;
using YesSql.Sql;
using Mess.Fields.Abstractions.ApiKeys;
using Mess.Fields.Abstractions.Settings;
using Mess.Fields.Abstractions;

namespace Mess.Eor;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    _contentDefinitionManager.AlterPartDefinition(
      "EorIotDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An Eor measurement device.")
          .WithDisplayName("Eor measurement device")
          .WithField(
            "Owner",
            fieldBuilder =>
              fieldBuilder
                .OfType("UserPickerField")
                .WithDisplayName("Owner")
                .WithDescription("The owner of the measurement device.")
                .WithSettings(
                  new UserPickerFieldSettings
                  {
                    Multiple = false,
                    Required = true,
                    DisplayAllUsers = false,
                    DisplayedRoles = new[] { "EOR measurement device owner" },
                    Hint = "The owner of the measurement device."
                  }
                )
          )
          .WithField(
            "ManufactureDate",
            fieldBuilder =>
              fieldBuilder
                .OfType("DateField")
                .WithDisplayName("Manufacturer date")
                .WithDescription("Date of manufacture.")
                .WithSettings(
                  new DateFieldSettings
                  {
                    Required = true,
                    Hint = "Date of manufacture."
                  }
                )
          )
          .WithField(
            "Manufacturer",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Manufacturer")
                .WithDescription("Manufacturer.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Required = true,
                    Hint = "Manufacturer."
                  }
                )
          )
          .WithField(
            "CommisionDate",
            fieldBuilder =>
              fieldBuilder
                .OfType("DateField")
                .WithDisplayName("Commision date")
                .WithDescription("Date of commision.")
                .WithSettings(
                  new DateFieldSettings
                  {
                    Required = true,
                    Hint = "Date of commision."
                  }
                )
          )
          .WithField(
            "ProductNumber",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Product number")
                .WithDescription("Product number.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Required = true,
                    Hint = "Product number."
                  }
                )
          )
          .WithField(
            "Latitude",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Latitude")
                .WithDescription("Latitude.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Required = true,
                    Minimum = -180,
                    Maximum = 180,
                    Scale = 6,
                    Hint = "Latitude."
                  }
                )
          )
          .WithField(
            "Longitude",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Longitude")
                .WithDescription("Longitude.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Required = true,
                    Minimum = -180,
                    Maximum = 180,
                    Scale = 6,
                    Hint = "Longitude."
                  }
                )
          )
          .WithField(
            "ApiKey",
            fieldBuilder =>
              fieldBuilder
                .OfType("ApiKeyField")
                .WithDisplayName("API key")
                .WithDescription("API key.")
                .WithSettings<ApiKeyFieldSettings>(
                  new()
                  {
                    Hint = "API key that will be used to authorize the device."
                  }
                )
          )
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "EorIotDevice",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Securable()
          .DisplayedAs("Eor measurement device")
          .WithDescription("An Eor measurement device.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription(
                  "Title displaying the identifier of the Eor measurement device."
                )
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedHidden,
                    Pattern =
                      @"{{- ContentItem.Content.IotDevicePart.DeviceId.Text -}}"
                  }
                )
          )
          .WithPart(
            "IotDevicePart",
            part =>
              part.WithDisplayName("Measurement device")
                .WithDescription("A measurement device.")
                .WithPosition("2")
          )
          .WithPart(
            "EorIotDevicePart",
            part =>
              part.WithDisplayName("Eor measurement device")
                .WithDescription("An Eor measurement device.")
                .WithPosition("3")
          )
          .WithPart(
            "ChartPart",
            part =>
              part.WithDisplayName("Chart")
                .WithDescription(
                  "Chart displaying the Eor measurement device data."
                )
                .WithPosition("4")
          )
    );

    SchemaBuilder.CreateMapIndexTable<EorIotDeviceIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("DeviceId", c => c.WithLength(64))
          .Column<string>("OwnerId", c => c.WithLength(64))
          .Column<string>("Author", c => c.WithLength(256))
    );
    SchemaBuilder.AlterIndexTable<EorIotDeviceIndex>(table =>
    {
      table.CreateIndex("IDX_EorIotDeviceIndex_OwnerId", "OwnerId");
      table.CreateIndex("IDX_EorIotDeviceIndex_Author", "Author");
    });

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "EorIotDeviceAdmin",
        RoleName = "EOR measurement device administrator",
        RoleDescription = "Administrator of an EOR measurement devices.",
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
            ClaimValue = "ManageUsersInRole_EOR measurement device owner"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_EOR measurement device owner"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ViewOwn_EorIotDevice"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ControlOwn_EorIotDevice"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "PublishOwn_EorIotDevice"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "EditOwn_EorIotDevice"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "DeleteOwn_EorIotDevice"
          },
        }
      }
    );

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "EorIotDeviceOwner",
        RoleName = "EOR measurement device owner",
        RoleDescription = "Owner of an EOR measurement devices.",
        RoleClaims = new()
        {
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ViewOwned_EorIotDevice"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ControlOwned_EorIotDevice"
          }
        }
      }
    );

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

    if (_hostEnvironment.IsDevelopment())
    {
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
      var eorChart =
        await _contentManager.NewContentAsync<TimeseriesChartItem>();
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

    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    IRecipeMigrator recipeMigrator,
    RoleManager<IRole> roleManager,
    IContentManager contentManager,
    IHostEnvironment hostEnvironment,
    ISession session,
    IUserService userService,
    IApiKeyFieldService apiKeyFieldService
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _recipeMigrator = recipeMigrator;
    _roleManager = roleManager;
    _contentManager = contentManager;
    _hostEnvironment = hostEnvironment;
    _session = session;
    _userService = userService;
    _apiKeyFieldService = apiKeyFieldService;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IRecipeMigrator _recipeMigrator;
  private readonly RoleManager<IRole> _roleManager;
  private readonly IContentManager _contentManager;
  private readonly IHostEnvironment _hostEnvironment;
  private readonly ISession _session;
  private readonly IUserService _userService;
  private readonly IApiKeyFieldService _apiKeyFieldService;
}
