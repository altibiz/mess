import { Message } from "../messengers/index";
import type { MaybePromise } from "../types/promise";
import * as http from "./http";

export type Push = (message: Message) => MaybePromise<unknown>;

export type Update = (message: Message) => MaybePromise<unknown>;

export type Log = (
  response: unknown,
  logger: (...values: unknown[]) => MaybePromise<void>,
) => MaybePromise<void>;

export type Setup = () => Promise<void> | void;

export type Teardown = () => Promise<void> | void;

type Pusher = {
  push: Push;
  update: Update;
  log: Log;
  setup?: Setup;
  teardown?: Teardown;
};

export const importPushers = async (): Promise<Record<string, Pusher>> => ({
  http,
});

export default Pusher;
