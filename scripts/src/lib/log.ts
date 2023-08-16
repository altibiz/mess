import winston from "winston";

const colorizer = winston.format.colorize();

export const log = winston.createLogger({
  level: "info",
  format: winston.format.combine(
    winston.format.ms(),
    winston.format.printf(({ level, message, ms, cmds, pcmd }) => {
      let transparent = false;

      let pcmdString = "";
      if (pcmd) {
        const [name, { stdout, stderr, error, fmt }] = pcmd;
        pcmdString = error
          ? `${name}:\n${colorizer.colorize("error", error.trim())}\n\n`
          : stderr
          ? colorizer.colorize(
              "error",
              fmt ? fmt(stdout) : formatTransparent(name, stderr),
            )
          : stdout
          ? fmt
            ? fmt(stdout)
            : formatTransparent(name, stdout)
          : "";
        transparent = !error;
      }

      let cmdsString = "";
      if (cmds) {
        const cmdEntries = Object.entries<{ stdout: string; stderr: string }>(
          cmds,
        );
        cmdsString =
          cmdEntries.length === 1
            ? cmdEntries[0][1].stderr
              ? `:\n${colorizer.colorize(
                  "error",
                  cmdEntries[0][1].stderr.trim(),
                )}\n\n`
              : cmdEntries[0][1].stdout
              ? `:\n${cmdEntries[0][1].stdout.trim()}\n\n`
              : `: Done!\n\n`
            : cmdEntries.reduce(
                (curr, [name, { stdout, stderr }]) =>
                  curr +
                  (stderr
                    ? `${name}:\n${colorizer.colorize(
                        "error",
                        stderr.trim(),
                      )}\n\n`
                    : stdout
                    ? `${name}:\n${stdout.trim()}\n\n`
                    : `${name}: Done!\n\n`),
                "\n\n",
              );
      }

      const msString = ms === "+0ms" || pcmd ? "" : ` (${ms})`;
      const extended = pcmdString
        ? `${pcmdString}`
        : cmdsString
        ? `${cmdsString}`
        : "\n";

      return transparent
        ? extended
        : `${colorizer.colorize(
            level,
            "[mess]",
          )}${msString}: ${message}${extended}`;
    }),
  ),
  transports: [new winston.transports.Console()],
});

const formatTransparent = (name: string, lines: string) => {
  return lines
    .trim()
    .split("\n")
    .map((line: string) => `${name.padEnd(10)}: ${line}`)
    .join("\n");
};
