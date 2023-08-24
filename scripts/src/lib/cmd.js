import { log } from "./log";
import process from "process";

export const cmd = (handlerOrDescriptor) =>
  typeof handlerOrDescriptor === "function"
    ? {
        usage: "",
        description: "",
        builder: (_) => _,
        handler: async (args) => {
          try {
            await handlerOrDescriptor(args);
          } catch (error) {
            log.error(error);
            process.exit(1);
          }
        },
      }
    : (handler) => ({
        ...handlerOrDescriptor,
        handler: async (args) => {
          try {
            await handler(args);
          } catch (error) {
            log.error(error);
            process.exit(1);
          }
        },
      });
