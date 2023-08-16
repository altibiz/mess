import { SUBCOMMANDS } from "../../lib/index";

import module from "./module";
import theme from "./theme";

export default {
  usage: "plop",
  description: "Plop a new template",
  [SUBCOMMANDS]: {
    module,
    theme,
  },
};
