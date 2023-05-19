import { ChartDescriptor, chartSescriptorSchema } from "./schema";

export const createChart = async (
  providerId: string,
  contentItem: string,
): Promise<ChartDescriptor | null> => {
  const response = await fetch(`/chart/${providerId}`, { body: contentItem });
  const rawChart = await response.json();

  const parsedChart = await chartSescriptorSchema.safeParseAsync(rawChart);
  if (!parsedChart.success) {
    return null;
  }

  return parsedChart.data;
};

window.createChart = createChart;
