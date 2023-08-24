import args from "../args";
import {
  PollRequest,
  PollResponse,
  PushRequest,
  PushResponse,
  pollResponseScheme,
  pushResponseScheme,
} from "./models/index";
import fetch from "cross-fetch";

const forward = async (uri: string, body: unknown): Promise<unknown> => {
  const response = await fetch(uri, {
    method: "POST",
    body: JSON.stringify(body),
    headers: {
      "Content-Type": "application/json",
    },
  });
  const json = await response.json();

  return json;
};

export const poll = async (
  handlerId: string,
  request: PollRequest,
): Promise<PollResponse> => {
  const endpoint = new URL(
    `/${args.tenant}${args.pollPath}?handlerId=${handlerId}`,
    args.pollBase,
  );

  const response = await forward(endpoint.toString(), request);
  const parsedResponse = await pollResponseScheme.safeParseAsync(response);
  if (!parsedResponse.success) {
    throw new Error(`Failed parsing poll response for ${handlerId}`);
  }

  return parsedResponse.data;
};

export const push = async (
  dispatcherId: string,
  request: PushRequest,
): Promise<PushResponse> => {
  const endpoint = new URL(
    `/${args.tenant}${args.pushPath}?dispatcherId=${dispatcherId}`,
    args.pushBase,
  );
  const response = await forward(endpoint.toString(), request);
  const parsedResponse = await pushResponseScheme.safeParseAsync(response);
  if (!parsedResponse.success) {
    throw new Error(`Failed parsing poll response for ${dispatcherId}`);
  }

  return parsedResponse.data;
};
