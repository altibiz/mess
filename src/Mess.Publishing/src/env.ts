import "dotenv/config";

export const mode: "client" | "server" | "proxy" =
  process.env.MESS_PUBLISHING_MODE === "client"
    ? "client"
    : process.env.MESS_PUBLISHING_MODE === "server"
    ? "server"
    : process.env.MESS_PUBLISHING_MODE === "proxy"
    ? "proxy"
    : "client";

export const modbusAddress: string =
  process.env.MESS_PUBLISHING_MODBUS_ADDRESS || "127.0.0.1";

export const modbusPort = process.env.MESS_PUBLISHING_MODBUS_PORT
  ? parseInt(process.env.MESS_PUBLISHING_MODBUS_PORT)
  : 502;

export const modbusUnitId = process.env.MESS_PUBLISHING_MODBUS_UNIT_ID
  ? parseInt(process.env.MESS_PUBLISHING_MODBUS_UNIT_ID)
  : 255;

export const tenant = process.env.MESS_PUBLISHING_TENANT || "eor";

export const proxyId = process.env.MESS_PUBLISHING_PROXY_ID || "dev-proxy";

export const pushBase =
  process.env.MESS_PUBLISHING_PUSH_BASE || "http://localhost:5000";

export const pushPath = process.env.MESS_PUBLISHING_PUSH_PATH || "/push";

export const updateBase =
  process.env.MESS_PUBLISHING_UPDATE_BASE || "http://localhost:5000";

export const updatePath = process.env.MESS_PUBLISHING_UPDATE_PATH || "/update";

export const pollBase =
  process.env.MESS_PUBLISHING_POLL_BASE || "http://localhost:5000";

export const pollPath = process.env.MESS_PUBLISHING_POLL_PATH || "/poll";

export const push = process.env.MESS_PUBLISHING_SIMULATE
  ? process.env.MESS_PUBLISHING_SIMULATE.split("|")
  : [""]; // messenger,pusher,interval|...

export const status = process.env.MESS_PUBLISHING_SIMULATE
  ? process.env.MESS_PUBLISHING_SIMULATE.split("|")
  : [""]; // messenger,pusher,approximateInterval|...
