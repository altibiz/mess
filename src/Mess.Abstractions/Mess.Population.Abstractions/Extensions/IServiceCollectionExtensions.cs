namespace Mess.Population.Abstractions.Extensions;

public static class IServiceCollectionExtensions
{
  public static IServiceCollection AddPopulation<TPopulation>(
    this IServiceCollection services
  )
    where TPopulation : class, IPopulation
  {
    services.AddScoped<IPopulation, TPopulation>();

    return services;
  }
}
