export const yarnFmt = (output?: string | null) =>
  (output || "")
    .split("\n")
    .filter((line) => line)
    .map(
      (line) =>
        `${"yarn".padEnd(10)}: ${line.substring(line.indexOf(":") + 1)}`,
    )
    .join("\n");

export const dotnetFmt = (output?: string | null) =>
  `${"dotnet".padEnd(10)}: ${(output || "")
    .split("\n")
    .filter((line) => line)
    .map((line) => line.substring(line.indexOf(":") + 1).trim())
    .join("\n" + " ".repeat(12))}`;
