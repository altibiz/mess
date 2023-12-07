import { MaybePromise } from "../types/promise";
import * as modbus from "./modbus";

export type Serve = () => MaybePromise<void>;

export type Setup = () => MaybePromise<void>;

export type Teardown = () => MaybePromise<void>;

type Server = {
  serve: Serve;
  setup?: Setup;
  teardown?: Teardown;
};

export const importServers = async (): Promise<Record<string, Server>> => ({
  modbus,
});

export default Server;
