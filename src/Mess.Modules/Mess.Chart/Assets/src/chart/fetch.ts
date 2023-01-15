import {
  ChartSpecification,
  chartSpecificationSchema,
} from "./schema";

export const fetchChartSpecification = async (
  query: string,
): Promise<ChartSpecification | null> => {
  const response = await fetch(`/Chart${query}`);
  const specification = await response.json();

  const parsedSpecification = await chartSpecificationSchema.safeParseAsync(
    specification,
  );
  if (!parsedSpecification.success) {
    return null;
  }

  return parsedSpecification.data;
};
