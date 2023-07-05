import path from "path";
import { Configuration } from "webpack";

const configuration: Configuration = {
  target: "node",
  mode: "production",
  entry: "./src/index.ts",
  output: {
    filename: "index.js",
    path: path.resolve(__dirname, "dist"),
  },
  resolve: {
    extensions: [".ts", ".js"],
  },
  module: {
    rules: [
      {
        test: /\.ts$/,
        use: "ts-loader",
        exclude: /node_modules/,
      },
      {
        test: /\.hbs$/,
        use: "handlebars-loader",
        exclude: /node_modules/,
      },
    ],
  },
};

export default configuration;
