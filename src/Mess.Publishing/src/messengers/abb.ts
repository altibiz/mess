import compile from "./compile";
import { Create } from "./index";

export const push: Create = async (template) => {
  const data = {
    currentL1: "null",
    currentL2: (Math.random() * 1000).toFixed(0),
    currentL3: (Math.random() * 1000).toFixed(0),
    voltageL1: (Math.random() * 1000).toFixed(0),
    voltageL2: (Math.random() * 1000).toFixed(0),
    voltageL3: (Math.random() * 1000).toFixed(0),
    activePowerL1: (Math.random() * 1000).toFixed(0),
    activePowerL2: (Math.random() * 1000).toFixed(0),
    activePowerL3: (Math.random() * 1000).toFixed(0),
    reactivePowerL1: (Math.random() * 1000).toFixed(0),
    reactivePowerL2: (Math.random() * 1000).toFixed(0),
    reactivePowerL3: (Math.random() * 1000).toFixed(0),
    apparentPowerL1: (Math.random() * 1000).toFixed(0),
    apparentPowerL2: (Math.random() * 1000).toFixed(0),
    apparentPowerL3: (Math.random() * 1000).toFixed(0),
    powerFactorL1: (Math.random() * 1000).toFixed(0),
    powerFactorL2: (Math.random() * 1000).toFixed(0),
    powerFactorL3: (Math.random() * 1000).toFixed(0),
  };

  return {
    metadata: {
      contentType: "application/json",
      apiKey: "abb",
      deviceId: "abb",
    },
    payload: compile(template, data),
  };
};
