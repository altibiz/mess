import { DateTime } from "luxon";
import { importTemplates } from "../templates";
import { push as abbPush } from "./abb";
import compile from "./compile";
import { Create } from "./index";
import { push as schneiderPush } from "./schneider";

const allowedTemplates = { abb: abbPush, schneider: schneiderPush };
let templates: Awaited<ReturnType<typeof importTemplates>> | null = null;

export const push: Create = async (template) => {
  if (!templates) {
    templates = await importTemplates();
  }

  const data = {
    timestamp: `"${DateTime.utc().toISO()}"`,
    measurements: JSON.stringify(
      (
        await Promise.all(
          Object.entries(allowedTemplates).map(async ([id, push]) => {
            const template = templates?.[id]?.push;
            if (!template) {
              return null;
            }

            const {
              metadata: { deviceId },
              payload,
            } = await push(template);
            return {
              deviceId,
              timestamp: DateTime.utc().toISO(),
              data: payload,
            };
          }),
        )
      ).filter((measurement) => measurement),
      null,
      2,
    ),
  };

  return {
    metadata: {
      contentType: "application/json",
      deviceId: "pidgeon",
      apiKey: "pidgeon",
    },
    payload: compile(template, data),
  };
};
