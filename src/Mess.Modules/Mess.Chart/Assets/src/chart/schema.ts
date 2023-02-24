import * as z from "zod";

export type ChartModel = typeof chartModelSchema._type;

export type LineChartModel = typeof lineChartModelSchema._type;

export const isLineChartModel = (chart: ChartModel): chart is LineChartModel =>
  chart.type == "Line";

export const timeseriesChartDatapointModelScema = z.object({
  x: z.date(),
  y: z.number(),
});

export const timeseriesChartDatasetModelSchema = z.object({
  type: z.literal("Timeseries"),
  label: z.string(),
  color: z.string(),
  datapoints: z.array(timeseriesChartDatapointModelScema),
});

export const lineChartModelSchema = z.object({
  type: z.literal("Line"),
  datasets: z.array(timeseriesChartDatasetModelSchema),
});

export const chartModelSchema = lineChartModelSchema;
