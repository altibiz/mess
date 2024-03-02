import compile from "./compile";
import { Create } from "./index";

export const push: Create = async (template) => {
  const data = {
    voltageL1_V: (Math.random() * (300 - 161) + 161).toFixed(0),
    voltageL2_V: (Math.random() * (300 - 161) + 161).toFixed(0),
    voltageL3_V: (Math.random() * (300 - 161) + 161).toFixed(0),
    currentL1_A: (Math.random() * (80 - 0) + 0).toFixed(0),
    currentL2_A: (Math.random() * (80 - 0) + 0).toFixed(0),
    currentL3_A: (Math.random() * (80 - 0) + 0).toFixed(0),
    activePowerL1_W: (Math.random() * (24000 - -24000) + -24000).toFixed(0),
    activePowerL2_W: (Math.random() * (24000 - -24000) + -24000).toFixed(0),
    activePowerL3_W: (Math.random() * (24000 - -24000) + -24000).toFixed(0),
    reactivePowerL1_VAR: (Math.random() * (24000 - -24000) + -24000).toFixed(0),
    reactivePowerL2_VAR: (Math.random() * (24000 - -24000) + -24000).toFixed(0),
    reactivePowerL3_VAR: (Math.random() * (24000 - -24000) + -24000).toFixed(0),
    activePowerImportL1_Wh: (Math.random() * 100000).toFixed(0),
    activePowerImportL2_Wh: (Math.random() * 100000).toFixed(0),
    activePowerImportL3_Wh: (Math.random() * 100000).toFixed(0),
    activePowerExportL1_Wh: (Math.random() * 100000).toFixed(0),
    activePowerExportL2_Wh: (Math.random() * 100000).toFixed(0),
    activePowerExportL3_Wh: (Math.random() * 100000).toFixed(0),
    reactivePowerImportL1_VARh: (Math.random() * 100000).toFixed(0),
    reactivePowerImportL2_VARh: (Math.random() * 100000).toFixed(0),
    reactivePowerImportL3_VARh: (Math.random() * 100000).toFixed(0),
    reactivePowerExportL1_VARh: (Math.random() * 100000).toFixed(0),
    reactivePowerExportL2_VARh: (Math.random() * 100000).toFixed(0),
    reactivePowerExportL3_VARh: (Math.random() * 100000).toFixed(0),
    activeEnergyImportTotal_Wh: (Math.random() * 100000).toFixed(0),
    activeEnergyExportTotal_Wh: (Math.random() * 100000).toFixed(0),
    reactiveEnergyImportTotal_VARh: (Math.random() * 100000).toFixed(0),
    reactiveEnergyExportTotal_VARh: (Math.random() * 100000).toFixed(0),
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
