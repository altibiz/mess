import * as zod from "zod";
import {
  modbusPollRequestScheme,
  modbusPollResponseScheme,
  modbusPushRequestScheme,
  modbusPushResponseScheme,
} from "./modbus";

export const pollMetadataScheme = zod.object({
  handlerId: zod.string(),
  tenant: zod.string(),
  deviceId: zod.string(),
  timestamp: zod.date(),
});

export const pushMetadataScheme = zod.object({
  dispatcherId: zod.string(),
  tenant: zod.string(),
  deviceId: zod.string(),
  timestamp: zod.date(),
});

export type PollMetadata = typeof pollMetadataScheme._type;

export type PushMetadata = typeof pushMetadataScheme._type;

export const pollRequestScheme = zod.intersection(
  pollMetadataScheme,
  modbusPollRequestScheme,
);

export const pollResponseScheme = zod.intersection(
  pollMetadataScheme,
  modbusPollResponseScheme,
);

export const pushRequestScheme = zod.intersection(
  pushMetadataScheme,
  modbusPushRequestScheme,
);

export const pushResponseScheme = zod.intersection(
  pushMetadataScheme,
  modbusPushResponseScheme,
);

export type PollRequest = typeof pollRequestScheme._type;

export type PollResponse = typeof pollResponseScheme._type;

export type PushRequest = typeof pushRequestScheme._type;

export type PushResponse = typeof pushResponseScheme._type;
