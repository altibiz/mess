import { createChart } from "./api";
import { bindChart, refreshChart } from "./chart";
import loadFont, { Font } from "./font";

export type UseChartProps = {
  requestUrlPrefix: string;
  contentItemId: string;
  isPreview: boolean;
  canvasId: string;
  culture: string;
  font?: Font;
};

export const useChart = ({
  requestUrlPrefix,
  contentItemId,
  isPreview,
  canvasId,
  culture,
  font,
}: UseChartProps) => {
  let interval: number | null = null;
  window.addEventListener("load", async () => {
    if (font) {
      await loadFont(font);
    }
    const chartDescriptor = await createChart(
      requestUrlPrefix,
      contentItemId,
      isPreview,
    );
    const chart = bindChart(canvasId, culture, chartDescriptor, font);
    interval = setInterval(async () => {
      const chartDescriptor = await createChart(
        requestUrlPrefix,
        contentItemId,
        isPreview,
      );
      refreshChart(chart, chartDescriptor);
    }, chartDescriptor.refreshInterval);
  });
  window.addEventListener("unload", () => {
    if (interval) {
      clearInterval(interval);
    }
  });
};
