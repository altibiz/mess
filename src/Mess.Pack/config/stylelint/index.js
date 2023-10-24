/* eslint-disable */

module.exports = require("js-yaml").load(
  require("fs").readFileSync(
    require("path").join(__dirname, ".stylelintrc.yml"),
    "utf8",
  ),
);
