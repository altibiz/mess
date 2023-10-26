import compile from "./compile";
import { Create } from "./index";

export const push: Create = async (template) => {
  const data = {
    voltageL1_V: "null",
    voltageL2_V: (Math.random() * 1000).toFixed(0),
    voltageL3_V: (Math.random() * 1000).toFixed(0),
    currentL1_A: (Math.random() * 1000).toFixed(0),
    currentL2_A: (Math.random() * 1000).toFixed(0),
    currentL3_A: (Math.random() * 1000).toFixed(0),
    activePowerTotal_W: (Math.random() * 1000).toFixed(0),
    activePowerL1_W: (Math.random() * 1000).toFixed(0),
    activePowerL2_W: (Math.random() * 1000).toFixed(0),
    activePowerL3_W: (Math.random() * 1000).toFixed(0),
    reactivePowerTotal_VAR: (Math.random() * 1000).toFixed(0),
    reactivePowerL1_VAR: (Math.random() * 1000).toFixed(0),
    reactivePowerL2_VAR: (Math.random() * 1000).toFixed(0),
    reactivePowerL3_VAR: (Math.random() * 1000).toFixed(0),
    apparentPowerTotal_VA: (Math.random() * 1000).toFixed(0),
    apparentPowerL1_VA: (Math.random() * 1000).toFixed(0),
    apparentPowerL2_VA: (Math.random() * 1000).toFixed(0),
    apparentPowerL3_VA: (Math.random() * 1000).toFixed(0),
    powerFactorTotal: (Math.random() * 1000).toFixed(0),
    powerFactorL1: (Math.random() * 1000).toFixed(0),
    powerFactorL2: (Math.random() * 1000).toFixed(0),
    powerFactorL3: (Math.random() * 1000).toFixed(0),
    activeEnergyImportTotal_kWh: (Math.random() * 1000).toFixed(0),
    activeEnergyExportTotal_kWh: (Math.random() * 1000).toFixed(0),
    activeEnergyNetTotal_kWh: (Math.random() * 1000).toFixed(0),
    activeEnergyImportTariff1_kWh: (Math.random() * 1000).toFixed(0),
    activeEnergyImportTariff2_kWh: (Math.random() * 1000).toFixed(0),
    activeEnergyExportTariff1_kWh: (Math.random() * 1000).toFixed(0),
    activeEnergyExportTariff2_kWh: (Math.random() * 1000).toFixed(0),
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
