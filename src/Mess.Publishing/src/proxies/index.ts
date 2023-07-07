export type Run = () => Promise<void> | void;

export type Setup = () => Promise<void> | void;

export type Teardown = () => Promise<void> | void;

type Proxy = {
  run: Run;
  setup?: Setup;
  teardown?: Teardown;
};

export const importProxies = async (): Promise<Record<string, Proxy>> => ({
  modbus: await import("./modbus"),
});

export default Proxy;
