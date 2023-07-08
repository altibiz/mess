import { Configuration, EntryObject } from "webpack";
import * as path from "path";
import * as glob from "glob";
import autoprefixer from "autoprefixer";
import MiniCssExtractPlugin from "mini-css-extract-plugin";
import TerserPlugin from "terser-webpack-plugin";
import CssMinimizerPlugin from "css-minimizer-webpack-plugin";
import ESLintPlugin from "eslint-webpack-plugin";
import StylelintPlugin from "stylelint-webpack-plugin";

const configurations: Configuration[] = glob
  .sync("../Mess.{Modules,Themes}/*/Assets", {
    posix: true,
  })
  .map((workspace) => {
    const project = workspace.split("/")[2];

    const configuration: Configuration = {
      entry: () => {
        const entries: EntryObject = {};
        const bundles = glob.sync(`${workspace}/src/*`, {
          posix: true,
        });

        bundles.forEach((bundle) => {
          const bundleName = path.parse(bundle).name;
          const scriptFiles = glob
            .sync(`${bundle}/**/*.{js,ts}`)
            .map((file) => path.resolve(file));
          const styleFiles = glob
            .sync(`${bundle}/**/*.{css,scss}`)
            .map((file) => path.resolve(file));

          if (scriptFiles.length > 0) {
            entries[`scripts/${bundleName}`] = {
              import: scriptFiles,
              publicPath: `~/${project}/assets/scripts/${bundleName}.js`,
            };
          }

          if (styleFiles.length > 0) {
            entries[`styles/${bundleName}`] = {
              import: styleFiles,
              publicPath: `~/${project}/assets/styles/${bundleName}.css`,
            };
          }
        });

        return entries;
      },
      output: {
        filename: "[name].js",
        cssFilename: "[name].css",
        path: path.resolve(workspace, "../wwwroot/assets/"),
        publicPath: `~/${project}/assets/`,
      },
      module: {
        rules: [
          {
            test: /\.(js|ts)$/,
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
              MiniCssExtractPlugin.loader,
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
          {
            test: /\.(png|jpe?g|gif|svg|woff2?|fnt|webp)$/,
            type: "asset/resource",
            generator: {
              filename: "resources/[name][ext]",
            },
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
          files: `${workspace}/src/**/*.{css,scss}`,
        }),
      ],
      optimization: {
        minimizer: [new TerserPlugin(), new CssMinimizerPlugin()],
      },
      devtool: process.env.NODE_ENV === "production" ? false : "source-map",
      resolve: {
        extensions: [".ts", ".js"],
      },
      context: path.resolve(__dirname),
      performance: {
        hints: false,
        maxEntrypointSize: 512000,
        maxAssetSize: 512000,
      },
    };

    return configuration;
  });

export default configurations;
