using System.ComponentModel.DataAnnotations;
using OrchardCore.Environment.Shell;

namespace Mess.Timeseries.Options;

public record class TimeseriesOptions(
  IShellSettingsManager shellSettingsManager
) : IValidatableObject
{
  public List<TimeseriesTenantOptions> Tenants { get; set; } = new();

  public IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext
  )
  {
    if (Tenants.Count == 0)
    {
      yield return new ValidationResult(
        $"The field '{nameof(Tenants)}' should contain at least one tenant."
      );
    }

    foreach (
      var validationResult in Tenants.SelectMany(
        tenant => tenant.Validate(validationContext)
      )
    )
    {
      yield return validationResult;
    }

    var settings = shellSettingsManager.LoadSettingsAsync().Result.ToList();

    if (
      !Tenants
        .Select(tenant => tenant.Name)
        .ToHashSet()
        .SetEquals(settings.Select(setting => setting.Name).ToHashSet())
    )
    {
      yield return new ValidationResult(
        $"The field '{nameof(Tenants)}' should contain one tenant for every OrchardCore tenant."
      );
    }
  }
}
