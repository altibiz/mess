using Mess.Ozds.Abstractions.Timeseries;
using Mess.Timeseries.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mess.Ozds.Abstractions.Billing;

namespace Mess.Ozds.Timeseries;

public class OzdsTimeseriesClient : IOzdsTimeseriesClient
{
  public void AddAbbMeasurement(AbbMeasurement model) =>
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.AbbMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddAbbMeasurementAsync(AbbMeasurement model) =>
    _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(async context =>
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
      OzdsTimeseriesDbContext,
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
      OzdsTimeseriesDbContext,
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
      OzdsTimeseriesDbContext,
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
      OzdsTimeseriesDbContext,
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

  public void AddSchneiderMeasurement(SchneiderMeasurement model) =>
    _services.WithTimeseriesDbContext<OzdsTimeseriesDbContext>(context =>
    {
      context.SchneiderMeasurements.Add(model.ToEntity());
      context.SaveChanges();
    });

  public Task AddSchneiderMeasurementAsync(SchneiderMeasurement model) =>
    _services.WithTimeseriesDbContextAsync<OzdsTimeseriesDbContext>(async context =>
    {
      context.SchneiderMeasurements.Add(model.ToEntity());
      await context.SaveChangesAsync();
    });

  public IReadOnlyList<SchneiderMeasurement> GetSchneiderMeasurements(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  ) =>
    _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      List<SchneiderMeasurement>
    >(context =>
    {
      return context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToList();
    });

  public async Task<
    IReadOnlyList<SchneiderMeasurement>
  > GetSchneiderMeasurementsAsync(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  ) =>
    await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      List<SchneiderMeasurement>
    >(async context =>
    {
      return await context.SchneiderMeasurements
        .Where(measurement => measurement.Source == source)
        .Where(measurement => measurement.Timestamp > beginning)
        .Where(measurement => measurement.Timestamp < end)
        .OrderBy(measurement => measurement.Timestamp)
        .Select(measurement => measurement.ToModel())
        .ToListAsync();
    });

  public OzdsBillingData? GetSchneiderBillingData(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  )
  {
    return _services.WithTimeseriesDbContext<
      OzdsTimeseriesDbContext,
      OzdsBillingData?
    >(context =>
    {
      var first = context.SchneiderMeasurements
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

      var last = context.SchneiderMeasurements
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

      var peak = context.SchneiderMeasurements
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

  public async Task<OzdsBillingData?> GetSchneiderBillingDataAsync(
    string source,
    DateTimeOffset beginning,
    DateTimeOffset end
  )
  {
    return await _services.WithTimeseriesDbContextAsync<
      OzdsTimeseriesDbContext,
      OzdsBillingData?
    >(async context =>
    {
      var makeExtremeQuery = () =>
        context.SchneiderMeasurements
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
        context.SchneiderMeasurements
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

  public OzdsTimeseriesClient(
    IServiceProvider services,
    ILogger<OzdsTimeseriesClient> logger
  )
  {
    _services = services;
    _logger = logger;
  }

  private readonly IServiceProvider _services;
  private readonly ILogger _logger;
}
