using Mess.Iot.Abstractions.Indexes;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using YesSql.Sql;

namespace Mess.Iot;

public class Migrations : DataMigration
{
  private readonly IContentDefinitionManager _contentDefinitionManager;

  public Migrations(IContentDefinitionManager contentDefinitionManager)
  {
    _contentDefinitionManager = contentDefinitionManager;
  }

  public async Task<int> CreateAsync()
  {
    _contentDefinitionManager.AlterPartDefinition(
      "IotDevicePart",
      builder =>
        builder
          .Attachable()
          .WithDescription("A measurement device.")
          .WithDisplayName("Measurement device")
          .WithField(
            "DeviceId",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Device identifier")
                .WithDescription("The identifier of the measurement device.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Hint = "The identifier of the measurement device."
                  }
                )
          )
    );

    SchemaBuilder.CreateMapIndexTable<IotDeviceIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("DeviceId", c => c.WithLength(64))
          .Column<bool>("IsMessenger")
    );
    SchemaBuilder.AlterIndexTable<IotDeviceIndex>(
      table => table.CreateIndex("IDX_IotDeviceIndex_DeviceId", "DeviceId")
    );

    return 1;
  }
}
