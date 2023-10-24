using Mess.Ozds.Abstractions.Indexes;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using Mess.Fields.Abstractions.Settings;
using YesSql.Sql;
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
    await CreateAsyncMigrations.MigrateInvoice(_serviceProvider, SchemaBuilder);
    await CreateAsyncMigrations.MigrateReceipt(_serviceProvider, SchemaBuilder);
    await CreateAsyncMigrations.MigrateDevice(_serviceProvider, SchemaBuilder);
    await CreateAsyncMigrations.MigratePidgeon(_serviceProvider, SchemaBuilder);
    await CreateAsyncMigrations.MigrateAbb(_serviceProvider, SchemaBuilder);

    return 1;
  }

  public Migrations(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  private readonly IServiceProvider _serviceProvider;
}

internal static partial class CreateAsyncMigrations
{
  internal static async Task MigrateOperator(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

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

  internal static async Task MigrateSystem(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

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

  internal static async Task MigrateUnit(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
  )
  {
    var contentDefinitionManager =
      serviceProvider.GetRequiredService<IContentDefinitionManager>();

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

  internal static async Task MigratePidgeon(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
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
                .WithSettings<ApiKeyFieldSettings>(
                  new()
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
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{%- ContentItem.Content.IotDevicePart.DeviceId.Text -%}"
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
            "PidgeonIotDevicePart",
            part =>
              part.WithDisplayName("Pidgeon measurement device")
                .WithDescription("A Pidgeon measurement device.")
                .WithPosition("3")
          )
    );
  }

  internal static async Task MigrateAbb(
    IServiceProvider serviceProvider,
    ISchemaBuilder schemaBuilder
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
                .WithPosition("1")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedDisabled,
                    Pattern =
                      @"{%- ContentItem.Content.IotDevicePart.DeviceId.Text -%}"
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
            "OzdsIotDevicePart",
            part =>
              part.WithDisplayName("OZDS Measurement device")
                .WithDescription("An OZDS measurement device.")
                .WithPosition("3")
          )
          .WithPart(
            "AbbIotDevicePart",
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
            "IotDeviceFee",
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
