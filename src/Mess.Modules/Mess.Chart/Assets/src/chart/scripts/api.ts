import { ChartDescriptor, chartSescriptorSchema } from "./schema";

export const createChart = async (
  requestUrlPrefix: string,
  contentItemId: string,
  isPreview = false,
): Promise<ChartDescriptor> => {
  const response = await fetch(
    isPreview
      ? `${requestUrlPrefix}/Chart/Preview/${contentItemId}`
      : `${requestUrlPrefix}/Chart/${contentItemId}`,
  );
  const rawChart = await response.json();
  const parsedChart = await chartSescriptorSchema.parseAsync(rawChart);
  return parsedChart;
};
