import * as fs from "fs/promises";
import * as os from "os";

export const distro = async () => {
  const platform = os.platform();
  if (platform !== "linux") {
    return;
  }

  let release: string | undefined;
  try {
    release = await fs.readFile("/etc/os-release", { encoding: "utf-8" });
    return release
      .split("\n")
      .filter((p) => p.startsWith("NAME"))[0]
      .split("=")[1];
  } catch {
    return;
  }
};
