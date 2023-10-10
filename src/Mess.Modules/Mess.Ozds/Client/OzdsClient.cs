using Mess.Ozds.Abstractions.Client;
using Mess.Ozds.Context;
using Mess.Timeseries.Abstractions.Extensions;
using Mess.Ozds.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mess.Ozds.Abstractions.Billing;
using System.Diagnostics.Metrics;

namespace Mess.Ozds.Client;

public class OzdsClient : IOzdsClient
{
  public void AddAbbMeasurement(AbbMeasurement model) =>
    _services.WithTimeseriesDbContext<OzdsDbContext>(context =>
    {
      context.AbbMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddAbbMeasurementAsync(AbbMeasurement model) =>
    _services.WithTimeseriesDbContextAsync<OzdsDbContext>(async context =>
    {
      context.AbbMeasurements.Add(model.ToEntity());
      await context.SaveChangesAsync();
    });

  public IReadOnlyList<AbbMeasurement> GetAbbMeasurements(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  ) =>
    _services.WithTimeseriesDbContext<
      OzdsDbContext,
      List<AbbMeasurement>
    >(context =>
    {
      return context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });

  public async Task<IReadOnlyList<AbbMeasurement>> GetAbbMeasurementsAsync(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  ) =>
    await _services.WithTimeseriesDbContextAsync<
      OzdsDbContext,
      List<AbbMeasurement>
    >(async context =>
    {
      return await context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });

  public OzdsBillingData? GetAbbBillingData(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsDbContext,
      OzdsBillingData?
    >(context =>
    {
      var first = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .Where(
          measurement =>
            measurement.Energy != null
            && measurement.LowEnergy != null
            && measurement.HighEnergy != null
        )
        .FirstOrDefault();

      var last = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .Where(
          measurement =>
            measurement.Energy != null
            && measurement.LowEnergy != null
            && measurement.HighEnergy != null
        )
        .LastOrDefault();

      var peak = context.AbbMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .Where(measurement => measurement.Power != null)
        .OrderByDescending(measurement => measurement.Power)
        .FirstOrDefault();

      if (first is null || last is null || peak is null)
      {
        return null;
      }

      return new OzdsBillingData(
        StartEnergy: (decimal)first.Energy!,
        EndEnergy: (decimal)last.Energy!,
        StartLowEnergy: (decimal)first.LowEnergy!,
        EndLowEnergy: (decimal)last.LowEnergy!,
        StartHighEnergy: (decimal)first.HighEnergy!,
        EndHighEnergy: (decimal)last.HighEnergy!,
        MaxPower: (decimal)peak.Power!
      );
    });
  }

  public async Task<OzdsBillingData?> GetAbbBillingDataAsync(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsDbContext,
      OzdsBillingData?
    >(async context =>
    {
      var makeExtremeQuery = () =>
        context.AbbMeasurements
          .Where(measurement => measurement.Source == source)
          .Where(measurement => measurement.Timestamp > beginning)
          .Where(measurement => measurement.Timestamp < end)
          .OrderBy(measurement => measurement.Timestamp)
          .Select(
            measurement =>
              new
              {
                Energy = measurement.Energy!.Value,
                LowEnergy = measurement.LowEnergy!.Value,
                HighEnergy = measurement.HighEnergy!.Value
              }
          );

      var makePeakQuery = () =>
        context.AbbMeasurements
          .Where(measurement => measurement.Source == source)
          .Where(measurement => measurement.Timestamp > beginning)
          .Where(measurement => measurement.Timestamp < end)
          .Where(measurement => measurement.Power != null)
          .GroupBy(measurement => measurement.Milliseconds / (1000 * 60 * 15))
          .Select(
            group =>
              new
              {
                Power = group.Average(measurement => measurement.Power!.Value)
              }
          )
          .OrderByDescending(measurement => measurement.Power);

      var first = await makeExtremeQuery().FirstOrDefaultAsync();
      var last = await makeExtremeQuery().LastOrDefaultAsync();
      var peak = await makePeakQuery().FirstOrDefaultAsync();

      if (first is null || last is null || peak is null)
      {
        return null;
      }

      return new OzdsBillingData(
        StartEnergy: (decimal)first.Energy,
        EndEnergy: (decimal)last.Energy,
        StartLowEnergy: (decimal)first.LowEnergy,
        EndLowEnergy: (decimal)last.LowEnergy,
        StartHighEnergy: (decimal)first.HighEnergy,
        EndHighEnergy: (decimal)last.HighEnergy,
        MaxPower: (decimal)peak.Power
      );
    });
  }

  public OzdsClient(IServiceProvider services, ILogger<OzdsClient> logger)
  {
    _services = services;
    _logger = logger;
  }

  private readonly IServiceProvider _services;
  private readonly ILogger _logger;
}
