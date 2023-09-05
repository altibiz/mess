import { importTemplates } from "../templates";
import compile from "./compile";
import { push as egaugePush } from "./egauge";
import { Create } from "./index";

const allowedTemplates = { egauge: egaugePush };
let templates: Awaited<ReturnType<typeof importTemplates>> | null = null;

export const push: Create = async (template) => {
  if (!templates) {
    templates = await importTemplates();
  }

  const data = {
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
            return { deviceId, request: payload };
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
      deviceId: "raspberryPi",
      apiKey: "raspberryPi",
    },
    payload: compile(template, data),
  };
};
