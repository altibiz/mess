export type Create = (
  template: string,
) => PromiseLike<string | string[]> | string | string[];

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
