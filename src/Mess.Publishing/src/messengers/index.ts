import { MaybePromise } from "../types/promise";
import * as abb from "./abb";
import * as egauge from "./egauge";
import * as eor from "./eor";
import * as pidgeon from "./pidgeon";
import * as schneider from "./schneider";

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
  eor,
  egauge,
  pidgeon,
  abb,
  schneider,
});

export default Messenger;
