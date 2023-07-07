using Mess.Chart.Abstractions.Models;
using Mess.Eor.Abstractions.Client;
using Mess.Eor.Abstractions.Indexes;
using Mess.Eor.Abstractions.Models;
using Mess.Eor.Chart.Providers;
using Mess.Eor.MeasurementDevice.Pushing;
using Mess.Eor.MeasurementDevice.Updating;
using Mess.MeasurementDevice.Abstractions.Indexes;
using Mess.OrchardCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Msss.Eor.MeasurementDevice.Polling;
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

namespace Mess.Eor;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    _contentDefinitionManager.AlterPartDefinition(
      "EorMeasurementDevicePart",
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
                    DisplayedRoles = new[] { "MeasurementDeviceOwner" },
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
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "EorMeasurementDevice",
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
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{{- ContentItem.Content.MeasurementDevicePart.DeviceId.Text -}}"
                  }
                )
          )
          .WithPart(
            "MeasurementDevicePart",
            part =>
              part.WithDisplayName("Measurement device")
                .WithDescription("A measurement device.")
                .WithPosition("2")
          )
          .WithPart(
            "EorMeasurementDevicePart",
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

    SchemaBuilder.CreateMapIndexTable<EorMeasurementDeviceIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(30))
          .Column<string>("DeviceId", c => c.WithLength(30))
          .Column<string>("OwnerId", c => c.WithLength(30))
    );
    SchemaBuilder.AlterIndexTable<EorMeasurementDeviceIndex>(
      table =>
        table.CreateIndex("IDX_EorMeasurementDeviceIndex_OwnerId", "OwnerId")
    );

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "EorMeasurementDeviceAdmin",
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
            ClaimValue = "ManageOwnUserInformation"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ManageChart"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ViewEorMeasurementDevice"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ManageEorMeasurementDevice"
          }
        }
      }
    );

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "EorMeasurementDeviceOwner",
        RoleName = "EOR measurement device owner",
        RoleDescription = "Owner of an EOR measurement devices.",
        RoleClaims = new()
        {
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ViewEorMeasurementDevice"
          }
        }
      }
    );

    if (_hostEnvironment.IsDevelopment())
    {
      await _userService.CreateUserAsync(
        new User
        {
          UserId = "OwnerId",
          UserName = "Owner",
          Email = "owner@dev.com",
          RoleNames = new[] { "EOR measurement device owner" }
        },
        "Owner123!",
        (_, _) => { }
      );

      await _userService.CreateUserAsync(
        new User
        {
          UserId = "AdminId",
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
      eorCurrentDataset.Alter(
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
      eorCurrentDataset.Alter(
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
        eorChart => eorChart.TimeseriesChartPart,
        timeseriesChartPart =>
        {
          timeseriesChartPart.ChartProviderId =
            EorChartProvider.ChartProviderId;
          timeseriesChartPart.Datasets = new()
          {
            eorCurrentDataset,
            eorVoltageDataset,
            eorModeDataset
          };
        }
      );
      await _contentManager.PublishAsync(eorChart);

      var eorMeasurementDevice =
        (
          await _session
            .Query<ContentItem, MeasurementDeviceIndex>()
            .Where(index => index.DeviceId == "eor")
            .FirstOrDefaultAsync()
        )?.AsContent<EorMeasurementDeviceItem>()
        ?? await _contentManager.NewContentAsync<EorMeasurementDeviceItem>();
      eorMeasurementDevice.Alter(
        eorMeasurementDevice => eorMeasurementDevice.TitlePart,
        titlePart =>
        {
          titlePart.Title = "eor";
        }
      );
      eorMeasurementDevice.Alter(
        eorMeasurementDevice => eorMeasurementDevice.MeasurementDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "eor" };
          measurementDevicePart.DefaultPushHandlerId =
            EorPushHandler.PushHandlerId;
          measurementDevicePart.DefaultPollHandlerId =
            EorPollHandler.PollHandlerId;
          measurementDevicePart.DefaultUpdateHandlerId =
            EorStatusHandler.UpdateHandlerId;
        }
      );
      eorMeasurementDevice.Alter(
        eorMeasurementDevice => eorMeasurementDevice.ChartPart,
        chartPart =>
        {
          chartPart.ChartDataProviderId = EorChartProvider.ChartProviderId;
          chartPart.ChartContentItemId = eorChart.ContentItemId;
        }
      );
      eorMeasurementDevice.Alter(
        eorMeasurementDevice => eorMeasurementDevice.EorMeasurementDevicePart,
        eorMeasurementDevice =>
        {
          eorMeasurementDevice.Owner = new() { UserIds = new[] { "OwnerId" } };
          eorMeasurementDevice.ManufactureDate = new()
          {
            Value = DateTime.UtcNow
          };
          eorMeasurementDevice.Manufacturer = new() { Text = "Siemens" };
          eorMeasurementDevice.CommisionDate = new()
          {
            Value = DateTime.UtcNow
          };
          eorMeasurementDevice.ProductNumber = new() { Text = "123456789" };
          eorMeasurementDevice.Longitude = new() { Value = -100.784430m };
          eorMeasurementDevice.Latitude = new() { Value = 31.697256m };
        }
      );
      await _contentManager.PublishAsync(eorMeasurementDevice);
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
    IUserService userService
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _recipeMigrator = recipeMigrator;
    _roleManager = roleManager;
    _contentManager = contentManager;
    _hostEnvironment = hostEnvironment;
    _session = session;
    _userService = userService;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IRecipeMigrator _recipeMigrator;
  private readonly RoleManager<IRole> _roleManager;
  private readonly IContentManager _contentManager;
  private readonly IHostEnvironment _hostEnvironment;
  private readonly ISession _session;
  private readonly IUserService _userService;
}
