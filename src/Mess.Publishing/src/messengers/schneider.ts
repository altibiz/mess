import compile from "./compile";
import { Create } from "./index";

export const push: Create = async (template) => {
  const data = {
    voltageL1_V: "null",
    voltageL2_V: (Math.random() * 1000).toFixed(0),
    voltageL3_V: (Math.random() * 1000).toFixed(0),
    voltageAvg_V: (Math.random() * 1000).toFixed(0),
    currentL1_A: (Math.random() * 1000).toFixed(0),
    currentL2_A: (Math.random() * 1000).toFixed(0),
    currentL3_A: (Math.random() * 1000).toFixed(0),
    currentAvg_A: (Math.random() * 1000).toFixed(0),
    activePowerL1_kW: (Math.random() * 1000).toFixed(0),
    activePowerL2_kW: (Math.random() * 1000).toFixed(0),
    activePowerL3_kW: (Math.random() * 1000).toFixed(0),
    activePowerTotal_kW: (Math.random() * 1000).toFixed(0),
    reactivePowerTotal_kVAR: (Math.random() * 1000).toFixed(0),
    apparentPowerTotal_kVA: (Math.random() * 1000).toFixed(0),
    powerFactorTotal: (Math.random() * 1000).toFixed(0),
    activeEnergyImportTotal_Wh: (Math.random() * 1000).toFixed(0),
    activeEnergyExportTotal_Wh: (Math.random() * 1000).toFixed(0),
    activeEnergyImportRateA_Wh: (Math.random() * 1000).toFixed(0),
    activeEnergyImportRateB_Wh: (Math.random() * 1000).toFixed(0),
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
