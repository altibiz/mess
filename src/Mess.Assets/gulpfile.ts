/* eslint-disable @typescript-eslint/no-explicit-any */

import { dest, src, task, parallel, watch, series } from "gulp";
import { create } from "browser-sync";

import eslint, { format, failAfterError } from "gulp-eslint";
import stylelint from "gulp-stylelint";
import postcss from "gulp-postcss";
import minify from "cssnano";
import autoprefixer from "autoprefixer";

import browserify from "browserify";
import terser from "gulp-terser";
import { init, write } from "gulp-sourcemaps";
import buffer from "vinyl-buffer";
import source from "vinyl-source-stream";
import tsify from "tsify";
import gulpSass from "gulp-sass";
import dartSass from "sass";

import { sync } from "glob";
import fancyLog from "fancy-log";
import concat from "gulp-concat";
import merge from "merge2";
import gulpIf from "gulp-if";

import { parse } from "path";

import { workspaces } from "./package.json";

const autoperfix = autoprefixer();
const sass = gulpSass(dartSass);
const browserSync = create();

const isDev = process.env.NODE_ENV === "Development";

const scriptFileExtensions = "js,ts";
const scriptGlob = `../Mess.{Modules,Themes}/*/Assets/src/**/*.{${scriptFileExtensions}}`;
const styleFileExtensions = "css,scss";
const styleGlob = `../Mess.{Modules,Themes}/*/Assets/src/**/*.{${styleFileExtensions}}`;

const bundles = () =>
  workspaces.flatMap((workspaceGlob) =>
    sync(workspaceGlob).flatMap((workspace) =>
      sync(`${workspace}/src/*`).map((bundle) => ({
        input: bundle,
        output: `${parse(workspace).dir}/wwwroot/assets/${parse(bundle).base}`,
        name: parse(bundle).base,
      })),
    ),
  );

const buildScriptBundlePipeline = (bundle: any) => {
  const buildPipeline = ({ minified }: any) => {
    let pipeline = browserify({
      basedir: ".",
      debug: isDev,
      entries: sync(`${bundle.input}/**/*.{${scriptFileExtensions}}`),
      cache: {},
      packageCache: {},
    })
      .plugin(tsify)
      .bundle()
      .on("error", fancyLog)
      .pipe(source(minified ? `${bundle.name}.min.js` : `${bundle.name}.js`))
      .pipe(buffer())
      .pipe(gulpIf(isDev, init()));

    if (minified) {
      pipeline = pipeline.pipe(terser());
    }

    pipeline
      .pipe(gulpIf(isDev, write("./")))
      .pipe(dest(bundle.output))
      .pipe(gulpIf(browserSync.active, browserSync.stream()));
  };

  buildPipeline({ minified: true });
  buildPipeline({ minified: false });
};

const buildStyleBundlePipeline = (bundle: any) => {
  const buildPipeline = ({ minified }: any) => {
    let pipeline = merge(
      src(`${bundle.input}/*.css`).pipe(gulpIf(isDev, init())),
      src(`${bundle.input}/*.scss`).pipe(sass().on("error", fancyLog)),
    ).pipe(concat(minified ? `${bundle.name}.min.css` : `${bundle.name}.css`));
    if (minified) {
      pipeline = pipeline.pipe(postcss([autoperfix, minify]));
    }
    pipeline
      .pipe(gulpIf(isDev, write("./")))
      .pipe(dest(bundle.output))
      .pipe(gulpIf(browserSync.active, browserSync.stream()));
  };

  buildPipeline({ minified: true });
  buildPipeline({ minified: false });
};

const buildScriptLintPipeline = (bundle: any) => {
  src(`${bundle.input}/*.{${scriptFileExtensions}}`)
    .pipe(eslint())
    .pipe(format())
    .pipe(failAfterError());
};

const buildStyleLintPipeline = (bundle: any) => {
  src(`${bundle.input}/*.{${styleFileExtensions}}`).pipe(
    stylelint({
      failAfterError: true,
      reporters: [{ formatter: "string", console: true }],
    }),
  );
};

task("browserSync", () =>
  browserSync.init({
    port: 3001,
    proxy: "https://localhost:5001",
    open: false,
    logLevel: "silent",
  }),
);

task("lintScripts", () =>
  src(scriptGlob).pipe(eslint()).pipe(format()).pipe(failAfterError()),
);
task("lintStyles", () =>
  src(styleGlob).pipe(
    stylelint({
      failAfterError: true,
      reporters: [{ formatter: "string", console: true }],
    }),
  ),
);
task("lint", parallel("lintScripts", "lintStyles"));

task("bundleScripts", (done: any) => {
  bundles().forEach(buildScriptBundlePipeline);
  done();
});
task("watchScripts", (done: any) => {
  bundles().forEach((bundle) =>
    watch(
      `${bundle.input}/*.{${scriptFileExtensions}}`,
      series(
        (done: any) => {
          buildScriptLintPipeline(bundle);

          done();
        },
        (done: any) => {
          buildScriptBundlePipeline(bundle);

          done();
        },
      ),
    ),
  );

  done();
});

// FIX: sourcemaps
task("bundleStyles", (done: any) => {
  bundles().forEach(buildStyleBundlePipeline);
  done();
});
task("watchStyles", (done: any) => {
  bundles().forEach((bundle) =>
    watch(
      `${bundle.input}/*.{${styleFileExtensions}}`,
      series(
        (done: any) => {
          buildStyleLintPipeline(bundle);

          done();
        },
        (done: any) => {
          buildStyleBundlePipeline(bundle);

          done();
        },
      ),
    ),
  );

  done();
});

task("build", series("lint", parallel("bundleScripts", "bundleStyles")));
task(
  "watch",
  series("build", parallel("browserSync", "watchScripts", "watchStyles")),
);
