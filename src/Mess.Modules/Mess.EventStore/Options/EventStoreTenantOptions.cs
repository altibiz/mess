using System.ComponentModel.DataAnnotations;
using OrchardCore.Environment.Shell;

namespace Mess.EventStore.Options;

public record class EventStoreTenantOptions(
  IShellSettingsManager shellSettingsManager
) : IValidatableObject
{
  public string Name { get; set; } = default!;

  public string ConnectionString { get; set; } = default!;

  public IEnumerable<ValidationResult> Validate(
    ValidationContext validationContext
  )
  {
    if (String.IsNullOrWhiteSpace(Name))
    {
      yield return new ValidationResult(
        $"The field '{nameof(Name)}' should not be null or empty."
      );
    }

    if (String.IsNullOrWhiteSpace(ConnectionString))
    {
      yield return new ValidationResult(
        $"The field '{nameof(ConnectionString)}' should not be null or empty."
      );
    }

    var settings = shellSettingsManager.LoadSettingsAsync().Result.ToList();

    if (settings.Any(setting => setting.Name == Name))
    {
      yield return new ValidationResult(
        $"The field '{nameof(Name)}' should correspond to an OrchardCore tenant."
      );
    }
  }
}
