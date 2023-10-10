import { MaybePromise } from "../types/promise";

export type Message = {
  metadata: Record<string, unknown>;
  payload: string;
};

export type Create = (template: string) => MaybePromise<Message>;

type Messenger = {
  push?: Create;
  update?: Create;
};

export const importMessengers = async (): Promise<
  Record<string, Messenger>
> => ({
  eor: await import("./eor"),
  egauge: await import("./egauge"),
  pidgeon: await import("./pidgeon"),
  abb: await import("./abb"),
});

export default Messenger;
