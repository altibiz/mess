import { ChartDescriptor } from "./schema";

declare global {
  interface Window {
    createChart(
      providerId: string,
      contentItem: string,
    ): Promise<ChartDescriptor | null>;
  }
}
