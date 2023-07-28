import * as z from "zod";

const baseChartDescriptorSchema = z.object({
  type: z.literal("timeseries"),
  refreshInterval: z.number(),
});

export type ChartDescriptor = typeof chartSescriptorSchema._type;

export type TimeseriesChartDescriptor =
  typeof timeseriesChartDescriptorSchema._type;

export type TimeseriesChartDatasetDescriptor =
  typeof timeseriesChartDatasetDescriptorSchema._type;

export type TimeseriesChartDatapointDescriptor =
  typeof timeseriesChartDatapointDescriptorScema._type;

export const isTimeseriesChartDescriptor = (
  chart: ChartDescriptor,
): chart is TimeseriesChartDescriptor => chart.type == "timeseries";

export const timeseriesChartDatapointDescriptorScema = z.object({
  x: z.string(),
  y: z.number(),
});

export const timeseriesChartDatasetDescriptorSchema = z.object({
  label: z.string(),
  color: z.string(),
  datapoints: z.array(timeseriesChartDatapointDescriptorScema),
});

export const timeseriesChartDescriptorSchema = z.intersection(
  baseChartDescriptorSchema,
  z.object({
    type: z.literal("timeseries"),
    history: z.number(),
    datasets: z.array(timeseriesChartDatasetDescriptorSchema),
  }),
);

export const chartSescriptorSchema = z.intersection(
  timeseriesChartDescriptorSchema,
  baseChartDescriptorSchema,
);
