import compile from "./compile";
import { Create } from "./index";

export const push: Create = async (template) => {
  const data = {
    voltageL1_V: (Math.random() * 1000).toFixed(0),
    voltageL2_V: (Math.random() * 1000).toFixed(0),
    voltageL3_V: (Math.random() * 1000).toFixed(0),
    currentL1_A: (Math.random() * 1000).toFixed(0),
    currentL2_A: (Math.random() * 1000).toFixed(0),
    currentL3_A: (Math.random() * 1000).toFixed(0),
    activePowerL1_W: (Math.random() * 1000).toFixed(0),
    activePowerL2_W: (Math.random() * 1000).toFixed(0),
    activePowerL3_W: (Math.random() * 1000).toFixed(0),
    reactivePowerTotal_VAR: (Math.random() * 1000).toFixed(0),
    apparentPowerTotal_VA: (Math.random() * 1000).toFixed(0),
    activeEnergyImportL1_Wh: (Math.random() * 1000).toFixed(0),
    activeEnergyImportL2_Wh: (Math.random() * 1000).toFixed(0),
    activeEnergyImportL3_Wh: (Math.random() * 1000).toFixed(0),
    activeEnergyImportTotal_Wh: (Math.random() * 1000).toFixed(0),
    activeEnergyExportTotal_Wh: (Math.random() * 1000).toFixed(0),
    reactiveEnergyImportTotal_VARh: (Math.random() * 1000).toFixed(0),
    reactiveEnergyExportTotal_VARh: (Math.random() * 1000).toFixed(0),
  };

  return {
    metadata: {
      contentType: "application/json",
      apiKey: "schneider",
      deviceId: "schneider",
    },
    payload: compile(template, data),
  };
};
