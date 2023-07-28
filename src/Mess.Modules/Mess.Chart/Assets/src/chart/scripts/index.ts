import { useChart } from "./hooks";

declare global {
  interface Window {
    mess: {
      chart: {
        useChart: typeof useChart;
      };
    };
  }
}

window.mess = {
  ...window.mess,
  chart: {
    useChart,
  },
};
