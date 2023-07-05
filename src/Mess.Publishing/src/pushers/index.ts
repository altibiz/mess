export type Push = (handlerId: string, message: string) => Promise<void> | void;

export type Update = (
  handlerId: string,
  message: string,
) => Promise<void> | void;

export type Setup = () => Promise<void> | void;

export type Teardown = () => Promise<void> | void;

type Pusher = {
  push: Push;
  update: Update;
  setup?: Setup;
  teardown?: Teardown;
};

export const importPushers = async (): Promise<Record<string, Pusher>> => ({
  http: await import("./http"),
});

export default Pusher;
