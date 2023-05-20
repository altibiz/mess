import { Configuration, EntryObject } from "webpack";
import * as path from "path";
import * as glob from "glob";
import autoprefixer from "autoprefixer";
import MiniCssExtractPlugin from "mini-css-extract-plugin";
import TerserPlugin from "terser-webpack-plugin";
import CssMinimizerPlugin from "css-minimizer-webpack-plugin";
import ESLintPlugin from "eslint-webpack-plugin";
import StylelintPlugin from "stylelint-webpack-plugin";

const configuration: Configuration = {
  entry: () => {
    const workspaces = glob.sync("../Mess.{Modules,Themes}/*/Assets", {
      posix: true,
    });

    const entries = workspaces.reduce((entries, workspace) => {
      const project = workspace.split("/")[2];
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
          entries[`../${workspace}/../wwwroot/assets/scripts/${bundleName}`] = {
            import: scriptFiles,
            publicPath: `/${project}/assets/scripts/${bundleName}.js`,
          };
        }

        if (styleFiles.length > 0) {
          entries[`../${workspace}/../wwwroot/assets/styles/${bundleName}`] = {
            import: styleFiles,
            publicPath: `/${project}/assets/styles/${bundleName}.css`,
          };
        }
      });

      return entries;
    }, {} as EntryObject);

    return entries;
  },
  output: {
    filename: "[name].js",
    cssFilename: "[name].css",
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
  ],
  optimization: {
    minimizer: [new TerserPlugin(), new CssMinimizerPlugin()],
  },
  devtool: "source-map",
  resolve: {
    extensions: [".ts", ".js"],
  },
};

export default configuration;
