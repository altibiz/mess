import yargs from "yargs/yargs";
import { hideBin } from "yargs/helpers";

const args = yargs(hideBin(process.argv))
  .usage("Usage: yarn workspace @mess/publisher start [options]")
  .option("baseUri", {
    describe: "The base URI at which to publish measurements",
    type: "string",
    default: "http://localhost:5000/push",
    demandOption: true,
    nargs: 1,
  })
  .help("h")
  .alias("h", "help")
  .parseSync();

export type Args = typeof args;

export default args;
