import { Configuration } from "@rspack/cli";
import * as rspack from "@rspack/core";
import autoprefixer from "autoprefixer";
import * as fs from "fs";
import * as glob from "glob";
import * as path from "path";

// TODO: eslint, stylelint, presetEnv

const configurations: Configuration[] = glob
  .sync("../Mess.Assets/*", {
    posix: true,
  })
  .map((workspace) => {
    const project = workspace.split("/")[2];
    const projectPath = fs.existsSync(`../Mess.Modules/${project}`)
      ? `../Mess.Modules/${project}`
      : fs.existsSync(`../Mess.Themes/${project}`)
        ? `../Mess.Themes/${project}`
        : (() => {
            throw new Error(`Project with name ${project} does not exist`);
          })();

    const entries = (() => {
      const entries: Configuration["entry"] = {};
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
    })();

    const configuration: Configuration = {
      name: project,
      entry: entries,
      output: {
        filename: "[name].js",
        cssFilename: "[name].css",
        path: path.resolve(projectPath, "wwwroot/assets"),
        publicPath: `/${project}/assets/`,
      },
      target: ["web", "es5"],
      experiments: {
        css: true,
        lazyCompilation: true,
      },
      module: {
        rules: [
          {
            test: /\.(t|j)s$/,
            exclude: [/[\\/]node_modules[\\/]/],
            loader: "builtin:swc-loader",
            options: {
              sourceMap: true,
              jsc: {
                parser: {
                  syntax: "typescript",
                },
                externalHelpers: true,
              },
              env: {
                targets: "Chrome >= 48",
              },
            },
          },
          {
            test: /\.(sa|sc|c)ss$/,
            type: "css",
            use: [
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
            test: /\.(png|jpe?g|gif|svg|ico|woff2?)$/,
            type: "asset/resource",
            generator: {
              filename: "resources/[name].[contenthash][ext]",
            },
          },
        ],
      },
      plugins: [
        new rspack.SwcCssMinimizerRspackPlugin({}),
        new rspack.SwcJsMinimizerRspackPlugin({}),
        new rspack.ProgressPlugin({}),
        new rspack.HotModuleReplacementPlugin(),
        new rspack.CopyRspackPlugin({
          patterns: [
            {
              from: `${workspace}/src/**/*.{png,jpg,jpeg,gif,svg,ico,woff,woff2}`,
              to: "resources/[name][ext]",
              noErrorOnMissing: true,
            },
          ],
        }),
      ],
      devtool: process.env.NODE_ENV === "production" ? false : "source-map",
      resolve: {
        extensions: [".ts", ".js"],
      },
      context: path.resolve(__dirname),
      stats: "errors-warnings",
    };

    return configuration;
  });

export default configurations;
