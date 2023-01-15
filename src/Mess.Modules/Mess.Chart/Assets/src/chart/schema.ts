import * as z from "zod";

export type ChartSpecification = typeof chartSpecificationSchema._type;

export type LineChartSpecification = typeof lineChartSpecificationSchema._type;

export const isLineChartSpecification = (
  chartSpecification: ChartSpecification["typeSpecification"],
): chartSpecification is LineChartSpecification =>
  chartSpecification.type == "line";

export const lineChartSpecificationSchema = z.object({
  type: z.literal("line"),
  datasets: z.array(
    z.object({
      id: z.string(),
      label: z.string(),
      unit: z.string(),
      color: z.string(),
      data: z.array(
        z.object({
          x: z.number(),
          y: z.number(),
        }),
      ),
    }),
  ),
});

export const chartSpecificationSchema = z.object({
  type: z.string(),
  typeSpecification: z.discriminatedUnion("type", [
    lineChartSpecificationSchema,
  ]),
});
