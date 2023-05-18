import { Configuration } from "webpack";
import * as path from "path";
import * as glob from "glob";
import autoprefixer from "autoprefixer";
import MiniCssExtractPlugin from "mini-css-extract-plugin";
import TerserPlugin from "terser-webpack-plugin";
import CssMinimizerPlugin from "css-minimizer-webpack-plugin";
import ESLintPlugin from "eslint-webpack-plugin";
import StylelintPlugin from "stylelint-webpack-plugin";
import BrowserSyncPlugin from "browser-sync-webpack-plugin";

const development = process.env.NODE_ENV !== "production";

const configuration: Configuration = {
  entry: () => {
    const workspaces = glob.sync("../Mess.{Modules,Themes}/*/Assets", {
      posix: true,
    });

    const entries = workspaces.reduce((entries, workspace) => {
      const bundles = glob.sync(`${workspace}/src/*`, {
        posix: true,
      });

      bundles.forEach((bundle) => {
        const bundleName = path.parse(bundle).base;
        const scriptFiles = glob.sync(`${bundle}/**/*.{js,ts}`);
        const styleFiles = glob.sync(`${bundle}/**/*.{css,scss}`);
        const allFiles = scriptFiles.concat(styleFiles);

        if (allFiles.length > 0) {
          entries[
            `../${workspace}/../wwwroot/assets/${bundleName}/${bundleName}`
          ] = allFiles;
        }
      });

      return entries;
    }, {} as { [key: string]: string[] });

    return entries;
  },
  output: {
    filename: "[name].js",
    cssFilename: "[name].css",
  },
  module: {
    rules: [
      {
        test: /\.ts$/,
        exclude: /node_modules/,
        use: {
          loader: "babel-loader",
          options: {
            presets: ["@babel/preset-env", "@babel/preset-typescript"],
          },
        },
      },
      {
        test: /\.(sa|sc|c)ss$/,
        use: [
          development ? "style-loader" : MiniCssExtractPlugin.loader,
          "css-loader",
          {
            loader: "postcss-loader",
            options: {
              postcssOptions: {
                plugins: [autoprefixer()],
              },
            },
          },
          "sass-loader",
        ],
      },
    ],
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: "[name].css",
    }),
    new ESLintPlugin({
      extensions: ["js", "ts"],
    }),
    new StylelintPlugin({
      files: "../Mess.{Modules,Themes}/*/Assets/src/**/*.{css,scss}",
    }),
    new BrowserSyncPlugin({
      host: "localhost",
      port: 3001,
      proxy: "https://localhost:5001",
    }),
  ],
  optimization: {
    minimizer: [new TerserPlugin(), new CssMinimizerPlugin()],
  },
  devtool: development ? "source-map" : false,
  resolve: {
    extensions: [".ts", ".js"],
  },
};

export default configuration;
