import { SUBCOMMANDS } from "../../lib/index";

import assets from "./assets";
import module from "./module";
import tests from "./tests";
import theme from "./theme";

export default {
  usage: "plop",
  description: "Plop a new template",
  [SUBCOMMANDS]: {
    module,
    theme,
    assets,
    tests,
  },
};
