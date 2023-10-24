import * as zod from "zod";

export const runStateSchema = zod.enum(["Started", "Stopped", "Error"]);

export type RunState = zod.infer<typeof runStateSchema>;

export const resetStateSchema = zod.enum(["ShouldReset", "ShouldntReset"]);

export type ResetState = zod.infer<typeof resetStateSchema>;

export const doorStateSchema = zod.enum(["Open", "Closed"]);

export type DoorState = zod.infer<typeof doorStateSchema>;

export const mainCircuitBreakerStateSchema = zod.enum(["On", "Off"]);

export type MainCircuitBreakerState = zod.infer<
  typeof mainCircuitBreakerStateSchema
>;

export const transformerContractorStateSchema = zod.enum(["On", "Off"]);

export type TransformerContractorState = zod.infer<
  typeof transformerContractorStateSchema
>;

export const diodeBridgeStateSchema = zod.enum(["Ok", "Error"]);

export type DiodeBridgeState = zod.infer<typeof diodeBridgeStateSchema>;

export const eorIotDeviceControlsSchema = zod.object({
  mode: zod.number(),
  runState: runStateSchema,
  resetState: resetStateSchema,
});

export type EorIotDeviceControls = zod.infer<
  typeof eorIotDeviceControlsSchema
>;

export const eorIotDeviceMeasurementSchema = zod.object({
  tenant: zod.string(),
  deviceId: zod.string(),
  timestamp: zod.coerce.date(),
  voltage: zod.number(),
  current: zod.number(),
  temperature: zod.number(),
  coolingFans: zod.boolean(),
  heatsinkFans: zod.boolean(),
});

export type EorIotDeviceMeasurement = zod.infer<
  typeof eorIotDeviceMeasurementSchema
>;

export const eorIotDeviceStatusSchema = zod.object({
  tenant: zod.string(),
  deviceId: zod.string(),
  timestamp: zod.coerce.date(),
  mode: zod.number(),
  processFault: zod.number(),
  processFaults: zod.array(zod.string()),
  communicationFault: zod.number(),
  runState: runStateSchema,
  resetState: resetStateSchema,
  doorState: doorStateSchema,
  mainCircuitBreakerState: mainCircuitBreakerStateSchema,
  transformerContractorState: transformerContractorStateSchema,
  firstDiodeBridgeState: diodeBridgeStateSchema,
  secondDiodeBridgeState: diodeBridgeStateSchema,
});

export type EorIotDeviceStatus = zod.infer<
  typeof eorIotDeviceStatusSchema
>;

export const eorIotDeviceSummarySchema = zod.object({
  tenant: zod.string(),
  deviceId: zod.string(),
  lastMeasurement: eorIotDeviceMeasurementSchema,
  status: eorIotDeviceStatusSchema,
});

export type EorIotDeviceSummary = zod.infer<
  typeof eorIotDeviceSummarySchema
>;

export const eorIotDeviceDataSchema = zod.object({
  eorIotDeviceControls: eorIotDeviceControlsSchema,
  eorIotDeviceSummary: eorIotDeviceSummarySchema,
});

export type EorIotDeviceData = zod.infer<
  typeof eorIotDeviceDataSchema
>;
