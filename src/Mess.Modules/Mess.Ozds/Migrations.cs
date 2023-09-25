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
using Mess.ContentFields.Abstractions.Extensions;
using Mess.ContentFields.Abstractions.Settings;
using Mess.ContentFields.Abstractions.Services;
using YesSql.Sql;

namespace Mess.Ozds;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "Operator",
        RoleName = "Operator",
        RoleDescription = "Operator of closed distribution systems.",
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
            ClaimValue = "ManageUsersInRole_Owner"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_Owner"
          },
        }
      }
    );

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "Owner",
        RoleName = "Owner",
        RoleDescription = "Owner of a closed distribution system.",
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
            ClaimValue = "ManageUsersInRole_Unit"
          },
          new RoleClaim
          {
            ClaimType = Permission.ClaimType,
            ClaimValue = "AssignRole_Unit"
          },
        }
      }
    );

    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "Unit",
        RoleName = "Unit",
        RoleDescription = "Unit in a closed distribution system.",
      }
    );

    if (_hostEnvironment.IsDevelopment())
    {
      await _userService.CreateDevUserAsync(
        "OperatorId",
        "Operator",
        "Operator"
      );
      await _userService.CreateDevUserAsync("OwnerId", "Owner", "Owner");
      await _userService.CreateDevUserAsync("UnitId", "Unit", "Unit");
    }

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
          .WithDescription("A Abb measurement device.")
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
            "AbbMeasurementDevicePart",
            part =>
              part.WithDisplayName("Abb measurement device")
                .WithDescription("A Abb measurement device.")
                .WithPosition("3")
          )
    );

    SchemaBuilder.CreateMapIndexTable<OzdsMeasurementDeviceIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("DeviceId", c => c.WithLength(64))
    );
    SchemaBuilder.AlterIndexTable<OzdsMeasurementDeviceIndex>(
      table =>
        table.CreateIndex("IDX_OzdsMeasurementDeviceIndex_DeviceId", "DeviceId")
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

      var abbMeasurementDevice =
        await _contentManager.NewContentAsync<AbbMeasurementDeviceItem>();
      abbMeasurementDevice.Alter(
        abbMeasurementDevice => abbMeasurementDevice.MeasurementDevicePart,
        measurementDevicePart =>
        {
          measurementDevicePart.DeviceId = new() { Text = "abb" };
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
