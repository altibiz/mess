export type MaybePromise<T> = PromiseLike<T> | T;

export type Empty = Record<string, never>;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export type PrettyObject<TLocal> = TLocal extends infer TLocal
  ? {
      [K in keyof TLocal]: TLocal[K] extends Empty
        ? PrettyObject<TLocal[K]>
        : TLocal[K];
    }
  : never;
