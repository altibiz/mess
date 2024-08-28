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

  const now = DateTime.utc();

  const data = {
    timestamp: `"${now.toISO()}"`,
    measurements: JSON.stringify(
      (
        await Promise.all(
          Object.entries(allowedTemplates).flatMap(([id, push]) =>
            [...Array(1000 / Object.keys(allowedTemplates).length).keys()].map(
              async () => {
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
                  timestamp: now.toISO(),
                  data: JSON.parse(payload),
                };
              },
            ),
          ),
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
