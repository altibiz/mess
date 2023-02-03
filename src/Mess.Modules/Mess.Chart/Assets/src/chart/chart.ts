import {
  Chart,
  ScaleOptionsByType,
  CartesianScaleTypeRegistry,
} from "chart.js";
import { ChartModel, isLineChartModel } from "./schema";

export const bindChart = (
  id: string,
  culture: string,
  chart: ChartModel,
): Chart | null => {
  if (isLineChartModel(chart)) {
    return new Chart(id, {
      type: "line",
      data: {
        datasets: chart.datasets.map(({ label, datapoints: data, color }) => ({
          label,
          data,
          parsing: {
            xAxisKey: "x",
            yAxisKey: "y",
          },
          yAxisID: label,
          borderColor: color,
          backgroundColor: color,
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
          ...chart.datasets.reduce((scales, { label }) => {
            scales[label] = {
              type: "timeseries",
              position: "left",
            };

            return scales;
          }, {} as Record<string, Partial<ScaleOptionsByType<keyof CartesianScaleTypeRegistry>>>),
        },
      },
    });
  } else {
    return null;
  }
};
