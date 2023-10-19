namespace Mess.Population.Abstractions;

public interface IPopulation
{
  public void Populate();

  public Task PopulateAsync();
}
