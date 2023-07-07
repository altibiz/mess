export type Serve = () => Promise<void> | void;

export type Setup = () => Promise<void> | void;

export type Teardown = () => Promise<void> | void;

type Server = {
  serve: Serve;
  setup?: Setup;
  teardown?: Teardown;
};

export const importServers = async (): Promise<Record<string, Server>> => ({
  http: await import("./modbus"),
});

export default Server;
