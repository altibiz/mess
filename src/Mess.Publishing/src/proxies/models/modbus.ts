import * as zod from "zod";

export const modbusPollRequestScheme = zod.object({});

export const modbusPollResponseScheme = zod.object({
  values: zod.array(zod.number()),
});

export const modbusPushRequestScheme = zod.object({
  values: zod.array(zod.number()),
});

export const modbusPushResponseScheme = zod.object({});
