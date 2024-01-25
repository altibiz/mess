import { DateTime } from "luxon";
import args from "./args";
import { importMessengers } from "./messengers/index";
import { importPushers } from "./pushers/index";
import { importTemplates } from "./templates/index";

type Interval = NodeJS.Timeout;
type Timeout = NodeJS.Timeout;

type IntervalRef = { interval?: Interval };
type TimeoutRef = { timeout?: Timeout };

const run = async () => {
  const templates = await importTemplates();
  const messengers = await importMessengers();
  const pushers = await importPushers();
  const activePushers = Object.entries(pushers).filter(
    ([pusherId]) =>
      args.push.map(({ pusherId }) => pusherId).includes(pusherId) ||
      args.update.map(({ pusherId }) => pusherId).includes(pusherId),
  );

  let intervalRefs: IntervalRef[] = [];
  let timeoutRefs: TimeoutRef[] = [];
  for (const [pusherId, { setup }] of activePushers) {
    if (setup) {
      try {
        await setup();
      } catch (err) {
        console.error("Failed to setup pusher %s: %O", pusherId, err);
        return;
      }
    }
  }
  process.on("SIGINT", async () => {
    for (const intervalRef of intervalRefs) {
      if (intervalRef.interval) {
        clearInterval(intervalRef.interval);
      }
    }
    for (const timeoutRef of timeoutRefs) {
      if (timeoutRef.timeout) {
        clearTimeout(timeoutRef.timeout);
      }
    }
    for (const [pusherId, { teardown }] of activePushers) {
      if (teardown) {
        try {
          await teardown();
        } catch (err) {
          console.error("Failed to teardown pusher %s: %O", pusherId, err);
        }
      }
    }

    process.exit(0);
  });

  intervalRefs = args.push.map(({ messengerId, pusherId, interval }) => {
    const messenger = messengers[messengerId]?.push;
    const template = templates[messengerId]?.push;
    if (!messenger || !template) {
      console.error("Messenger %s not found", messengerId);
      return {};
    }

    const pusher = pushers[pusherId];
    if (!pusher) {
      console.error("Pusher %s not found", pusherId);
      return {};
    }

    const execute = async () => {
      try {
        const message = await messenger(template);
        const response = await pusher.push(message);
        const start = DateTime.now();
        await pusher.log(response, (...args) => {
          console.log(...args, `took ${start.diffNow().toHuman()}`);
        });
      } catch (err) {
        console.error(
          "Failed to push %s message via %s: %O",
          messengerId,
          pusherId,
          err,
        );
      }
    };

    if (!interval) {
      execute();
      return {};
    }

    return { interval: setInterval(execute, interval) };
  });

  timeoutRefs = args.update.map(({ messengerId, pusherId, interval }) => {
    const messenger = messengers[messengerId]?.update;
    const template = templates[messengerId]?.update;
    if (!messenger || !template) {
      console.error("Messenger %s not found", messengerId);
      return {};
    }

    const pusher = pushers[pusherId];
    if (!pusher) {
      console.error("Pusher %s not found", pusherId);
      return {};
    }

    const execute = async () => {
      try {
        const message = await messenger(template);
        const response = await pusher.update(message);
        const start = DateTime.now();
        await pusher.log(response, (...args) => {
          console.log(...args, `took ${start.diffNow().toHuman()}`);
        });
      } catch (err) {
        console.error(
          "Failed to push %s message via %s: %O",
          messengerId,
          pusherId,
          err,
        );
      }
    };

    if (!interval) {
      execute();
      return {};
    }

    const makeInterval = () => {
      let realInterval = interval;

      for (let i = 0; i < 10; i++) {
        realInterval = (Math.random() * 0.5 + 0.75) * interval;
        if (realInterval >= 1000) {
          return realInterval;
        }
      }

      return realInterval;
    };

    const ref: TimeoutRef = {};
    (async function executeWithRef() {
      await execute();
      if (ref.timeout) {
        clearTimeout(ref.timeout);
      }
      ref.timeout = setTimeout(executeWithRef, makeInterval());
    })();

    return ref;
  });
};

export default run;
