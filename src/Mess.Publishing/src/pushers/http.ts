import { Log, Push, Update } from "./index";
import { Message } from "../messengers/index";
import args from "../args";
import fetch, { Response } from "cross-fetch";

export const push: Push = async ({ payload: body, metadata }: Message) => {
  const endpoint = new URL(
    typeof metadata.deviceId === "string"
      ? `/${args.tenant}${args.pushPath}/${metadata.deviceId}`
      : `/${args.tenant}${args.pushPath}`,
    args.pushBase,
  );

  const headers: Record<string, string> = {};
  if (typeof metadata.contentType === "string") {
    headers["Content-Type"] = metadata.contentType;
  }
  if (typeof metadata.apiKey === "string") {
    headers["X-API-Key"] = metadata.apiKey;
  }

  const response = await fetch(endpoint.toString(), {
    method: "POST",
    headers,
    body,
  });

  return response;
};

export const update: Update = async ({ payload: body, metadata }: Message) => {
  const endpoint = new URL(
    typeof metadata.deviceId === "string"
      ? `/${args.tenant}${args.updatePath}/${metadata.deviceId}`
      : `/${args.tenant}${args.updatePath}`,
    args.updateBase,
  );

  const headers: Record<string, string> = {};
  if (typeof metadata.contentType === "string") {
    headers["Content-Type"] = metadata.contentType;
  }
  if (typeof metadata.apiKey === "string") {
    headers["X-API-Key"] = metadata.apiKey;
  }

  const response = await fetch(endpoint.toString(), {
    method: "POST",
    headers,
    body,
  });

  return response;
};

export const log: Log = async (response, logger) => {
  if (!(response instanceof Response)) {
    return;
  }

  logger(response.url, response.status, response.statusText);
};
