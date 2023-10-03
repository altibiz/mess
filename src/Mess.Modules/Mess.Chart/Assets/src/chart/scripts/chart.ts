import {
  CartesianScaleTypeRegistry,
  CategoryScale,
  Chart,
  ChartConfiguration,
  ChartType,
  DefaultDataPoint,
  Legend,
  LineController,
  LineElement,
  LinearScale,
  PointElement,
  ScaleOptionsByType,
  TimeScale,
  TimeSeriesScale,
  Title,
  Tooltip,
} from "chart.js";
import { DeepPartial } from "chart.js/dist/types/utils";
import "chartjs-adapter-luxon";
import { DateTime } from "luxon";
import { Font } from "./font";
import {
  ChartDescriptor,
  TimeseriesChartDescriptor,
  isTimeseriesChartDescriptor,
} from "./schema";

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

type SimpleChartConfiguration<
  TType extends ChartType = ChartType,
  TData = DefaultDataPoint<TType>,
  TLabel = unknown,
> = ChartConfiguration<TType, TData, TLabel>;

type SimpleChart<
  TType extends ChartType = ChartType,
  TData = DefaultDataPoint<TType>,
  TLabel = unknown,
> = Chart<TType, TData, TLabel> & {
  config: SimpleChartConfiguration<TType, TData, TLabel>;
};

export type BoundTimeseriesChart = SimpleChart<
  "line",
  { x: DateTime; y: number }[],
  string
>;

export type BoundChart = BoundTimeseriesChart;

export const bindChart = (
  id: string,
  culture: string,
  descriptor: ChartDescriptor,
  font?: Font,
): BoundChart => {
  let boundChart: BoundChart | null = null;

  if (isTimeseriesChartDescriptor(descriptor)) {
    boundChart = new Chart<"line", { x: DateTime; y: number }[], string>(id, {
      type: "line",
      data: {
        datasets: mapToTimeseriesChartDatasets(descriptor),
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        devicePixelRatio: window.devicePixelRatio,
        plugins: {
          tooltip: {
            enabled: true,
            titleFont: font,
            bodyFont: font,
          },
          legend: {
            display: true,
            labels: {
              font,
              color: font?.color,
            },
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
            ticks: {
              font: font,
              color: font?.color,
            },
          },
          ...descriptor.datasets.reduce((scales, { label }) => {
            scales[label] = {
              type: "linear",
              position: "left",
              ticks: {
                font: font,
                color: font?.color,
              },
            };

            return scales;
          }, {} as Record<string, DeepPartial<ScaleOptionsByType<keyof CartesianScaleTypeRegistry>>>),
        },
      },
    }) as BoundTimeseriesChart;
  }

  if (!boundChart) {
    throw new Error("Unsupported chart type");
  }
  return boundChart;
};

export const refreshChart = (
  chart: BoundChart,
  descriptor: ChartDescriptor,
): BoundChart => {
  let resultChart: BoundChart | null = null;

  if (
    isTimeseriesChartDescriptor(descriptor) &&
    isBoundTimeseriesChart(chart)
  ) {
    const datasets = mapToTimeseriesChartDatasets(descriptor);
    mergeTimeseriesChartDatasets(chart, datasets, descriptor.history);
    resultChart = chart;
  }

  if (!resultChart) {
    throw new Error("Unsupported chart type");
  }

  chart.update();
  return resultChart;
};

const mapToTimeseriesChartDatasets = (
  descriptor: TimeseriesChartDescriptor,
): BoundTimeseriesChart["data"]["datasets"] =>
  descriptor.datasets.map(({ label, datapoints: data, color }) => ({
    label,
    data: data.map(({ x, y }) => ({ x: DateTime.fromISO(x), y })),
    parsing: {
      xAxisKey: "x",
      yAxisKey: "y",
    },
    yAxisID: label,
    borderColor: color,
    backgroundColor: color,
  }));

const mergeTimeseriesChartDatasets = (
  chart: BoundTimeseriesChart,
  datasets: BoundTimeseriesChart["data"]["datasets"],
  history: number,
) => {
  datasets.forEach((newDataset, i) => {
    if (chart.data.datasets[i] && chart.data.datasets[i].data.length) {
      const oldDataset = chart.data.datasets[i];

      const lastTimestamp = oldDataset.data[oldDataset.data.length - 1].x;

      oldDataset.data = oldDataset.data.filter(
        (datapoint) =>
          datapoint.x > lastTimestamp.minus({ millisecond: history }),
      );

      newDataset.data.forEach((datapoint) => {
        if (datapoint.x > lastTimestamp) {
          oldDataset.data.push(datapoint);
        }
      });
    } else {
      chart.data.datasets.push(newDataset);
    }
  });
};

const isBoundTimeseriesChart = (
  chart: BoundChart,
): chart is BoundTimeseriesChart => chart.config.type === "line";
