using Mess.Eor.Abstractions.Indexes;
using Mess.Fields.Abstractions.Settings;
using Microsoft.AspNetCore.Identity;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;
using OrchardCore.Title.Models;
using YesSql.Sql;

namespace Mess.Eor;

public class Migrations : DataMigration
{
  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly RoleManager<IRole> _roleManager;

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    RoleManager<IRole> roleManager
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _roleManager = roleManager;
  }

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
                .WithSettings(
                  new ApiKeyFieldSettings
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
                .WithSettings(
                  new TitlePartSettings
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedHidden,
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
            ClaimValue = "ManageUsersInRole_EOR measurement device owner"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_EOR measurement device owner"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ViewOwn_EorIotDevice"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ControlOwn_EorIotDevice"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "PublishOwn_EorIotDevice"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "EditOwn_EorIotDevice"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "DeleteOwn_EorIotDevice"
          }
        }
      }
    );

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "EorIotDeviceOwner",
        RoleName = "EOR measurement device owner",
        RoleDescription = "Owner of an EOR measurement devices.",
        RoleClaims = new List<RoleClaim>
        {
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ViewOwned_EorIotDevice"
          },
          new()
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "ControlOwned_EorIotDevice"
          }
        }
      }
    );

    return 1;
  }
}
