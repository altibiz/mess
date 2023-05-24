import {
  Chart,
  ScaleOptionsByType,
  CartesianScaleTypeRegistry,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  TimeScale,
  TimeSeriesScale,
  LineController,
} from "chart.js";
import "chartjs-adapter-luxon";
import { DateTime } from "luxon";
import { ChartDescriptor, isTimeseriesChartDescriptor } from "./schema";

Chart.register(
  CategoryScale,
  LinearScale,
  TimeScale,
  TimeSeriesScale,
  PointElement,
  LineElement,
  LineController,
  Title,
  Tooltip,
  Legend,
);

export type BoundChart = Chart<"line", { x: DateTime; y: number }[], string>;

export const bindChart = (
  id: string,
  culture: string,
  chart: ChartDescriptor,
): BoundChart | null => {
  let boundChart: BoundChart | null = null;
  if (isTimeseriesChartDescriptor(chart)) {
    boundChart = new Chart<"line", { x: DateTime; y: number }[], string>(id, {
      type: "line",
      data: {
        datasets: chart.datasets.map(({ label, datapoints: data, color }) => ({
          label,
          data: data.map(({ x, y }) => ({ x: DateTime.fromISO(x), y })),
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
        plugins: {
          legend: {
            display: false,
          },
        },
        scales: {
          x: {
            type: "time",
            position: "bottom",
            adapters: {
              date: {
                locale: culture,
              },
            },
          },
          ...chart.datasets.reduce((scales, { label }) => {
            scales[label] = {
              type: "linear",
              position: "left",
            };

            return scales;
          }, {} as Record<string, Partial<ScaleOptionsByType<keyof CartesianScaleTypeRegistry>>>),
        },
      },
    });
  }

  return boundChart;
};
