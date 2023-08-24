import { DateTime } from "luxon";
import { Create } from "./index";
import compile from "./compile";

export const push: Create = async (template) => {
  const now = DateTime.now().toUTC();

  const data = {
    timestamp: `0x${now.toUnixInteger().toString(16)}`,
    power: (Math.random() * 1000).toFixed(0),
    voltage: (Math.random() * 2 + 239).toFixed(0),
  };
  return {
    metadata: {
      contentType: "application/xml",
      deviceId: "egauge",
    },
    payload: compile(template, data),
  };
};
