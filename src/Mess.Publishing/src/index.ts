import client from "./client";
import server from "./server";
import proxy from "./proxy";
import args from "./args";

(async () => {
  if (args.mode === "server") {
    await server();
  } else if (args.mode === "proxy") {
    await proxy();
  } else {
    await client();
  }
})();
