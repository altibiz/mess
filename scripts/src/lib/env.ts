import { env as processEnv } from "process";
import { config as configDotenv } from "dotenv";
import { exists, root } from "./fs";

export const env = (key: string, value?: string) => {
  if (value) {
    processEnv[key] = value;
  } else {
    return processEnv[key];
  }
};

export const dotenv = async (path: string) => {
  if (await exists(path)) {
    configDotenv({ path: root(path) });
  }
};
