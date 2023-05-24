import { ChartDescriptor, chartSescriptorSchema } from "./schema";

export type CreateChartOptions = {
  isPreview?: boolean;
};

export const createChart = async (
  contentItemId: string,
  options: CreateChartOptions = {},
): Promise<ChartDescriptor | null> => {
  const action = options.isPreview ? "/Chart/Preview" : "/Chart";
  const response = await fetch(`${action}/${contentItemId}`);
  const rawChart = await response.json();

  const parsedChart = await chartSescriptorSchema.safeParseAsync(rawChart);
  if (!parsedChart.success) {
    return null;
  }

  return parsedChart.data;
};
