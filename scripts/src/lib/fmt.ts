export const yarnFmt = (output?: string | null) =>
  (output || "")
    .split("\n")
    .map(
      (line) =>
        `${"yarn".padEnd(10)}: ${line.substring(line.indexOf(":") + 1)}`,
    )
    .join("\n");

export const dotnetFmt = (output?: string | null) =>
  (output || "")
    .split("\n")
    .map(
      (line) =>
        `${"dotnet".padEnd(10)}: ${line
          .substring(line.indexOf(":") + 1)
          .trim()}`,
    )
    .join("\n");
