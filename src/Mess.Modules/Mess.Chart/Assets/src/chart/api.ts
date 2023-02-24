import { ChartModel, chartModelSchema } from "./schema";

export const createChart = async (
  providerId: string,
  contentItem: string,
): Promise<ChartModel | null> => {
  const response = await fetch(`/Chart/${providerId}`, { body: contentItem });
  const rawChart = await response.json();

  const parsedChart = await chartModelSchema.safeParseAsync(rawChart);
  if (!parsedChart.success) {
    return null;
  }

  return parsedChart.data;
};
