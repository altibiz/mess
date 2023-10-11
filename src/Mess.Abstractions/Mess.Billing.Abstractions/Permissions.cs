using OrchardCore.Security.Permissions;

namespace Mess.Billing.Abstractions;

public class Permissions : IPermissionProvider
{
  public static readonly Permission IssueInvoices = new Permission(
    "IssueInvoices",
    "Issuing invoices"
  );

  public static readonly Permission ConfirmPayments = new Permission(
    "ConfirmPayments",
    "Confirming payments"
  );

  public static readonly Permission ListPayments = new Permission(
    "ListPayments",
    "Listing payments"
  );

  public static readonly Permission ListOwnPayments = new Permission(
    "ListOwnPayments",
    "Listing own payments"
  );

  public Task<IEnumerable<Permission>> GetPermissionsAsync()
  {
    return Task.FromResult(
      new[] { IssueInvoices, ConfirmPayments, ListPayments }.AsEnumerable()
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
          IssueInvoices,
          ConfirmPayments,
          ListPayments,
          ListOwnPayments
        }
      },
    };
  }
}
