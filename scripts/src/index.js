#!/usr/bin/env bun run

import commands from "./cmds/index";
import { buildGlobalArgs, log, SUBCOMMANDS } from "./lib/index";

const addCommand = (args, name, command) =>
  command.usage && command.description
    ? args.command(
        command.usage,
        command.description,
        command.builder ?? ((_) => _),
        (...args) => {
          log.info(`Starting ${name} script...`);
          command.handler(...args);
        },
      )
    : args.command(
        name,
        name,
        (_) => _,
        (...args) => {
          log.info(`Starting ${name} script...`);
          command.handler(...args);
        },
      );

Object.entries(commands)
  .reduce(
    (args, [name, command]) =>
      command[SUBCOMMANDS]
        ? args.command(command.usage, command.description, (args) =>
            Object.entries(command[SUBCOMMANDS]).reduce(
              (args, [name, command]) => addCommand(args, name, command),
              args,
            ),
          )
        : addCommand(args, name, command),
    buildGlobalArgs(),
  )
  .parseAsync();
