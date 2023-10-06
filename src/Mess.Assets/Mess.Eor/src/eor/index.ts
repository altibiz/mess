import { useRefresh } from "./hooks";

declare global {
  interface Window {
    mess: {
      eor: {
        useRefresh: typeof useRefresh;
      };
    };
  }
}

window.mess = {
  ...window.mess,
  eor: {
    useRefresh,
  },
};
