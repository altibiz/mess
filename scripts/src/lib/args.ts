import { argv } from "process";
import { hideBin } from "yargs/helpers";
import yargs, { type Argv as YargsArgv } from "yargs";

export const buildGlobalArgs = () => {
  const args = yargs(hideBin(argv));
  const terminalWidth = args.terminalWidth();
  return args.scriptName("mess").help().wrap(terminalWidth);
};

export type GlobalArgs = ReturnType<typeof buildGlobalArgs>["argv"];

export type Argv<T> = YargsArgv<GlobalArgs & T>;

export const coerceRegex =
  (regex: RegExp, error?: string) => (value: string) => {
    if (!value.match(regex)) {
      throw new Error(
        error ? `${error}\nGiven: '${value}'` : `Invalid value: '${value}'`,
      );
    }

    return value;
  };

export const coerceValue =
  <T>() =>
  (value: unknown) => {
    return value as T;
  };

export const coerceValues =
  <T>() =>
  (values: unknown[]) => {
    return values as T[];
  };

export const SUBCOMMANDS = Symbol("commands");
