import { cmd, ptask } from "../../lib/index";

export default cmd({
  usage: "dev",
  description: "Start the documentation development environment",
})(async () => {
  await ptask({
    name: "docs",
    command: "bun run docs watch",
  });
});
