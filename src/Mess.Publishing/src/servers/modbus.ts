import { IServiceVector, ServerTCP } from "modbus-serial";
import { Serve } from "./index";
import args from "../args";

export const serve: Serve = async () => {
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
      if (err) {
        reject(err);
        return;
      }
    });
  });
};

let modbusServer: ServerTCP | undefined;

const vector: IServiceVector = {
  getCoil: (addr: number, unitID: number) => {
    console.log("getCoil", addr, unitID);
  },
  getDiscreteInput: (addr: number, unitID: number) => {
    console.log("getDiscreteInput", addr, unitID);
  },
  getInputRegister: (addr: number, unitID: number) => {
    console.log("getInputRegister", addr, unitID);
  },
  getHoldingRegister: (addr: number, unitID: number) => {
    console.log("getHoldingRegister", addr, unitID);
  },
  getMultipleInputRegisters: (addr: number, length: number, unitID: number) => {
    console.log("getMultipleInputRegisters", addr, length, unitID);
  },
  getMultipleHoldingRegisters: (
    addr: number,
    length: number,
    unitID: number,
  ) => {
    console.log("getMultipleHoldingRegisters", addr, length, unitID);
  },
  setCoil: (addr: number, value: boolean, unitID: number) => {
    console.log("setCoil", addr, value, unitID);
  },
  setRegister: (addr: number, value: number, unitID: number) => {
    console.log("setRegister", addr, value, unitID);
  },
  setRegisterArray: (addr: number, value: number[], unitID: number) => {
    console.log("setRegisterArray", addr, value, unitID);
  },
};
