using OrchardCore.Security.Permissions;

namespace Mess.Billing.Abstractions;

public class Permissions : IPermissionProvider
{
  public static readonly Permission IssueInvoice = new Permission(
    "IssueInvoice",
    "Issuing invoice"
  );

  public static readonly Permission ConfirmPayment = new Permission(
    "ConfirmPayment",
    "Confirming payment"
  );

  public static readonly Permission ListIssuedBills = new Permission(
    "ListIssuedBills",
    "Listing issued bills"
  );

  public static readonly Permission ListReceivedBills = new Permission(
    "ListReceivedBills",
    "Listing received bills"
  );

  public Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return Task.FromResult(
      new[]
      {
        IssueInvoice,
        ConfirmPayment,
        ListIssuedBills,
        ListReceivedBills
      }.AsEnumerable()
    );
  }

  public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
  {
    return new[]
    {
      new PermissionStereotype
      {
        Name = "Administrator",
        Permissions = new[]
        {
          IssueInvoice,
          ConfirmPayment,
          ListIssuedBills,
          ListReceivedBills
        }
      },
    };
  }
}
