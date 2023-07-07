import { Push, Update } from ".";
import args from "../args";

export const push: Push = async (handlerId: string, message: string) => {
  const endpoint = new URL(
    `/${args.tenant}${args.pushPath}?deviceId=${handlerId}`,
    args.pushBase,
  );

  const response = await fetch(endpoint.toString(), {
    method: "POST",
    headers: { "Content-Type": "text/plain" },
    body: message,
  });

  console.log(endpoint.toString(), response.status, response.statusText);
};

export const update: Update = async (handlerId: string, message: string) => {
  const endpoint = new URL(
    `/${args.tenant}${args.updatePath}?deviceId=${handlerId}`,
    args.updateBase,
  );

  const response = await fetch(endpoint.toString(), {
    method: "POST",
    headers: { "Content-Type": "text/plain" },
    body: message,
  });

  console.log(endpoint.toString(), response.status, response.statusText);
};
