using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace Mess.Billing.Abstractions.Models;

public class LegalEntityPart : ContentPart
{
  public UserPickerField Representatives { get; set; } = new();

  public TextField Name { get; set; } = new();

  public TextField SocialSecurityNumber { get; set; } = new();

  public TextField Address { get; set; } = new();

  public TextField City { get; set; } = new();

  public TextField PostalCode { get; set; } = new();

  public TextField Email { get; set; } = new();
}
