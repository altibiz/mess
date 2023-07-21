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
  egauge: await import("./egauge"),
  eor: await import("./eor"),
});

export default Messenger;
