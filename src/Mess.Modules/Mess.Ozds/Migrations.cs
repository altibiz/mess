using Mess.OrchardCore.Extensions.OrchardCore;
using Mess.Ozds.Abstractions.Indexes;
using Mess.Ozds.Abstractions.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using OrchardCore.Users.Services;
using Mess.OrchardCore;
using OrchardCore.Title.Models;
using Mess.Fields.Abstractions.Extensions;
using Mess.Fields.Abstractions.Settings;
using Mess.Fields.Abstractions.Services;
using YesSql.Sql;
using Mess.Chart.Abstractions.Models;
using Mess.Ozds.Abstractions.Client;
using Mess.Fields.Abstractions;
using OrchardCore.ContentFields.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Mess.Ozds;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    await CreateAsyncMigrations.MigrateRegulatoryAgencyCatalogue(
      _serviceProvider,
      SchemaBuilder
    );

    await CreateAsyncMigrations.MigrateOperatorCatalogue(
      _serviceProvider,
      SchemaBuilder
    );

    await CreateAsyncMigrations.MigrateOperator(
      _serviceProvider,
      SchemaBuilder
    );

    await CreateAsyncMigrations.MigrateSystem(_serviceProvider, SchemaBuilder);

    await CreateAsyncMigrations.MigrateUnit(_serviceProvider, SchemaBuilder);

    var contentDefinitionManager =
      _serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "OzdsMeasurementDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An OZDS measurement device.")
          .WithDisplayName("OZDS measurement device")
          .WithField(
            "DistributionSystemUnit",
            fieldBuilder =>
              fieldBuilder
                .OfType("ContentPickerField")
                .WithDisplayName("Distribution system unit")
                .WithDescription("Distribution system unit.")
                .WithSettings<ContentPickerFieldSettings>(
                  new()
                  {
                    Hint = "Distribution system unit.",
                    Multiple = false,
                    Required = true,
                    DisplayedContentTypes = new[] { "DistributionSystemUnit" },
                    DisplayAllContentTypes = false
                  }
                )
          )
    );

    contentDefinitionManager.AlterPartDefinition(
      "OzdsCalculationPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An OZDS billing calculation.")
          .WithDisplayName("OZDS billing calculation")
    );

    await CreateAsyncMigrations.MigrateInvoice(_serviceProvider, SchemaBuilder);

    await CreateAsyncMigrations.MigrateReceipt(_serviceProvider, SchemaBuilder);

    await CreateAsyncMigrations.MigratePidgeon(_serviceProvider, SchemaBuilder);

    await CreateAsyncMigrations.MigrateAbb(_serviceProvider, SchemaBuilder);

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

    (
      string whiteHighVoltageMeasurementDeviceCatalogueContentItemId,
      string whiteMediumVoltageMeasurementDeviceCatalogueContentItemId,
      string blueMeasurementDeviceCatalogueContentItemId,
      string whiteLowVoltageMeasurementDeviceCatalogueContentItemId,
      string redMeasurementDeviceCatalogueContentItemId,
      string yellowMeasurementDeviceCatalogueContentItemId
    ) = await CreateAsyncMigrations.PopulateOperatorCatalogues(
      _serviceProvider
    );

    (string? operatorUserId, string? operatorContentItemId) =
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

    (string? systemUserId, string? systemContentItemId) =
      await CreateAsyncMigrations.PopulateSystem(
        _serviceProvider,
        operatorUserId!,
        operatorContentItemId!
      );

    (string? unitUserId, string? unitContentItemId) =
      await CreateAsyncMigrations.PopulateUnit(
        _serviceProvider,
        operatorUserId!,
        operatorContentItemId!,
        systemUserId!,
        systemContentItemId!
      );

    await CreateAsyncMigrations.PopulatePidgeon(
      _serviceProvider,
      unitContentItemId!
    );

    await CreateAsyncMigrations.PopulateAbb(
      _serviceProvider,
      unitContentItemId!
    );

    return 1;
  }

  public Migrations(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}

internal static class CreateAsyncMigrations
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

          distributionSystemOperatorPart.WhiteHighVoltageOperatorCatalogueContentItemId =
            new()
            {
              ContentItemIds = new[]
              {
                whiteHighVoltageOperatorCatalogueContentItemId
              }
            };

          distributionSystemOperatorPart.WhiteMediumVoltageOperatorCatalogueContentItemId =
            new()
            {
              ContentItemIds = new[]
              {
                whiteMediumVoltageOperatorCatalogueContentItemId
              }
            };

          distributionSystemOperatorPart.BlueOperatorCatalogueContentItemId =
            new()
            {
              ContentItemIds = new[] { blueOperatorCatalogueContentItemId }
            };

          distributionSystemOperatorPart.WhiteLowVoltageOperatorCatalogueContentItemId =
            new()
            {
              ContentItemIds = new[]
              {
                whiteLowVoltageOperatorCatalogueContentItemId
              }
            };

          distributionSystemOperatorPart.RedOperatorCatalogueContentItemId =
            new()
            {
              ContentItemIds = new[] { redOperatorCatalogueContentItemId }
            };

          distributionSystemOperatorPart.YellowOperatorCatalogueContentItemId =
            new()
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

  internal static async Task MigrateOperator(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    schemaBuilder.CreateMapIndexTable<OzdsMeasurementDeviceDistributionSystemOperatorIndex>(
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
    schemaBuilder.AlterIndexTable<OzdsMeasurementDeviceDistributionSystemOperatorIndex>(table =>
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

    contentDefinitionManager.AlterPartDefinition(
      "DistributionSystemOperatorPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A distribution system operator.")
          .WithDisplayName("Distribution system operator")
    );

    contentDefinitionManager.AlterTypeDefinition(
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
          .WithPart(
            "LegalEntityPart",
            part =>
              part.WithDisplayName("Legal entity")
                .WithDescription(
                  "Identification, contact and address information the distributed system operator."
                )
                .WithPosition("3")
          )
    );
  }

  internal static async Task<(
    string? UserId,
    string? ContentItemId
  )> PopulateSystem(
    IServiceProvider serviceProvider,
    string operatorUserId,
    string operatorContentItemId
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

  internal static async Task MigrateSystem(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    schemaBuilder.CreateMapIndexTable<OzdsMeasurementDeviceClosedDistributionSystemIndex>(
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
    schemaBuilder.AlterIndexTable<OzdsMeasurementDeviceClosedDistributionSystemIndex>(table =>
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

    contentDefinitionManager.AlterPartDefinition(
      "ClosedDistributionSystemPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A closed distribution system.")
          .WithDisplayName("Closed distribution system")
          .WithField(
            "DistributionSystemOperator",
            fieldBuilder =>
              fieldBuilder
                .OfType("ContentPickerField")
                .WithDisplayName("Distribution system operator")
                .WithDescription("Distribution system operator.")
                .WithSettings<ContentPickerFieldSettings>(
                  new()
                  {
                    Hint = "Distribution system operator.",
                    Multiple = false,
                    Required = true,
                    DisplayedContentTypes = new[]
                    {
                      "DistributionSystemOperator"
                    },
                    DisplayAllContentTypes = false
                  }
                )
          )
    );

    contentDefinitionManager.AlterTypeDefinition(
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
          .WithPart(
            "LegalEntityPart",
            part =>
              part.WithDisplayName("Legal entity")
                .WithDescription(
                  "Identification, contact and address information the closed distributed system."
                )
                .WithPosition("3")
          )
    );
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

  internal static async Task MigrateUnit(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    schemaBuilder.CreateMapIndexTable<OzdsMeasurementDeviceDistributionSystemUnitIndex>(
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
    schemaBuilder.AlterIndexTable<OzdsMeasurementDeviceDistributionSystemUnitIndex>(table =>
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

    schemaBuilder.CreateMapIndexTable<DistributionSystemUnitIndex>(
      table =>
        table
          .Column<string>(
            "DistributionSystemUnitContentItemId",
            c => c.WithLength(64)
          )
          .Column<string>(
            "ClosedDistributionSystemContentItemId",
            c => c.WithLength(64)
          )
          .Column<string>(
            "DistributionSystemOperatorContentItemId",
            c => c.WithLength(64)
          )
    );
    schemaBuilder.AlterIndexTable<DistributionSystemUnitIndex>(table =>
    {
      table.CreateIndex(
        "IDX_DistributionSystemUnitIndex_DistributionSystemUnitContentItemId",
        "DistributionSystemUnitContentItemId"
      );
      table.CreateIndex(
        "IDX_DistributionSystemUnitIndex_ClosedDistributionSystemContentItemId",
        "ClosedDistributionSystemContentItemId"
      );
      table.CreateIndex(
        "IDX_DistributionSystemUnitIndex_DistributionSystemOperatorContentItemId",
        "DistributionSystemOperatorContentItemId"
      );
    });

    contentDefinitionManager.AlterPartDefinition(
      "DistributionSystemUnitPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A distribution system unit.")
          .WithDisplayName("Distribution system unit")
          .WithField(
            "ClosedDistributionSystem",
            fieldBuilder =>
              fieldBuilder
                .OfType("ContentPickerField")
                .WithDisplayName("Closed distribution system")
                .WithDescription("Closed distribution system.")
                .WithSettings<ContentPickerFieldSettings>(
                  new()
                  {
                    Hint = "Closed distribution system.",
                    Multiple = false,
                    Required = true,
                    DisplayedContentTypes = new[]
                    {
                      "ClosedDistributionSystem"
                    },
                    DisplayAllContentTypes = false
                  }
                )
          )
    );

    contentDefinitionManager.AlterTypeDefinition(
      "DistributionSystemUnit",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Distribution system unit")
          .WithDescription("A distribution system unit.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription("Title of the distribution system unit.")
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
            "DistributionSystemUnitPart",
            part =>
              part.WithDisplayName("Distribution system unit")
                .WithDescription("A distribution system unit.")
                .WithPosition("2")
          )
          .WithPart(
            "LegalEntityPart",
            part =>
              part.WithDisplayName("Legal entity")
                .WithDescription(
                  "Identification, contact and address information the distributed system unit."
                )
                .WithPosition("3")
          )
    );
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
      var pidgeonMeasurementDevice =
        await contentManager.NewContentAsync<PidgeonMeasurementDeviceItem>();
      pidgeonMeasurementDevice.Alter(
        pidgeonMeasurementDevice => pidgeonMeasurementDevice.TitlePart,
        titlePart =>
        {
          titlePart.Title = "pidgeon";
        }
      );
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
            apiKeyFieldService.HashApiKeyField("pidgeon");
        }
      );
      pidgeonMeasurementDevice.Alter(
        pidgeonMeasurementDevice =>
          pidgeonMeasurementDevice.OzdsMeasurementDevicePart,
        ozdsMeasurementDevicePart =>
        {
          ozdsMeasurementDevicePart.DistributionSystemUnit = new()
          {
            ContentItemIds = new[] { unitContentItemId }
          };
        }
      );
      await contentManager.CreateAsync(
        pidgeonMeasurementDevice,
        VersionOptions.Latest
      );
    }
  }

  internal static async Task MigratePidgeon(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
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

    contentDefinitionManager.AlterTypeDefinition(
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
  }

  internal static async Task PopulateAbb(
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
      var abbPowerDataset =
        await contentManager.NewContentAsync<TimeseriesChartDatasetItem>();
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
      await contentManager.CreateAsync(abbChart, VersionOptions.Latest);

      var abbMeasurementDevice =
        await contentManager.NewContentAsync<AbbMeasurementDeviceItem>();
      abbMeasurementDevice.Alter(
        abbMeasurementDevice => abbMeasurementDevice.TitlePart,
        titlePart =>
        {
          titlePart.Title = "Abb";
        }
      );
      abbMeasurementDevice.Alter(
        abbMeasurementDevice => abbMeasurementDevice.MeasurementDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "abb" };
        }
      );
      abbMeasurementDevice.Alter(
        abbMeasurementDevice => abbMeasurementDevice.OzdsMeasurementDevicePart,
        ozdsMeasurementDevicePart =>
        {
          ozdsMeasurementDevicePart.DistributionSystemUnit = new()
          {
            ContentItemIds = new[] { unitContentItemId }
          };
        }
      );
      abbMeasurementDevice.Alter(
        abbMeasurementDevice => abbMeasurementDevice.ChartPart,
        chartPart =>
        {
          chartPart.ChartContentItemId = abbChart.ContentItemId;
        }
      );
      await contentManager.CreateAsync(
        abbMeasurementDevice,
        VersionOptions.Latest
      );
    }
  }

  internal static async Task MigrateAbb(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "AbbMeasurementDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A Abb measurement device.")
          .WithDisplayName("Abb measurement device")
    );

    contentDefinitionManager.AlterTypeDefinition(
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
            "OzdsMeasurementDevicePart",
            part =>
              part.WithDisplayName("OZDS Measurement device")
                .WithDescription("An OZDS measurement device.")
                .WithPosition("3")
          )
          .WithPart(
            "AbbMeasurementDevicePart",
            part =>
              part.WithDisplayName("Abb measurement device")
                .WithDescription("An Abb measurement device.")
                .WithPosition("4")
          )
          .WithPart(
            "ChartPart",
            part =>
              part.WithDisplayName("Chart")
                .WithDescription(
                  "Chart displaying the Abb measurement device data."
                )
                .WithPosition("5")
          )
          .WithPart(
            "BillingPart",
            part =>
              part.WithDisplayName("Billing")
                .WithDescription("Billing information.")
                .WithPosition("6")
          )
    );
  }

  internal static async Task MigrateRegulatoryAgencyCatalogue(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "RegulatoryAgencyCataloguePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A regulatory agency catalogue.")
          .WithDisplayName("Regulatory agency catalogue")
    );

    contentDefinitionManager.AlterTypeDefinition(
      "RegulatoryAgencyCatalogue",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Regulatory agency catalogue")
          .WithDescription("A regulatory agency catalogue.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription(
                  "Title displaying the identifier of the regulatory agency catalogue."
                )
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{%- ContentItem.Content.RegulatoryAgencyCataloguePart.RegulatoryAgencyCatalogueId.Text -%}"
                  }
                )
          )
          .WithPart(
            "RegulatoryAgencyCataloguePart",
            part =>
              part.WithDisplayName("Regulatory agency catalogue")
                .WithDescription("A regulatory agency catalogue.")
                .WithPosition("2")
          )
    );
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

  internal static async Task MigrateOperatorCatalogue(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "OperatorCataloguePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A operator catalogue.")
          .WithDisplayName("Operator catalogue")
          .WithField(
            "Voltage",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Voltage")
                .WithDescription("Voltage.")
                .WithSettings<TextFieldSettings>(
                  new() { Hint = "Voltage.", Required = true, }
                )
          )
          .WithField(
            "Model",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Model")
                .WithDescription("Model.")
                .WithSettings<TextFieldSettings>(
                  new() { Hint = "Model.", Required = true, }
                )
          )
          .WithField(
            "EnergyPrice",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Energy price")
                .WithDescription("Energy price.")
                .WithSettings<NumericFieldSettings>(
                  new() { Hint = "Energy price.", }
                )
          )
          .WithField(
            "HighEnergyPrice",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("High energy price")
                .WithDescription("High energy price.")
                .WithSettings<NumericFieldSettings>(
                  new() { Hint = "High energy price.", }
                )
          )
          .WithField(
            "LowEnergyPrice",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Low energy price")
                .WithDescription("Low energy price.")
                .WithSettings<NumericFieldSettings>(
                  new() { Hint = "Low energy price.", }
                )
          )
          .WithField(
            "MaxPowerPrice",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Max power price")
                .WithDescription("Max power price.")
                .WithSettings<NumericFieldSettings>(
                  new() { Hint = "Max power price.", }
                )
          )
          .WithField(
            "MeasurementDeviceFee",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Measurement device fee")
                .WithDescription("Measurement device fee.")
                .WithSettings<NumericFieldSettings>(
                  new() { Hint = "Measurement device fee.", }
                )
          )
    );
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
        operatorCataloguePart.MeasurementDeviceFee = new() { Value = 68.00M };
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
        operatorCataloguePart.MeasurementDeviceFee = new() { Value = 66.00M };
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
        operatorCataloguePart.MeasurementDeviceFee = new() { Value = 41.30M };
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
        operatorCataloguePart.MeasurementDeviceFee = new() { Value = 41.30M };
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
        operatorCataloguePart.MeasurementDeviceFee = new() { Value = 41.30M };
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
        operatorCataloguePart.MeasurementDeviceFee = new() { Value = 15.45M };
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

  internal static async Task MigrateInvoice(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "OzdsInvoicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An OZDS invoice.")
          .WithDisplayName("OZDS invoice")
    );
    contentDefinitionManager.AlterTypeDefinition(
      "OzdsInvoice",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("OZDS invoice")
          .WithDescription("An OZDS invoice.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription("Title.")
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                  }
                )
          )
          .WithPart(
            "InvoicePart",
            part =>
              part.WithDisplayName("Invoice")
                .WithDescription("An invoice")
                .WithPosition("2")
          )
          .WithPart(
            "OzdsCalculationPart",
            part =>
              part.WithDisplayName("OZDS calculation")
                .WithDescription("An OZDS calculation.")
                .WithPosition("3")
          )
          .WithPart(
            "OzdsInvoicePart",
            part =>
              part.WithDisplayName("OZDS Invoice")
                .WithDescription("An OZDS invoice.")
                .WithPosition("4")
          )
    );
  }

  internal static async Task MigrateReceipt(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "OzdsReceiptPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An OZDS receipt.")
          .WithDisplayName("OZDS receipt")
    );
    contentDefinitionManager.AlterTypeDefinition(
      "OzdsReceipt",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("OZDS receipt")
          .WithDescription("An OZDS receipt.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription("Title.")
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                  }
                )
          )
          .WithPart(
            "ReceiptPart",
            part =>
              part.WithDisplayName("Receipt")
                .WithDescription("An receipt")
                .WithPosition("2")
          )
          .WithPart(
            "OzdsCalculationPart",
            part =>
              part.WithDisplayName("OZDS calculation")
                .WithDescription("An OZDS calculation.")
                .WithPosition("3")
          )
          .WithPart(
            "OzdsReceiptPart",
            part =>
              part.WithDisplayName("OZDS Receipt")
                .WithDescription("An OZDS receipt.")
                .WithPosition("4")
          )
    );
  }
}
