import { importServers } from "./servers/index";

const server = async () => {
  const servers = await importServers();

  for (const [serverId, { setup }] of Object.entries(servers)) {
    if (setup) {
      try {
        await setup();
      } catch (err) {
        console.error("Failed to setup server %s: %O", serverId, err);
        return;
      }
    }
  }
  process.on("SIGINT", async () => {
    for (const [serverId, { teardown }] of Object.entries(servers)) {
      if (teardown) {
        try {
          await teardown();
        } catch (err) {
          console.error("Failed to teardown server %s: %O", serverId, err);
        }
      }
    }
    process.exit(0);
  });

  await Promise.all(
    Object.entries(servers).map(async ([serverId, { serve }]) => {
      try {
        await serve();
      } catch (err) {
        console.error("Exception while serving %s: %O", serverId, err);
      }
    }),
  );
};

export default server;
