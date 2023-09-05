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
  raspberryPi: await import("./raspberryPi"),
});

export default Messenger;
