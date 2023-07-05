import { DateTime } from "luxon";
import { Create } from ".";
import compile from "./compile";
import args from "../args";

export const push: Create = async (template) => {
  const now = DateTime.now().toUTC();
  const data = [
    {
      tenant: args.tenant,
      deviceId: "eor",
      timestamp: now.toISO(),
      voltage: (Math.random() * 1000).toFixed(0),
      current: (Math.random() * 1000).toFixed(0),
      temperature: (Math.random() * 1000).toFixed(0),
      coolingFans: Math.random() > 0.5,
      heatsinkFans: Math.random() > 0.5,
    },
  ];

  return data.map((data) => compile(template, data));
};

export const update: Create = async (template) => {
  const now = DateTime.now().toUTC();
  const isRunning = Math.random() > 0.1;
  const shouldReset = Math.random() > 0.9;
  const doorOpen = Math.random() > 0.9;
  const mainCircuitBreakerOn = Math.random() > 0.9;
  const transformerContractorOn = Math.random() > 0.9;
  const firstDiodeBridgeOk = Math.random() > 0.9;
  const secondDiodeBridgeOk = Math.random() > 0.9;

  const data = [
    {
      tenant: args.tenant,
      deviceId: "eor",
      timestamp: now.toISO(),
      mode: (Math.random() * 63).toFixed(0),
      processFaults: (Math.random() * 1000).toFixed(0),
      communicationFaults: (Math.random() * 1000).toFixed(0),
      runState: isRunning ? "1" : "0",
      resetState: shouldReset ? "1" : "0",
      doorState: doorOpen ? "1" : "0",
      mainCircuitBreakerState: mainCircuitBreakerOn ? "0" : "1",
      transformerContractorState: transformerContractorOn ? "0" : "1",
      firstDiodeBridgeState: firstDiodeBridgeOk ? "1" : "0",
      secondDiodeBridgeState: secondDiodeBridgeOk ? "1" : "0",
    },
  ];

  return data.map((data) => compile(template, data));
};
