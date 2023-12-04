import { IServiceVector, ServerTCP } from "modbus-serial";
import { Run } from "./index";
import args from "../args";
import { poll, push } from "./forward";
import { pollResponseScheme } from "./models/index";
import luxon from "luxon";

export const run: Run = async () => {
  await new Promise<void>((_, reject) => {
    modbusServer = new ServerTCP(vector, {
      host: args.modbusAddress,
      port: args.modbusPort,
      unitID: args.modbusUnitId,
    });

    modbusServer.on("error", (err) => {
      if (err) {
        console.error("Modbus server error: %O", err);
        return;
      }
    });

    modbusServer.on("socketError", (err) => {
      if (err) {
        console.error("Modbus server socket error: %O", err);
        return;
      }
    });

    modbusServer.on("initialized", (err) => {
      console.log("Modbus proxy initialized");
      if (err) {
        reject(err);
        return;
      }
    });
  });
};

let modbusServer: ServerTCP | undefined;

const vector: IServiceVector = {
  getMultipleHoldingRegisters: (
    addr: number,
    length: number,
    unitID: number,
    callback: (err: Error | null, value: number[]) => void,
  ) => {
    console.log("getMultipleHoldingRegisters", addr, length, unitID);

    if (addr < 400001 || addr >= 465535 || length > 120 || unitID !== 255) {
      callback(new Error("Received invalid modbus poll request"), []);
      return;
    }

    poll(`${args.proxyId}_${addr}`, {
      handlerId: `${args.proxyId}_${addr}`,
      tenant: args.tenant,
      deviceId: `${args.proxyId}_${addr}`,
      timestamp: luxon.DateTime.utc().toJSDate(),
    })
      .then(async (response) => {
        const parsedResponse =
          await pollResponseScheme.safeParseAsync(response);
        if (!parsedResponse.success) {
          console.error(
            "Received invalid modbus poll response",
            parsedResponse.error,
          );
          callback(parsedResponse.error, []);
          return;
        }
        const validResponse = parsedResponse.data;

        callback(null, validResponse.values);
      })
      .catch((error) => callback(error, []));
  },

  setRegisterArray: (
    addr: number,
    value: number[],
    unitID: number,
    callback: (err: Error | null) => void,
  ) => {
    console.log("setRegisterArray", addr, value, unitID);

    if (
      addr < 400001 ||
      addr >= 465535 ||
      value.length > 120 ||
      unitID !== 255
    ) {
      callback(new Error("Received invalid modbus push request"));
      return;
    }

    push(`${args.proxyId}_${addr}`, {
      dispatcherId: `${args.proxyId}_${addr}`,
      tenant: args.tenant,
      deviceId: `${args.proxyId}_${addr}`,
      timestamp: luxon.DateTime.utc().toJSDate(),
      values: value,
    })
      .then(() => callback(null))
      .catch(callback);
  },
};
