import { createChart } from "./api";
import { bindChart } from "./chart";

declare global {
  interface Window {
    mess: {
      chart: {
        create: typeof createChart;
        bind: typeof bindChart;
      };
    };
  }
}

window.mess = {
  ...window.mess,
  chart: {
    create: createChart,
    bind: bindChart,
  },
};
