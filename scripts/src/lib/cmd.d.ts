import { Argv, ArgumentsCamelCase } from "yargs";
import type { Empty, PrettyObject } from "./types";

type CommandBuilder<TInherited = Empty, TLocal = Empty> = (
  args: Argv<TInherited>,
) => Argv<TLocal>;

type CommandHandler<TLocal = Empty> = (
  args: ArgumentsCamelCase<TLocal>,
) => Promise<void>;

type InitialCommandHandler<TLocal = Empty> = (
  args: ArgumentsCamelCase<TLocal>,
) => MaybePromise<void>;

type PrettyCommandHandler<TLocal = Empty> = (
  args: PrettyObject<ArgumentsCamelCase<TLocal>>,
) => MaybePromise<void>;

type CommandDescriptor<TInherited = Empty, TLocal = Empty> = {
  usage: string;
  description: string;
  builder?: CommandBuilder<TInherited, TLocal>;
};

type Command<TInherited = Empty, TLocal = Empty> = CommandDescriptor<
  TInherited,
  TLocal
> & {
  handler: CommandHandler<TLocal>;
};

export declare function cmd(
  handler: InitialCommandHandler<GlobalArgs>,
): Command<GlobalArgs, GlobalArgs>;

export declare function cmd<TLocal extends GlobalArgs = GlobalArgs>(
  descriptor: CommandDescriptor<GlobalArgs, TLocal>,
): (handler: PrettyCommandHandler<TLocal>) => Command<GlobalArgs, TLocal>;
