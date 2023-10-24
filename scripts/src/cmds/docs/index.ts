import { SUBCOMMANDS } from "../../lib/index";

import dev from "./dev";

export default {
  usage: "docs",
  description: "Run a documentation command",
  [SUBCOMMANDS]: {
    dev,
  },
};
