namespace Mess.Ozds.Timeseries;

public partial class OzdsTimeseriesClient
{
  // Grand Meter Unification Subquery
  private const string GMUS = """
    select
      "Source" as source,
      "Timestamp" as timestamp,
      "ActiveEnergyImportTotal_Wh" as energy
    from
      "AbbMeasurements"
    where
        timestamp >= {{0}}
      and
        timestamp < {{1}}
      and
        source {1}
    union
    select
      "Source" as source,
      "Timestamp" as timestamp,
      "ActiveEnergyImportTotal_Wh" as energy
    from
      "SchneiderMeasurements"
    where
        timestamp >= {{0}}
      and
        timestamp < {{1}}
      and
        source {1}
  """;

  // Peak Power Subqueries
  private const string PPS = "with"
    + $"measurements as ({GMUS}), "
    + """
      buckets as (
        select
          distinct on (
            source,
            time_bucket('15 minutes', timestamp)
          )
          source,
          timestamp,
          time_bucket('15 minutes', timestamp) as interval,
          first_value(energy) over bucket_windows as begin_energy,
          last_value(energy) over bucket_windows as end_energy
        from
          {0}
        window bucket_windows as (
          partition by source, time_bucket('15 minutes', timestamp)
          order by timestamp asc
          range between unbounded preceding and unbounded following
        )
      ),
      calculation as (
        select
          source,
          interval,
          (end_energy - begin_energy) * 4 as power
        from
          buckets
      ),
      sum as (
        select
          interval,
          sum(power) as power
        from
          calculation
        group by
          interval
      )
    """;

  // First Last Subqueries
  private const string FLS =
    "with"
    + $"measurements as ({GMUS}), "
    + """
      ranked as (
        select
          source,
          timestamp,
          energy,
          row_number() over (partition by source order by timestamp asc) as row_number_ascending,
          row_number() over (partition by source order by timestamp desc) as row_number_descending
        from
          measurements
      )
    """;

  private const string PeakPowerQueryTemplate = PPS + """
    select
      source as "Source",
      interval as "Interval",
      power as "ActivePower_W"
    from
      calculation
    order by
      power desc
    limit 1
  """;

  private const string PeakPowerQuerySumTemplate = PPS + """
    select
      interval as "Interval",
      power as "ActivePower_W"
    from
      sum
    group by
      power desc
    limit 1
  """;

  private const string FirstLastEnergiesQueryTemplate = FLS + """
    select
      source as "Source",
      timestamp as "Timestamp",
      energy as "ActiveEnergyImportTotal_Wh"
    from
      ranked
    where
      row_number_ascending = 1
      or row_number_descending = 1
  """;
}
