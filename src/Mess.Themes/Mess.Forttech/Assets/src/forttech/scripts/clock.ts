import { DateTime } from "luxon";

window.onload = () => {
  setInterval(() => {
    const clock = document.getElementById("clock");
    if (clock) {
      clock.textContent = DateTime.now().toLocaleString(
        DateTime.DATETIME_SHORT_WITH_SECONDS,
      );
    } else {
      console.error("Clock not found");
    }
  }, 1000);
};
