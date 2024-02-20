namespace Mess.Ozds.Timeseries;

public partial class OzdsTimeseriesClient
{
  // Abb Base Subquery
  private const string AbbBaseSubquery = """
    select
      "Source" as source,
      "Timestamp" as timestamp,
      "ActiveEnergyImportTotal_Wh" as energy,
      "ActivePowerL1_W" + "ActivePowerL2_W" + "ActivePowerL3_W" as power
    from
      "AbbMeasurements"
    where
        "Tenant" = 'ozds'
      and
        "Timestamp" >= {{0}}
      and
        "Timestamp" < {{1}}
      and
        ("Source" {0})
  """;

  // Schneider Base Subquery
  private const string SchneiderBaseSubquery = """
    select
      "Source" as source,
      "Timestamp" as timestamp,
      "ActiveEnergyImportTotal_Wh" as energy,
      "ActivePowerL1_W" + "ActivePowerL2_W" + "ActivePowerL3_W" as power
    from
      "SchneiderMeasurements"
    where
        "Tenant" = 'ozds'
      and
        "Timestamp" >= {{0}}
      and
        "Timestamp" < {{1}}
      and
        ("Source" {0})
  """;

  // Peak Power Subqueries
  private const string PPS = """
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
          measurements
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
  private const string FLS = """
    ranked as (
      select
        source,
        timestamp,
        energy,
        power,
        row_number() over (partition by source order by timestamp asc) as timestamp_ascending,
        row_number() over (partition by source order by timestamp desc) as timestamp_descending
      from
        measurements
    )
  """;

  private const string PeakPowerQueryTemplate =
    "with "
    + "measurements as ({0}), "
    + PPS
    + """
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

  private const string PeakPowerQuerySumTemplate =
    "with "
    + "measurements as ({0}), "
    + PPS
    + """
      select
        interval as "Interval",
        power as "ActivePower_W"
      from
        sum
      group by
        power desc
      limit 1
    """;

  private const string FirstLastEnergiesQueryTemplate =
    "with "
    + "measurements as ({0}), "
    + FLS
    + """
      select
        source as "Source",
        timestamp as "Timestamp",
        energy as "ActiveEnergyImportTotal_Wh"
      from
        ranked
      where
        timestamp_ascending = 1
        or timestamp_descending = 1
    """;

  private const string LastMeasurementsQueryTemplate =
    "with "
    + "measurements as ({0}), "
    + FLS
    + """
      select
        source as "Source",
        timestamp as "Timestamp",
        energy as "ActiveEnergyImportTotal_Wh",
        power as "ActivePower_W"
      from
        ranked
      where
        timestamp_descending = 1
    """;
}
