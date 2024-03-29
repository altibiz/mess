using Mess.Billing.Abstractions.Indexes;
using Microsoft.AspNetCore.Identity;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Security;
using YesSql.Sql;

namespace Mess.Billing;

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
    await _roleManager.CreateAsync(
      new Role
      {
        NormalizedRoleName = "LegalEntityRepresentative",
        RoleName = "Legal entity representative",
        RoleDescription = "A legal entity representative."
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
                    DisplayedRoles = new[] { "Legal entity representative" },
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
      "InvoicePart",
      builder =>
        builder
          .Attachable()
          .WithDisplayName("Invoice")
          .WithDescription("An invoice.")
          .WithField(
            "Receipt",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("Receipt")
                .WithDescription("Receipt of the invoice.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    Required = true,
                    Hint = "Receipt of the invoice."
                  }
                )
          )
          .WithField(
            "Date",
            field =>
              field
                .OfType("DateField")
                .WithDisplayName("Date")
                .WithDescription("Date of the invoice.")
                .WithSettings(
                  new DateFieldSettings
                  {
                    Required = true,
                    Hint = "Date of the invoice."
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
          .WithField(
            "Invoice",
            field =>
              field
                .OfType("ContentPickerField")
                .WithDisplayName("Invoice")
                .WithDescription("Invoice of the receipt.")
                .WithSettings(
                  new ContentPickerFieldSettings
                  {
                    DisplayedContentTypes = new[] { "Invoice" },
                    Required = true,
                    Hint = "Invoice of the receipt."
                  }
                )
          )
          .WithField(
            "Date",
            field =>
              field
                .OfType("DateField")
                .WithDisplayName("Date")
                .WithDescription("Date of the receipt.")
                .WithSettings(
                  new DateFieldSettings
                  {
                    Required = true,
                    Hint = "Date of the receipt."
                  }
                )
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

    SchemaBuilder.CreateMapIndexTable<BillingIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("ContentType", c => c.WithLength(64))
          .Column<string>("IssuerContentItemId", c => c.WithLength(64))
          .Column<string>("RecipientContentItemId", c => c.WithLength(64))
    );
    SchemaBuilder.AlterIndexTable<BillingIndex>(table =>
    {
      table.CreateIndex("IDX_BillingIndex_ContentType", "ContentType");
      table.CreateIndex(
        "IDX_BillingIndex_RecipientContentItemId",
        "RecipientContentItemId"
      );
      table.CreateIndex(
        "IDX_BillingIndex_IssuerContentItemId",
        "IssuerContentItemId"
      );
    });

    SchemaBuilder.CreateMapIndexTable<PaymentIndex>(
      table =>
        table
          .Column<string>("ContentItemId", c => c.WithLength(64))
          .Column<string>("ContentType", c => c.WithLength(64))
          .Column<string>("BillingContentItemId", c => c.WithLength(64))
          .Column<string>("IssuerContentItemId", c => c.WithLength(64))
          .Column<string>("InvoiceContentItemId", c => c.WithLength(64))
          .Column<string>("RecipientContentItemId", c => c.WithLength(64))
          .Column<string>("ReceiptContentItemId", c => c.WithLength(64))
    );
    SchemaBuilder.AlterIndexTable<PaymentIndex>(table =>
    {
      table.CreateIndex("IDX_PaymentIndex_ContentType", "ContentType");
      table.CreateIndex(
        "IDX_PaymentIndex_BillingContentItemId",
        "BillingContentItemId"
      );
      table.CreateIndex(
        "IDX_PaymentIndex_RecipientContentItemId",
        "RecipientContentItemId"
      );
      table.CreateIndex(
        "IDX_PaymentIndex_IssuerContentItemId",
        "IssuerContentItemId"
      );
    });

    return 1;
  }
}
