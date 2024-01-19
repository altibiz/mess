using Mess.Fields.Abstractions.Settings;
using Mess.Ozds.Abstractions.Indexes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Lists.Models;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using OrchardCore.Title.Models;
using YesSql.Sql;

namespace Mess.Ozds;

public class Migrations : DataMigration
{
  private readonly IServiceProvider _serviceProvider;

  public Migrations(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  public async Task<int> CreateAsync()
  {
    await CreateAsyncMigrations.MigrateRegulatoryAgencyCatalogue(
      _serviceProvider
    );
    await CreateAsyncMigrations.MigrateOperatorCatalogue(
      _serviceProvider,
      SchemaBuilder
    );
    await CreateAsyncMigrations.MigrateOperator(
      _serviceProvider
    );
    await CreateAsyncMigrations.MigrateSystem(_serviceProvider, SchemaBuilder);
    await CreateAsyncMigrations.MigrateUnit(_serviceProvider, SchemaBuilder);
    await CreateAsyncMigrations.MigrateInvoice(_serviceProvider);
    await CreateAsyncMigrations.MigrateReceipt(_serviceProvider);
    await CreateAsyncMigrations.MigrateDevice(_serviceProvider, SchemaBuilder);

    await CreateAsyncMigrations.MigratePidgeon(_serviceProvider);
    await CreateAsyncMigrations.MigrateAbb(_serviceProvider);
    await CreateAsyncMigrations.MigrateSchneider(_serviceProvider);

    return 1;
  }
}

internal static partial class CreateAsyncMigrations
{
  internal static async Task MigrateOperator(
    IServiceProvider serviceProvider
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();

    await roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "DistributionSystemOperatorRepresentative",
        RoleName = "Distribution System Operator Representative",
        RoleDescription =
          "Representative of closed distribution systems operator.",
        RoleClaims = new List<RoleClaim>
        {
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AccessAdminPanel"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "View Users"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue =
              "ManageUsersInRole_Closed distribution system representative"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_Closed distribution system representative"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "Listing payments"
          }
        }
      }
    );

    contentDefinitionManager.AlterPartDefinition(
      "DistributionSystemOperatorPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A distribution system operator.")
          .WithDisplayName("Distribution system operator")
          .WithField(
            "RegulatoryAgencyCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("Regulatory agency catalogue")
                .WithDescription("Regulatory agency catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[]
                    {
                      "RegulatoryAgencyCatalogue"
                    },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "WhiteHighVoltageOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("White high voltage operator catalogue")
                .WithDescription("White high voltage operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "WhiteMediumVoltageOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("White medium voltage operator catalogue")
                .WithDescription("White medium voltage operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "BlueOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("Blue operator catalogue")
                .WithDescription("Blue operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "WhiteLowVoltageOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("White low voltage operator catalogue")
                .WithDescription("White low voltage operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "RedOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("Red operator catalogue")
                .WithDescription("Red operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "YellowOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("Yellow operator catalogue")
                .WithDescription("Yellow operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.EditableRequired
                  }
                )
          )
          .WithPart(
            "DistributionSystemOperatorPart",
            part =>
              part.WithDisplayName("Distribution system operator")
                .WithDescription("A distribution system operator.")
          )
          .WithPart(
            "LegalEntityPart",
            part =>
              part.WithDisplayName("Legal entity")
                .WithDescription(
                  "Identification, contact and address information the distributed system operator."
                )
          )
          .WithPart(
            "ListPart",
            part =>
              part.WithDisplayName("Closed distribution systems")
                .WithDescription("List of owned closed distribution systems.")
                .WithSettings(
                  new ListPartSettings
                  {
                    ContainedContentTypes = new[]
                    {
                      "ClosedDistributionSystem"
                    }
                  }
                )
          )
    );
  }

  internal static async Task MigrateSystem(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();

    await roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "ClosedDistributionSystemRepresentative",
        RoleName = "Closed Distribution System Representative",
        RoleDescription = "Representative of a closed distribution system.",
        RoleClaims = new List<RoleClaim>
        {
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AccessAdminPanel"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "View Users"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue =
              "ManageUsersInRole_Distribution system unit representative"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_Distribution system unit representative"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "Listing own payments"
          }
        }
      }
    );

    contentDefinitionManager.AlterPartDefinition(
      "ClosedDistributionSystemPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A closed distribution system.")
          .WithDisplayName("Closed distribution system")
          .WithField(
            "WhiteHighVoltageOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("White high voltage operator catalogue")
                .WithDescription("White high voltage operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "WhiteMediumVoltageOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("White medium voltage operator catalogue")
                .WithDescription("White medium voltage operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "BlueOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("Blue operator catalogue")
                .WithDescription("Blue operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "WhiteLowVoltageOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("White low voltage operator catalogue")
                .WithDescription("White low voltage operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "RedOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("Red operator catalogue")
                .WithDescription("Red operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
                  }
                )
          )
          .WithField(
            "YellowOperatorCatalogue",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("Yellow operator catalogue")
                .WithDescription("Yellow operator catalogue.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "OperatorCatalogue" },
                    DisplayAllContentTypes = false,
                    Multiple = false,
                    Required = true
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.EditableRequired
                  }
                )
          )
          .WithPart(
            "ClosedDistributionSystemPart",
            part =>
              part.WithDisplayName("Closed distribution system")
                .WithDescription("A closed distribution system.")
          )
          .WithPart(
            "LegalEntityPart",
            part =>
              part.WithDisplayName("Legal entity")
                .WithDescription(
                  "Identification, contact and address information the closed distributed system."
                )
          )
          .WithPart(
            "ListPart",
            part =>
              part.WithDisplayName("Distribution system units")
                .WithDescription(
                  "List of units in this closed distribution system."
                )
                .WithSettings(
                  new ListPartSettings
                  {
                    ContainedContentTypes = new[] { "DistributionSystemUnit" }
                  }
                )
          )
    );

    schemaBuilder.CreateMapIndexTable<ClosedDistributionSystemIndex>(
      table =>
        table
          .Column<string>(
            "ClosedDistributionSystemContentItemId",
            c => c.WithLength(64)
          )
          .Column<string>(
            "DistributionSystemOperatorContentItemId",
            c => c.WithLength(64)
          )
    );
    schemaBuilder.AlterIndexTable<ClosedDistributionSystemIndex>(table =>
    {
      table.CreateIndex(
        "IDX_ClosedDistributionSystemIndex_CloseDistributionSystemContentItemId",
        "ClosedDistributionSystemContentItemId"
      );
      table.CreateIndex(
        "IDX_ClosedDistributionSystemIndex_DistributionSystemOperatorContentItemId",
        "DistributionSystemOperatorContentItemId"
      );
    });
  }

  internal static async Task MigrateUnit(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IRole>>();

    await roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "DistributionSystemUnitRepresentative",
        RoleName = "Distribution System Unit Representative",
        RoleDescription = "Representative of a distribution system unit.",
        RoleClaims = new List<RoleClaim>
        {
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "Listing own payments"
          }
        }
      }
    );

    contentDefinitionManager.AlterPartDefinition(
      "DistributionSystemUnitPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A distribution system unit.")
          .WithDisplayName("Distribution system unit")
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.EditableRequired
                  }
                )
          )
          .WithPart(
            "DistributionSystemUnitPart",
            part =>
              part.WithDisplayName("Distribution system unit")
                .WithDescription("A distribution system unit.")
          )
          .WithPart(
            "LegalEntityPart",
            part =>
              part.WithDisplayName("Legal entity")
                .WithDescription(
                  "Identification, contact and address information the distributed system unit."
                )
          )
          .WithPart(
            "ListPart",
            part =>
              part.WithDisplayName("IOT Devices")
                .WithDescription(
                  "List of IOT devices in this distribution system unit."
                )
                .WithSettings(
                  new ListPartSettings
                  {
                    ContainedContentTypes = new[]
                    {
                      "AbbIotDevice",
                      "SchneiderIotDevice",
                      "PidgeonIotDevice"
                    }
                  }
                )
          )
          .WithPart(
            "BillingPart",
            part =>
              part.WithDisplayName("Billing")
                .WithDescription("Billing information.")
          )
    );

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
  }

  internal static async Task MigratePidgeon(
    IServiceProvider serviceProvider
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "PidgeonIotDevicePart",
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
                .WithSettings(
                  new ApiKeyFieldSettings
                  {
                    Hint = "API key that will be used to authorize the device."
                  }
                )
          )
    );

    contentDefinitionManager.AlterTypeDefinition(
      "PidgeonIotDevice",
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{{ ContentItem.Content.IotDevicePart.DeviceId.Text }}"
                  }
                )
          )
          .WithPart(
            "IotDevicePart",
            part =>
              part.WithDisplayName("Measurement device")
                .WithDescription("A measurement device.")
          )
          .WithPart(
            "PidgeonIotDevicePart",
            part =>
              part.WithDisplayName("Pidgeon measurement device")
                .WithDescription("A Pidgeon measurement device.")
          )
    );
  }

  internal static async Task MigrateAbb(
    IServiceProvider serviceProvider
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "AbbIotDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A Abb measurement device.")
          .WithDisplayName("Abb measurement device")
    );

    contentDefinitionManager.AlterTypeDefinition(
      "AbbIotDevice",
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{{ ContentItem.Content.IotDevicePart.DeviceId.Text }}"
                  }
                )
          )
          .WithPart(
            "IotDevicePart",
            part =>
              part.WithDisplayName("Measurement device")
                .WithDescription("A measurement device.")
          )
          .WithPart(
            "OzdsIotDevicePart",
            part =>
              part.WithDisplayName("OZDS Measurement device")
                .WithDescription("An OZDS measurement device.")
          )
          .WithPart(
            "AbbIotDevicePart",
            part =>
              part.WithDisplayName("Abb measurement device")
                .WithDescription("An Abb measurement device.")
          )
          .WithPart(
            "ChartPart",
            part =>
              part.WithDisplayName("Chart")
                .WithDescription(
                  "Chart displaying the Abb measurement device data."
                )
          )
    );
  }

  internal static async Task MigrateSchneider(
    IServiceProvider serviceProvider
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "SchneiderIotDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A Schneider measurement device.")
          .WithDisplayName("Schneider measurement device")
    );

    contentDefinitionManager.AlterTypeDefinition(
      "SchneiderIotDevice",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Schneider measurement device")
          .WithDescription("An Schneider measurement device.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription(
                  "Title displaying the identifier of the Schneider measurement device."
                )
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{{ ContentItem.Content.IotDevicePart.DeviceId.Text }}"
                  }
                )
          )
          .WithPart(
            "IotDevicePart",
            part =>
              part.WithDisplayName("Measurement device")
                .WithDescription("A measurement device.")
          )
          .WithPart(
            "OzdsIotDevicePart",
            part =>
              part.WithDisplayName("OZDS Measurement device")
                .WithDescription("An OZDS measurement device.")
          )
          .WithPart(
            "SchneiderIotDevicePart",
            part =>
              part.WithDisplayName("Schneider measurement device")
                .WithDescription("An Schneider measurement device.")
          )
          .WithPart(
            "ChartPart",
            part =>
              part.WithDisplayName("Chart")
                .WithDescription(
                  "Chart displaying the Schneider measurement device data."
                )
          )
    );
  }

  internal static async Task MigrateRegulatoryAgencyCatalogue(
    IServiceProvider serviceProvider
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
          .WithField(
            "RenewableEnergyFee",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Renewable energy fee")
                .WithDescription("Renewable energy fee.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Hint = "Renewable energy fee.",
                    Required = true,
                    Minimum = 0.0M,
                    Scale = 6
                  }
                )
          )
          .WithField(
            "BusinessUsageFee",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Business usage fee")
                .WithDescription("Business usage fee.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Hint = "Business usage fee.",
                    Required = true,
                    Minimum = 0.0M,
                    Scale = 6
                  }
                )
          )
          .WithField(
            "TaxRate",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Tax rate")
                .WithDescription("Tax rate.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Hint = "Tax rate.",
                    Required = true,
                    Minimum = 0.0M,
                    Scale = 6
                  }
                )
          )
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.EditableRequired
                  }
                )
          )
          .WithPart(
            "RegulatoryAgencyCataloguePart",
            part =>
              part.WithDisplayName("Regulatory agency catalogue")
                .WithDescription("A regulatory agency catalogue.")
          )
    );
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
                .WithSettings(
                  new TextFieldSettings { Hint = "Voltage.", Required = true }
                )
          )
          .WithField(
            "Model",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Model")
                .WithDescription("Model.")
                .WithSettings(
                  new TextFieldSettings { Hint = "Model.", Required = true }
                )
          )
          .WithField(
            "EnergyPrice",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Energy price")
                .WithDescription("Energy price.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Hint = "Energy price.",
                    Required = true,
                    Minimum = 0.0M,
                    Scale = 6
                  }
                )
          )
          .WithField(
            "HighEnergyPrice",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("High energy price")
                .WithDescription("High energy price.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Hint = "High energy price.",
                    Required = true,
                    Minimum = 0.0M,
                    Scale = 6
                  }
                )
          )
          .WithField(
            "LowEnergyPrice",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Low energy price")
                .WithDescription("Low energy price.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Hint = "Low energy price.",
                    Required = true,
                    Minimum = 0.0M,
                    Scale = 6
                  }
                )
          )
          .WithField(
            "MaxPowerPrice",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Max power price")
                .WithDescription("Max power price.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Hint = "Max power price.",
                    Required = true,
                    Minimum = 0.0M,
                    Scale = 6
                  }
                )
          )
          .WithField(
            "IotDeviceFee",
            fieldBuilder =>
              fieldBuilder
                .OfType("NumericField")
                .WithDisplayName("Measurement device fee")
                .WithDescription("Measurement device fee.")
                .WithSettings(
                  new NumericFieldSettings
                  {
                    Hint = "Measurement device fee.",
                    Required = true,
                    Minimum = 0.0M,
                    Scale = 6
                  }
                )
          )
    );

    contentDefinitionManager.AlterTypeDefinition(
      "OperatorCatalogue",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Draftable()
          .Securable()
          .DisplayedAs("Operator catalogue")
          .WithDescription("Operator catalogue.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription(
                  "Title displaying the identifier of the operator catalogue."
                )
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.EditableRequired
                  }
                )
          )
          .WithPart(
            "OperatorCataloguePart",
            part =>
              part.WithDisplayName("Operator catalogue")
                .WithDescription("An operator catalogue.")
          )
    );

    schemaBuilder.CreateMapIndexTable<OperatorCatalogueIndex>(
      builder =>
        builder
          .Column<string>(
            "OperatorCatalogueContentItemId",
            c => c.WithLength(64)
          )
          .Column<string>("Voltage", c => c.WithLength(64))
          .Column<string>("Model", c => c.WithLength(64))
    );

    schemaBuilder.AlterIndexTable<OperatorCatalogueIndex>(table =>
    {
      table.CreateIndex(
        "IDX_OperatorCatalogueIndex_OperatorCatalogueContentItemId",
        "OperatorCatalogueContentItemId"
      );

      table.CreateIndex("IDX_OperatorCatalogueIndex_Voltage", "Voltage");

      table.CreateIndex("IDX_OperatorCatalogueIndex_Model", "Model");
    });
  }

  internal static async Task MigrateInvoice(
    IServiceProvider serviceProvider
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled
                  }
                )
          )
          .WithPart(
            "InvoicePart",
            part =>
              part.WithDisplayName("Invoice").WithDescription("An invoice")
          )
          .WithPart(
            "OzdsCalculationPart",
            part =>
              part.WithDisplayName("OZDS calculation")
                .WithDescription("An OZDS calculation.")
          )
          .WithPart(
            "OzdsInvoicePart",
            part =>
              part.WithDisplayName("OZDS Invoice")
                .WithDescription("An OZDS invoice.")
          )
    );
  }

  internal static async Task MigrateReceipt(
    IServiceProvider serviceProvider
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled
                  }
                )
          )
          .WithPart(
            "ReceiptPart",
            part =>
              part.WithDisplayName("Receipt").WithDescription("An receipt")
          )
          .WithPart(
            "OzdsCalculationPart",
            part =>
              part.WithDisplayName("OZDS calculation")
                .WithDescription("An OZDS calculation.")
          )
          .WithPart(
            "OzdsReceiptPart",
            part =>
              part.WithDisplayName("OZDS Receipt")
                .WithDescription("An OZDS receipt.")
          )
    );
  }

  internal static async Task MigrateDevice(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

    contentDefinitionManager.AlterPartDefinition(
      "OzdsIotDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An OZDS measurement device.")
          .WithDisplayName("OZDS measurement device")
    );

    contentDefinitionManager.AlterPartDefinition(
      "OzdsCalculationPart",
      builder =>
        builder
          .Attachable()
          .WithDescription("An OZDS billing calculation.")
          .WithDisplayName("OZDS billing calculation")
    );

    schemaBuilder.CreateMapIndexTable<OzdsIotDeviceIndex>(
      builder =>
        builder
          .Column<string>("OzdsIotDeviceContentItemId", c => c.WithLength(64))
          .Column<string>("DeviceId", c => c.WithLength(64))
          .Column<bool>("IsMessenger")
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

    schemaBuilder.AlterIndexTable<OzdsIotDeviceIndex>(table =>
    {
      table.CreateIndex(
        "IDX_OzdsIotDeviceIndex_OzdsIotDeviceContentItemId",
        "OzdsIotDeviceContentItemId"
      );

      table.CreateIndex("IDX_OzdsIotDeviceIndex_DeviceId", "DeviceId");

      table.CreateIndex("IDX_OzdsIotDeviceIndex_IsMessenger", "IsMessenger");

      table.CreateIndex(
        "IDX_OzdsIotDeviceIndex_DistributionSystemUnitContentItemId",
        "DistributionSystemUnitContentItemId"
      );

      table.CreateIndex(
        "IDX_OzdsIotDeviceIndex_ClosedDistributionSystemContentItemId",
        "ClosedDistributionSystemContentItemId"
      );

      table.CreateIndex(
        "IDX_OzdsIotDeviceIndex_DistributionSystemOperatorContentItemId",
        "DistributionSystemOperatorContentItemId"
      );
    });
  }
}
