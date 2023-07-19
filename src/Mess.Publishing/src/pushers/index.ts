import { Message } from "../messengers";
import { MaybePromise } from "../types/promise";

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
  http: await import("./http"),
});

export default Pusher;
