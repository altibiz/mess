using Mess.Chart.Abstractions.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.Models;

namespace Mess.Chart.Abstractions.Providers;

public abstract class ChartProvider<
  TProviderParameters,
  TFieldEditorViewModel,
  TPartEditorViewModel
> : IChartProvider
  where TProviderParameters : class, IChartProviderParameters
  where TFieldEditorViewModel : notnull, new()
  where TPartEditorViewModel : notnull, new()
{
  public abstract string Id { get; }

  public virtual ChartSpecification? CreateChart(ChartParameters parameters)
  {
    var providerParameters =
      parameters.ProviderParameters as TProviderParameters;
    if (providerParameters is null)
    {
      return null;
    }

    return CreateChart(providerParameters);
  }

  public abstract ChartSpecification? CreateChart(
    TProviderParameters providerParameters
  );

  public virtual async Task<ChartSpecification?> CreateChartAsync(
    ChartParameters parameters
  )
  {
    var providerParameters =
      parameters.ProviderParameters as TProviderParameters;
    if (providerParameters is null)
    {
      return null;
    }

    return await CreateChartAsync(providerParameters);
  }

  public abstract Task<ChartSpecification?> CreateChartAsync(
    TProviderParameters providerParameters
  );

  public object CreateFieldEditorModel(
    BuildFieldEditorContext context,
    ContentField field,
    ChartParameters parameters
  )
  {
    var providerParameters =
      parameters.ProviderParameters as TProviderParameters;
    if (providerParameters is null)
    {
      return new TFieldEditorViewModel();
    }

    return CreateFieldEditorModel(context, field, providerParameters);
  }

  public abstract TFieldEditorViewModel CreateFieldEditorModel(
    BuildFieldEditorContext context,
    ContentField field,
    TProviderParameters providerParameters
  );

  public object CreatePartEditorModel(
    BuildPartEditorContext context,
    ContentPart part,
    ChartParameters parameters
  )
  {
    var providerParameters =
      parameters.ProviderParameters as TProviderParameters;
    if (providerParameters is null)
    {
      return new TPartEditorViewModel();
    }

    return CreatePartEditorModel(context, part, providerParameters);
  }

  public abstract TPartEditorViewModel CreatePartEditorModel(
    BuildPartEditorContext context,
    ContentPart field,
    TProviderParameters providerParameters
  );

  public abstract string GetFieldEditorShapeType(
    BuildFieldEditorContext context
  );

  public abstract string GetPartEditorShapeType(BuildPartEditorContext context);

  public virtual string? ValidateParameters(ChartParameters parameters)
  {
    var providerParameters =
      parameters.ProviderParameters as TProviderParameters;
    if (providerParameters is null)
    {
      return "Provider parameters are null";
    }

    return ValidateParameters(providerParameters);
  }

  public abstract string? ValidateParameters(
    TProviderParameters providerParameters
  );

  public virtual async Task<string?> ValidateParametersAsync(
    ChartParameters parameters
  )
  {
    var providerParameters =
      parameters.ProviderParameters as TProviderParameters;
    if (providerParameters is null)
    {
      return "Provider parameters are null";
    }

    return await ValidateParametersAsync(providerParameters);
  }

  public virtual Task<string?> ValidateParametersAsync(
    TProviderParameters providerParameters
  )
  {
    return Task.FromResult(ValidateParameters(providerParameters));
  }
}
