import { importProxies } from "./proxies/index";

const proxy = async () => {
  const proxies = await importProxies();

  for (const [serverId, { setup }] of Object.entries(proxies)) {
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
    for (const [serverId, { teardown }] of Object.entries(proxies)) {
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
    Object.entries(proxies).map(async ([serverId, { run }]) => {
      try {
        await run();
      } catch (err) {
        console.error("Exception while serving %s: %O", serverId, err);
      }
    }),
  );
};

export default proxy;
