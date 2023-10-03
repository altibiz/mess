using Mess.Billing.Abstractions.Indexes;
using Microsoft.AspNetCore.Identity;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using OrchardCore.Security;
using OrchardCore.Title.Models;
using YesSql.Sql;

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

    _contentDefinitionManager.AlterPartDefinition(
      "ReceiptPart",
      builder =>
        builder
          .Attachable()
          .WithDisplayName("Receipt")
          .WithDescription("A receipt.")
    );
    _contentDefinitionManager.AlterTypeDefinition(
      "Receipt",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Securable()
          .DisplayedAs("Receipt")
          .WithDescription("A receipt.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription("Title of the receipt.")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedHidden,
                    Pattern = @"{{- ContentItem.Content.ReceiptPart.Id -}}"
                  }
                )
                .WithPosition("1")
          )
          .WithPart(
            "ReceiptPart",
            part =>
              part.WithDisplayName("Receipt")
                .WithDescription("A receipt.")
                .WithPosition("2")
          )
    );

    _contentDefinitionManager.AlterPartDefinition(
      "InvoicePart",
      builder =>
        builder
          .Attachable()
          .WithDisplayName("Invoice")
          .WithDescription("An invoice.")
    );
    _contentDefinitionManager.AlterTypeDefinition(
      "Invoice",
      builder =>
        builder
          .Creatable()
          .Listable()
          .Securable()
          .DisplayedAs("Invoice")
          .WithDescription("An invoice.")
          .WithPart(
            "TitlePart",
            part =>
              part.WithDisplayName("Title")
                .WithDescription("Title of the invoice.")
                .WithSettings<TitlePartSettings>(
                  new()
                  {
                    RenderTitle = true,
                    Options = TitlePartOptions.GeneratedHidden,
                    Pattern = @"{{- ContentItem.Content.ReceiptPart.Id -}}"
                  }
                )
          )
          .WithPart(
            "InvoicePart",
            part =>
              part.WithDisplayName("Invoice")
                .WithDescription("An invoice.")
                .WithPosition("2")
          )
    );

    _contentDefinitionManager.AlterPartDefinition(
      "BillingPart",
      builder =>
        builder
          .Attachable()
          .WithDisplayName("Billing")
          .WithDescription("Make a content item billable.")
    );

    _contentDefinitionManager.AlterPartDefinition(
      "CataloguePart",
      builder =>
        builder
          .Attachable()
          .WithDisplayName("Catalogue")
          .WithDescription("A catalogue.")
    );

    SchemaBuilder.CreateMapIndexTable<BillingIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("ContentType", c => c.WithLength(64))
          .Column<string>("LegalEntityContentItemId", c => c.WithLength(64))
          .Column<string>("CatalogueContentItemId", c => c.WithLength(64))
    );
    SchemaBuilder.AlterIndexTable<BillingIndex>(table =>
    {
      table.CreateIndex("IDX_BillingIndex_ContentType", "ContentType");
      table.CreateIndex(
        "IDX_BillingIndex_LegalEntityContentItemId",
        "LegalEntityContentItemId"
      );
      table.CreateIndex(
        "IDX_BillingIndex_CatalogueContentItemId",
        "CatalogueContentItemId"
      );
    });

    SchemaBuilder.CreateMapIndexTable<PaymentIndex>(
      table =>
        table
          .Column<string>("InvoiceContentItemId", c => c.WithLength(64))
          .Column<string>("ReceiptContentItemId", c => c.WithLength(64))
          .Column<string>("BillingContentItemId", c => c.WithLength(64))
          .Column<string>("IssuerContentItemId", c => c.WithLength(64))
          .Column<string>("RecipientContentItemId", c => c.WithLength(64))
          .Column<string>("CatalogueContentItemId", c => c.WithLength(64))
    );
    SchemaBuilder.AlterIndexTable<PaymentIndex>(table =>
    {
      table.CreateIndex(
        "IDX_PaymentIndex_InvoiceContentItemId",
        "InvoiceContentItemId"
      );
      table.CreateIndex(
        "IDX_PaymentIndex_ReceiptContentItemId",
        "ReceiptContentItemId"
      );
      table.CreateIndex(
        "IDX_PaymentIndex_BillingContentItemId",
        "BillingContentItemId"
      );
      table.CreateIndex(
        "IDX_PaymentIndex_IssuerContentItemId",
        "IssuerContentItemId"
      );
      table.CreateIndex(
        "IDX_PaymentIndex_RecipientContentItemId",
        "RecipientContentItemId"
      );
      table.CreateIndex(
        "IDX_PaymentIndex_CatalogueContentItemId",
        "CatalogueContentItemId"
      );
    });

    SchemaBuilder.CreateMapIndexTable<LegalEntityIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("ContentType", c => c.WithLength(64))
    );
    SchemaBuilder.AlterIndexTable<LegalEntityIndex>(table =>
    {
      table.CreateIndex("IDX_LegalEntityIndex_ContentType", "ContentType");
    });

    SchemaBuilder.CreateMapIndexTable<CatalogueIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("ContentType", c => c.WithLength(64))
    );
    SchemaBuilder.AlterIndexTable<CatalogueIndex>(table =>
    {
      table.CreateIndex("IDX_CatalogueIndex_ContentType", "ContentType");
    });

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
