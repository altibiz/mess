import { log } from "./log";
import kill from "tree-kill";
import {
  ChildProcessWithoutNullStreams,
  ExecOptions,
  SpawnOptionsWithoutStdio,
  exec as execWithCallbacks,
  spawn as spawnWithCallbacks,
} from "child_process";
import { hasValue } from "./string";
import { rootDir } from "./dirs";

type TaskObject = {
  name: string;
  command: string;
  exit?: boolean;
  silent?: boolean;
  unary?: boolean;
  verbose?: boolean;
  fmt?: (output: string) => string;
  cwd?: string;
};

type Task = string | TaskObject;

export const task = async (message: string, ...commands: Task[]) => {
  if (!commands) {
    return;
  }

  const cmds: Record<string, { stdout?: string; stderr?: string }> = {};
  for (const command of commands) {
    const {
      exit,
      command: toExec,
      name,
      cwd,
      unary,
      silent,
    } = toObject(command);

    let result: { stdout?: string; stderr?: string } | null = null;
    if (exit) {
      result = await exec(toExec, { cwd: cwd ?? rootDir });
    } else {
      try {
        result = await exec(toExec);
      } catch (error) {
        result = { stderr: `Exited with: ${(error as { code: number }).code}` };
      }
    }
    if (unary) {
      result = { stdout: `${result?.stdout ?? ""}\n${result?.stdout ?? ""}` };
    }

    if (!silent && (hasValue(result.stdout) || hasValue(result.stderr))) {
      cmds[name] = result;
    }
  }

  log.info({
    message,
    cmds,
  });
};

export const ptask = async (...commands: Task[]) => {
  if (!commands) {
    return;
  }

  log.info({
    message: `Starting ${commands
      .map((command) => toObject(command).name)
      .join(", ")}...`,
  });

  const children: Record<string, Child> = {};
  const childrenExited = () => {
    for (const { exited } of Object.values(children)) {
      if (!exited) {
        return false;
      }
    }

    return true;
  };
  const stop = async () => {
    while (!childrenExited()) {
      for (const { name, exited, process, verbose } of Object.values(
        children,
      )) {
        if (!exited) {
          kill(process.pid!, "SIGINT", (error) => {
            if (error && verbose) {
              log.error({
                message: "Failed to kill",
                pcmd: [
                  name,
                  {
                    error: error.toString(),
                  },
                ],
              });
            }
          });
        }
      }

      await sleep(1000);
    }
    process.removeListener("SIGINT", stop);
  };
  process.addListener("SIGINT", stop);

  await Promise.all(
    commands.map(async (command) => {
      const commandObject = toObject(command);
      const child = await spawn(commandObject.command, {
        cwd: commandObject.cwd ?? rootDir,
      });
      children[commandObject.name] = {
        ...commandObject,
        process: child,
        exited: false,
      };
    }),
  );

  for (const { name, exit, process, silent, fmt, unary } of Object.values(
    children,
  )) {
    if (!silent) {
      process.stdout.on("data", (data) => {
        const stdout = data.toString();
        if (hasValue(stdout)) {
          log.info({
            message: "STDOUT from",
            pcmd: [
              name,
              {
                stdout,
                fmt,
              },
            ],
          });
        }
      });

      process.stderr.on("data", (data) => {
        const stderr = data.toString();
        if (hasValue(stderr)) {
          log.info({
            message: unary ? "STDOUT from" : "STDERR from",
            pcmd: [
              name,
              {
                [unary ? "stdout" : "stderr"]: stderr,
                fmt,
              },
            ],
          });
        }
      });
    }

    process.on("error", (error) => {
      log.error({
        message: "Failed to start",
        pcmd: [
          name,
          {
            error: error.toString(),
          },
        ],
      });

      if (exit) {
        stop();
      }
    });
  }

  await Promise.all(
    Object.values(children).map(
      ({ process, name, exit, verbose }) =>
        new Promise<void>((resolve) => {
          process.on("exit", async (code, signal) => {
            if (code !== 0 && verbose) {
              log.error({
                message: "Exit from",
                pcmd: [
                  name,
                  {
                    error: `${code ?? "null"} ${signal ?? "null"}`,
                  },
                ],
              });
            }

            children[name].exited = true;
            if (exit) {
              await stop();
            }

            resolve();
          });
        }),
    ),
  );
};

type Child = TaskObject & {
  process: ChildProcessWithoutNullStreams;
  exited: boolean;
};

const toObject = (command: Task): TaskObject => {
  if (typeof command === "string") {
    return {
      name: command,
      command,
      exit: true,
    };
  } else {
    return {
      ...command,
      exit: command.exit ?? true,
    };
  }
};

const exec = (
  command: string,
  options?: ExecOptions,
): Promise<{ stdout?: string; stderr?: string }> => {
  return new Promise((resolve, reject) => {
    execWithCallbacks(command, options, (error, stdout, stderr) => {
      if (error) {
        reject(error);
      } else {
        resolve({ stdout: stdout.toString(), stderr: stderr.toString() });
      }
    });
  });
};

const spawn = (
  command: string,
  options?: SpawnOptionsWithoutStdio,
): Promise<ChildProcessWithoutNullStreams> => {
  return new Promise((resolve, reject) => {
    const child = spawnWithCallbacks(command, { shell: true, ...options });

    child.on("spawn", () => {
      resolve(child);
    });

    child.on("error", (error) => {
      reject(error);
    });
  });
};

const sleep = async (ms: number) => {
  return await new Promise((resolve) => setTimeout(resolve, ms));
};
