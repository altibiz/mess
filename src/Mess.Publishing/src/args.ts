import yargs from "yargs/yargs";
import { hideBin } from "yargs/helpers";
import * as env from "./env";

// TODO: do i care about most of these arguments in dev?

export type Args = {
  mode: "client" | "server" | "proxy";
  modbusAddress: string;
  modbusPort: number;
  modbusUnitId: number;
  tenant: string;
  proxyId: string;
  pushBase: string;
  pushPath: string;
  updateBase: string;
  updatePath: string;
  pollBase: string;
  pollPath: string;
  push: {
    messengerId: string;
    pusherId: string;
    interval?: number;
  }[];
  status: {
    messengerId: string;
    pusherId: string;
    approximateInterval?: number;
  }[];
};

const unparsedArgs = yargs(hideBin(process.argv))
  .usage("Usage: yarn workspace @mess/publisher start [options]")
  .option("mode", {
    describe: "The mode in which to run",
    choices: ["client", "server", "proxy"],
    default: env.mode,
  })
  .option("modbusAddress", {
    describe:
      "The address at which listen for Modbus requests (only for proxy and server modes)",
    default: env.modbusAddress,
  })
  .option("modbusPort", {
    describe:
      "The port at which to listen for Modbus requests (only for proxy and server modes)",
    number: true,
    default: env.modbusPort,
  })
  .option("modbusUnitId", {
    describe:
      "The unit ID at which to listen for Modbus requests (only for proxy and server modes)",
    number: true,
    default: env.modbusUnitId,
  })
  .option("tenant", {
    describe:
      "The tenant to which to publish measurements (only for proxy and client mode)",
    default: env.tenant,
  })
  .option("proxyId", {
    describe: "The ID of the proxy to run (only for proxy and client mode)",
    default: env.proxyId,
  })
  .option("pushBase", {
    describe: "The base URI at which to push (only for proxy and client mode)",
    default: env.pushBase,
  })
  .option("pushPath", {
    describe: "The path at which to push (only for proxy and client mode)",
    default: env.pushPath,
  })
  .option("updateBase", {
    describe:
      "The base URI at which to update (only for proxy and client mode)",
    default: env.updateBase,
  })
  .option("updatePath", {
    describe: "The path at which to update (only for proxy and client mode)",
    default: env.updatePath,
  })
  .option("pollBase", {
    describe: "The base URI at which to poll (only for proxy and client mode)",
    default: env.pollBase,
  })
  .option("pollPath", {
    describe: "The path at which to poll (only for proxy and client mode)",
    default: env.pollPath,
  })
  .option("push", {
    describe: "The pushers to run (only for client mode)",
    type: "array",
    string: true,
    demandOption: true,
    default: env.push,
  })
  .option("status", {
    describe: "The status updaters to run (only for client mode)",
    type: "array",
    string: true,
    demandOption: true,
    default: env.status,
  });

const unstructuredArgs = unparsedArgs.parseSync();

const args = {
  mode: unstructuredArgs.mode,
  tenant: unstructuredArgs.tenant,
  proxyId: unstructuredArgs.proxyId,
  pushBase: unstructuredArgs.pushBase,
  pushPath: unstructuredArgs.pushPath,
  updateBase: unstructuredArgs.updateBase,
  updatePath: unstructuredArgs.updatePath,
  pollBase: unstructuredArgs.pollBase,
  pollPath: unstructuredArgs.pollPath,
  modbusAddress: unstructuredArgs.modbusAddress,
  modbusPort: unstructuredArgs.modbusPort,
  modbusUnitId: unstructuredArgs.modbusUnitId,
  push: unstructuredArgs.push.map((unparsed) => {
    const array = unparsed.split(",");
    return {
      messengerId: array[0],
      pusherId: array[1],
      interval: array[2] === "single" ? undefined : parseInt(array[2]),
    };
  }),
  status: unstructuredArgs.status.map((unparsed) => {
    const array = unparsed.split(",");
    return {
      messengerId: array[0],
      pusherId: array[1],
      approximateInterval:
        array[2] === "single" ? undefined : parseInt(array[2]),
    };
  }),
} as Args;

export default args;
