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

export const eorMeasurementDeviceControlsSchema = zod.object({
  mode: zod.number(),
  runState: runStateSchema,
  resetState: resetStateSchema,
});

export type EorMeasurementDeviceControls = zod.infer<
  typeof eorMeasurementDeviceControlsSchema
>;

export const eorMeasurementDeviceMeasurementSchema = zod.object({
  tenant: zod.string(),
  deviceId: zod.string(),
  timestamp: zod.coerce.date(),
  voltage: zod.number(),
  current: zod.number(),
  temperature: zod.number(),
  coolingFans: zod.boolean(),
  heatsinkFans: zod.boolean(),
});

export type EorMeasurementDeviceMeasurement = zod.infer<
  typeof eorMeasurementDeviceMeasurementSchema
>;

export const eorMeasurementDeviceStatusSchema = zod.object({
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

export type EorMeasurementDeviceStatus = zod.infer<
  typeof eorMeasurementDeviceStatusSchema
>;

export const eorMeasurementDeviceSummarySchema = zod.object({
  tenant: zod.string(),
  deviceId: zod.string(),
  lastMeasurement: eorMeasurementDeviceMeasurementSchema,
  status: eorMeasurementDeviceStatusSchema,
});

export type EorMeasurementDeviceSummary = zod.infer<
  typeof eorMeasurementDeviceSummarySchema
>;

export const eorMeasurementDeviceDataSchema = zod.object({
  eorMeasurementDeviceControls: eorMeasurementDeviceControlsSchema,
  eorMeasurementDeviceSummary: eorMeasurementDeviceSummarySchema,
});

export type EorMeasurementDeviceData = zod.infer<
  typeof eorMeasurementDeviceDataSchema
>;
