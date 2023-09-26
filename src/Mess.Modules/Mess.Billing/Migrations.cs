using Microsoft.AspNetCore.Identity;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Security;

namespace Mess.Billing;

public class Migrations : DataMigration
{
  public async Task<int> CreateAsync()
  {
    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "LegalEntityRepresentative",
        RoleName = "Legal entity representative",
        RoleDescription = "A legal entity representative.",
      }
    );

    _contentDefinitionManager.AlterPartDefinition(
      "LegalEntityPart",
      builder =>
        builder
          .Attachable()
          .WithDescription(
            "Identification, contact and address information of a legal entity."
          )
          .WithDisplayName("Legal entity")
          .WithField(
            "Representatives",
            fieldBuilder =>
              fieldBuilder
                .OfType("UserPickerField")
                .WithDisplayName("Representatives")
                .WithDescription(
                  "Representatives acting on behalf of this legal entity."
                )
                .WithSettings(
                  new UserPickerFieldSettings
                  {
                    Multiple = true,
                    Required = false,
                    DisplayAllUsers = false,
                    DisplayedRoles = new[] { "LegalEntityRepresentative" },
                    Hint =
                      "Representatives acting on behalf of this legal entity."
                  }
                )
          )
          .WithField(
            "Name",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Name")
                .WithDescription("Name of the legal entity.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Required = true,
                    Hint = "Name of the legal entity."
                  }
                )
          )
          .WithField(
            "SocialSecurityNumber",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Social security number")
                .WithDescription("Social security number of the legal entity.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Required = true,
                    Hint = "Social security number of the legal entity."
                  }
                )
          )
          .WithField(
            "Address",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Address")
                .WithDescription("Address of the legal entity.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Required = true,
                    Hint = "Address of the legal entity."
                  }
                )
          )
          .WithField(
            "City",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("City")
                .WithDescription("City of the legal entity.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Required = true,
                    Hint = "City of the legal entity."
                  }
                )
          )
          .WithField(
            "PostalCode",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Postal code")
                .WithDescription("Postal code of the legal entity.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Required = true,
                    Hint = "Postal code of the legal entity."
                  }
                )
          )
          .WithField(
            "Email",
            fieldBuilder =>
              fieldBuilder
                .OfType("TextField")
                .WithDisplayName("Email")
                .WithDescription("Email of the legal entity.")
                .WithSettings(
                  new TextFieldSettings
                  {
                    Required = true,
                    Hint = "Email of the legal entity."
                  }
                )
          )
    );

    return 1;
  }

  public Migrations(
    IContentDefinitionManager contentDefinitionManager,
    IRecipeMigrator recipeMigrator,
    RoleManager<IRole> roleManager
  )
  {
    _contentDefinitionManager = contentDefinitionManager;
    _recipeMigrator = recipeMigrator;
    _roleManager = roleManager;
  }

  private readonly IContentDefinitionManager _contentDefinitionManager;
  private readonly IRecipeMigrator _recipeMigrator;
  private readonly RoleManager<IRole> _roleManager;
}
