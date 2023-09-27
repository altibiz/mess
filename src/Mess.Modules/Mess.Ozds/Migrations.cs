using Mess.OrchardCore.Extensions.OrchardCore;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using OrchardCore.Users.Services;
using Mess.OrchardCore;
using YesSql;
using OrchardCore.Title.Models;
using Mess.Fields.Abstractions.Extensions;
using Mess.Fields.Abstractions.Settings;
using Mess.Fields.Abstractions.Services;
using YesSql.Sql;
using Mess.Chart.Abstractions.Models;
using Mess.Ozds.Abstractions.Client;
using Mess.Fields.Abstractions;
using Mess.Iot.Abstractions.Indexes;

namespace Mess.Ozds;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    SchemaBuilder.CreateMapIndexTable<OzdsMeasurementDeviceClosedDistributionSystemIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("DeviceId", c => c.WithLength(64))
          .Column<bool>("IsMessenger", c => c.WithDefault(false))
          .Column<string>(
            "ClosedDistributionSystemContentItemId",
            c => c.WithLength(64)
          )
          .Column<string>(
            "ClosedDistributionSystemRepresentativeUserId",
            c => c.WithLength(64)
          )
    );
    SchemaBuilder.AlterIndexTable<OzdsMeasurementDeviceClosedDistributionSystemIndex>(table =>
    {
      table.CreateIndex(
        "IDX_OzdsMeasurementDeviceClosedDistributionSystemIndex_DeviceId",
        "DeviceId"
      );
      table.CreateIndex(
        "IDX_OzdsMeasurementDeviceClosedDistributionSystemIndex_RepresentativeUserId",
        "ClosedDistributionSystemRepresentativeUserId"
      );
    });

    SchemaBuilder.CreateMapIndexTable<OzdsMeasurementDeviceDistributionSystemOperatorIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("DeviceId", c => c.WithLength(64))
          .Column<bool>("IsMessenger")
          .Column<string>(
            "DistributionSystemOperatorContentItemId",
            c => c.WithLength(64)
          )
          .Column<string>(
            "DistributionSystemOperatorRepresentativeUserId",
            c => c.WithLength(64)
          )
    );
    SchemaBuilder.AlterIndexTable<OzdsMeasurementDeviceDistributionSystemOperatorIndex>(table =>
    {
      table.CreateIndex(
        "IDX_OzdsMeasurementDeviceDistributionSystemOperatorIndex_DeviceId",
        "DeviceId"
      );
      table.CreateIndex(
        "IDX_OzdsMeasurementDeviceDistributionSystemOperatorIndex_RepresentativeUserId",
        "DistributionSystemOperatorRepresentativeUserId"
      );
    });

    SchemaBuilder.CreateMapIndexTable<OzdsMeasurementDeviceDistributionSystemUnitIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("DeviceId", c => c.WithLength(64))
          .Column<bool>("IsMessenger")
          .Column<string>(
            "DistributionSystemUnitContentItemId",
            c => c.WithLength(64)
          )
          .Column<string>(
            "DistributionSystemUnitRepresentativeUserId",
            c => c.WithLength(64)
          )
    );
    SchemaBuilder.AlterIndexTable<OzdsMeasurementDeviceDistributionSystemUnitIndex>(table =>
    {
      table.CreateIndex(
        "IDX_OzdsMeasurementDeviceDistributionSystemUnitIndex_DeviceId",
        "DeviceId"
      );
      table.CreateIndex(
        "IDX_OzdsMeasurementDeviceDistributionSystemUnitIndex_RepresentativeUserId",
        "DistributionSystemUnitRepresentativeUserId"
      );
    });

    await _roleManager.CreateAsync(
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
        }
      }
    );

    await _roleManager.CreateAsync(
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
        }
      }
    );

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "DistributionSystemUnitRepresentative",
        RoleName = "Distribution System Unit Representative",
        RoleDescription = "Representative of a distribution system unit.",
      }
    );

    if (_hostEnvironment.IsDevelopment())
    {
      await _userService.CreateDevUserAsync(
        id: "DistributionSystemOperatorId",
        userName: "Operator",
        roleNames: "DistributionSystemOperatorRepresentative"
      );
      await _userService.CreateDevUserAsync(
        id: "ClosedDistributionSystemId",
        userName: "System",
        roleNames: "ClosedDistributionSystemRepresentative"
      );
      await _userService.CreateDevUserAsync(
        id: "DistributionSystemUnitId",
        userName: "Unit",
        roleNames: "DistributionSystemUnitRepresentative"
      );
    }

    _contentDefinitionManager.AlterPartDefinition(
      "DistributionSystemOperatorPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A distribution system operator.")
          .WithDisplayName("Distribution system operator")
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "DistributionSystemOperator",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Distribution system operator")
          .WithDescription("A distribution system operator.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription("Title of the distribution system operator.")
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.EditableRequired,
                  }
                )
          )
          .WithPart(
            "DistributionSystemOperatorPart",
            part =>
              part.WithDisplayName("Distribution system operator")
                .WithDescription("A distribution system operator.")
                .WithPosition("2")
          )
    );

    _contentDefinitionManager.AlterPartDefinition(
      "ClosedDistributionSystemPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A closed distribution system.")
          .WithDisplayName("Closed distribution system")
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "ClosedDistributionSystem",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Closed distribution system")
          .WithDescription("A closed distribution system.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription("Title of the closed distribution system.")
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.EditableRequired,
                  }
                )
          )
          .WithPart(
            "ClosedDistributionSystemPart",
            part =>
              part.WithDisplayName("Closed distribution system")
                .WithDescription("A closed distribution system.")
                .WithPosition("2")
          )
    );

    _contentDefinitionManager.AlterPartDefinition(
      "DistributionSystemOperatorPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A distribution system operator.")
          .WithDisplayName("Distribution system operator")
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "DistributionSystemOperator",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Distribution system operator")
          .WithDescription("A distribution system operator.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription("Title of the distribution system operator.")
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.EditableRequired,
                  }
                )
          )
          .WithPart(
            "DistributionSystemOperatorPart",
            part =>
              part.WithDisplayName("Distribution system operator")
                .WithDescription("A distribution system operator.")
                .WithPosition("2")
          )
    );

    _contentDefinitionManager.AlterPartDefinition(
      "PidgeonMeasurementDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A Pidgeon measurement device.")
          .WithDisplayName("Pidgeon measurement device")
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
      "PidgeonMeasurementDevice",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Pidgeon measurement device")
          .WithDescription("A Pidgeon measurement device.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription(
                  "Title displaying the identifier of the Pidgeon measurement device."
                )
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{%- ContentItem.Content.MeasurementDevicePart.DeviceId.Text -%}"
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
            "PidgeonMeasurementDevicePart",
            part =>
              part.WithDisplayName("Pidgeon measurement device")
                .WithDescription("A Pidgeon measurement device.")
                .WithPosition("3")
          )
    );

    if (_hostEnvironment.IsDevelopment())
    {
      var pidgeonMeasurementDevice =
        await _contentManager.NewContentAsync<PidgeonMeasurementDeviceItem>();
      pidgeonMeasurementDevice.Alter(
        pidgeonMeasurementDevice =>
          pidgeonMeasurementDevice.MeasurementDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "pidgeon" };
        }
      );
      pidgeonMeasurementDevice.Alter(
        pidgeonMeasurementDevice =>
          pidgeonMeasurementDevice.PidgeonMeasurementDevicePart,
        pidgeonMeasurementDevicePart =>
        {
          pidgeonMeasurementDevicePart.ApiKey =
            _apiKeyFieldService.HashApiKeyField("pidgeon");
        }
      );
      await _contentManager.CreateAsync(
        pidgeonMeasurementDevice,
        VersionOptions.Latest
      );
    }

    _contentDefinitionManager.AlterPartDefinition(
      "AbbMeasurementDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A Abb measurement device.")
          .WithDisplayName("Abb measurement device")
    );

    _contentDefinitionManager.AlterTypeDefinition(
      "AbbMeasurementDevice",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Abb measurement device")
          .WithDescription("An Abb measurement device.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription(
                  "Title displaying the identifier of the Abb measurement device."
                )
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{%- ContentItem.Content.AbbMeasurementDevicePart.DeviceId.Text -%}"
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
            "AbbMeasurementDevicePart",
            part =>
              part.WithDisplayName("Abb measurement device")
                .WithDescription("An Abb measurement device.")
                .WithPosition("3")
          )
          .WithPart(
            "ChartPart",
            part =>
              part.WithDisplayName("Chart")
                .WithDescription(
                  "Chart displaying the Abb measurement device data."
                )
                .WithPosition("4")
          )
    );

    if (_hostEnvironment.IsDevelopment())
    {
      var abbPowerDataset =
        await _contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
      abbPowerDataset.Alter(
        eguagePowerDataset => eguagePowerDataset.TimeseriesChartDatasetPart,
        timeseriesChartDatasetPart =>
        {
          timeseriesChartDatasetPart.Color = new() { Value = "#ff0000" };
          timeseriesChartDatasetPart.Label = new() { Text = "Power" };
          timeseriesChartDatasetPart.Property = nameof(
            AbbMeasurement.ActivePowerL1
          );
        }
      );
      var abbChart =
        await _contentManager.NewContentAsync<TimeseriesChartItem>();
      abbChart.Alter(
        abbChart => abbChart.TitlePart,
        titlePart =>
        {
          titlePart.Title = "Abb";
        }
      );
      abbChart.Alter(
        abbChart => abbChart.TimeseriesChartPart,
        timeseriesChartPart =>
        {
          timeseriesChartPart.ChartContentType = "AbbMeasurementDevice";
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
      await _contentManager.CreateAsync(abbChart, VersionOptions.Latest);

      var abbMeasurementDevice =
        (
          await _session
            .Query<ContentItem, MeasurementDeviceIndex>()
            .Where(index => index.DeviceId == "abb")
            .FirstOrDefaultAsync()
        )?.AsContent<AbbMeasurementDeviceItem>()
        ?? await _contentManager.NewContentAsync<AbbMeasurementDeviceItem>();
      abbMeasurementDevice.Alter(
        abbMeasurementDevice => abbMeasurementDevice.MeasurementDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "abb" };
        }
      );
      abbMeasurementDevice.Alter(
        abbMeasurementDevice => abbMeasurementDevice.ChartPart,
        chartPart =>
        {
          chartPart.ChartContentItemId = abbChart.ContentItemId;
        }
      );
      await _contentManager.CreateAsync(
        abbMeasurementDevice,
        VersionOptions.Latest
      );
    }

    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    IRecipeMigrator recipeMigrator,
    RoleManager<IRole> roleManager,
    IUserService userService,
    IHostEnvironment hostEnvironment,
    IContentManager contentManager,
    ISession session,
    IApiKeyFieldService apiKeyFieldService
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _recipeMigrator = recipeMigrator;
    _roleManager = roleManager;
    _userService = userService;
    _hostEnvironment = hostEnvironment;
    _contentManager = contentManager;
    _session = session;
    _apiKeyFieldService = apiKeyFieldService;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IRecipeMigrator _recipeMigrator;
  private readonly RoleManager<IRole> _roleManager;
  private readonly IUserService _userService;
  private readonly IHostEnvironment _hostEnvironment;
  private readonly IContentManager _contentManager;
  private readonly ISession _session;
  private readonly IApiKeyFieldService _apiKeyFieldService;
}
