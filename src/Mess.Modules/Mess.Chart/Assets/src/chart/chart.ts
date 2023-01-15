import { Chart } from "chart.js";
import {
  ChartSpecification,
  isLineChartSpecification,
} from "./schema";

export const bindChart = (
  id: string,
  culture: string,
  chartSpecification: ChartSpecification,
): Chart | null => {
  const typeSpecification = chartSpecification.typeSpecification;
  if (isLineChartSpecification(typeSpecification)) {
    const datasets = typeSpecification.datasets;
    return new Chart(id, {
      type: "line",
      data: {
        datasets: datasets.map((dataset) => ({
          label: dataset.unit
            ? `${dataset.label} (${dataset.unit})`
            : dataset.label,
          data: dataset.data,
          parsing: {
            xAxisKey: "x",
            yAxisKey: "y",
          },
          yAxisID: dataset.id,
          borderColor: dataset.color,
          backgroundColor: dataset.color,
        })),
      },
      options: {
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: false,
          },
        },
        scales: {
          x: {
            type: "time",
            adapters: {
              date: {
                locale: culture,
              },
            },
          },
          ...datasets.reduce((axes, dataset) => {
            axes[dataset.id] = {
              type: "linear",
              position: "left",
            };
          }, {} as any),
        },
      },
    });
  } else {
    return null;
  }
};
