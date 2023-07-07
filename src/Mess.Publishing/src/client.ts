import { importMessengers } from "./messengers";
import { importPushers } from "./pushers";
import { importTemplates } from "./templates";
import args from "./args";

type Interval = NodeJS.Timer;
type Timeout = NodeJS.Timer;

type IntervalRef = { interval?: Interval };
type TimeoutRef = { timeout?: Timeout };

const run = async () => {
  const templates = await importTemplates();
  const messengers = await importMessengers();
  const pushers = await importPushers();
  const activePushers = Object.entries(pushers).filter(
    ([pusherId]) =>
      args.push.map(({ pusherId }) => pusherId).includes(pusherId) ||
      args.status.map(({ pusherId }) => pusherId).includes(pusherId),
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
      const message = await messenger(template);
      const messages = Array.isArray(message) ? message : [message];
      for (const message of messages) {
        try {
          await pusher.push(messengerId, message);
          console.log("Sent %s message via %s", messengerId, pusherId);
        } catch (err) {
          console.error(
            "Failed to push %s message via %s: %O",
            messengerId,
            pusherId,
            err,
          );
        }
      }
    };

    if (!interval) {
      execute();
      return {};
    }

    return { interval: setInterval(execute, interval) };
  });

  timeoutRefs = args.status.map(
    ({ messengerId, pusherId, approximateInterval }) => {
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
        const message = await messenger(template);
        const messages = Array.isArray(message) ? message : [message];
        for (const message of messages) {
          try {
            await pusher.update(messengerId, message);
            console.log("Sent %s update via %s", messengerId, pusherId);
          } catch (err) {
            console.error(
              "Failed to update %s update via %s: %O",
              messengerId,
              pusherId,
              err,
            );
          }
        }
      };

      if (!approximateInterval) {
        execute();
        return {};
      }

      const makeInterval = () => {
        for (let i = 0; i < 10; i++) {
          const interval = (Math.random() * 0.5 + 0.75) * approximateInterval;
          if (interval >= 1000) {
            return interval;
          }
        }

        return approximateInterval;
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
    },
  );
};

export default run;
