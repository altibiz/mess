export const yarnFmt = (output: string) =>
  output
    .split("\n")
    .filter((line) => line)
    .map(
      (line) =>
        `${"yarn".padEnd(10)}: ${line.substring(line.indexOf(":") + 1)}`,
    )
    .join("\n");

export const dotnetFmt = (output: string) =>
  `${"dotnet".padEnd(10)}: ${output
    .split("\n")
    .map((line) => line.substring(line.indexOf(":") + 1).trim())
    .join(" ")}`;
