import { DateTime } from "luxon";

export const useClock = (clockId: string) => {
  let interval: number | null = null;
  window.addEventListener("load", () => {
    interval = setInterval(() => {
      const clock = document.getElementById(clockId);
      if (clock) {
        clock.textContent = DateTime.now().toLocaleString(
          DateTime.DATETIME_SHORT_WITH_SECONDS,
        );
      } else {
        console.error("Clock not found");
      }
    }, 1000);
  });
  window.addEventListener("unload", () => {
    if (interval) {
      clearInterval(interval);
    }
  });
};
