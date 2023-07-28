import { useClock } from "./hooks";

declare global {
  interface Window {
    mess: {
      forttech: {
        useClock: typeof useClock;
      };
    };
  }
}

window.mess = {
  ...window.mess,
  forttech: {
    useClock,
  },
};
