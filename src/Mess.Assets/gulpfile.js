const gulp = require("gulp");
const browserSync = require("browser-sync").create();

const eslint = require("gulp-eslint");
const stylelint = require("gulp-stylelint");
const postcss = require("gulp-postcss");
const minify = require("cssnano");
const autoperfix = require("autoprefixer")();

const browserify = require("browserify");
const terser = require("gulp-terser");
const sourcemaps = require("gulp-sourcemaps");
const buffer = require("vinyl-buffer");
const source = require("vinyl-source-stream");
const tsify = require("tsify");

const sass = require("gulp-sass")(require("sass"));

const glob = require("glob");
const fancyLog = require("fancy-log");
const concat = require("gulp-concat");
const merge = require("merge2");
const gulpIf = require("gulp-if");

const package = require("./package.json");
const path = require("path");

const bundles = () =>
  package.workspaces.flatMap(
    workspaceGlob => glob
      .sync(workspaceGlob)
      .flatMap(workspace => 
        glob.sync(`${workspace}/src/*`).map(bundle => ({
            input: bundle,
            output: `${path.parse(workspace).dir}/wwwroot/assets/${path.parse(bundle).base}`,
            name: path.parse(bundle).base,
        })),
      ));

const styleGlob = "../Mess.{Modules,Themes}/*/Assets/src/**/*.{css,scss}";
const scriptGlob = "../Mess.{Modules,Themes}/*/Assets/src/**/*.{js,ts}";

const isDev = process.env.NODE_ENV === "Development";

Object.defineProperty(Object.prototype, "pipeToOutputDirs", {
  value: function () {
    let pipeline = this;
    outputDirs.forEach(
      (outputDir) => (pipeline = pipeline.pipe(gulp.dest(outputDir))),
    );
    return pipeline;
  },
});

gulp.task("browserSync", () =>
  browserSync.init({
    port: 3001,
    proxy: "https://localhost:5001",
    open: false,
    logLevel: "silent",
  }),
);

gulp.task("lintScripts", () =>
  gulp
    .src(scriptGlob)
    .pipe(eslint())
    .pipe(eslint.format())
    .pipe(eslint.failAfterError()),
);
gulp.task("lintStyles", () =>
  gulp.src(styleGlob).pipe(
    stylelint({
      failAfterError: true,
      reporters: [{ formatter: "string", console: true }],
    }),
  ),
);
gulp.task("lint", gulp.parallel("lintScripts", "lintStyles"));

gulp.task("bundleScripts", () =>
  bundles().forEach(bundle => {
    const buildPipeline = ({ minified }) => {
      let pipeline = browserify({
        basedir: ".",
        debug: isDev,
        entries: glob.sync(`${bundle.input}/**/*.{js,ts}`),
        cache: {},
        packageCache: {},
      })
        .plugin(tsify)
        .bundle()
        .on("error", fancyLog)
        .pipe(source(minified ? `${bundle.name}.min.js` : `${bundle.name}.js`))
        .pipe(buffer())
        .pipe(gulpIf(isDev, sourcemaps.init()))

      if (minified) {
        pipeline = pipeline.pipe(terser());
      }

      pipeline
        .pipe(gulpIf(isDev, sourcemaps.write("./")))
        .pipe(gulp.dest(bundle.output))
        .pipe(gulpIf(browserSync.active, browserSync.stream()))
    }

    buildPipeline({ minified: true });
    buildPipeline({ minified: false });
  })
);
gulp.task("watchScripts", () =>
  gulp.watch(bundleGlob, gulp.series("lintScripts", "bundleScripts")),
);

// FIX: sourcemaps
gulp.task("bundleStyles", () =>
  bundles().forEach(bundle => {
    const buildPipeline = ({minified}) => {
      let pipeline = merge(
        gulp.src(`${bundle.input}/*.css`).pipe(gulpIf(isDev, sourcemaps.init())),
        gulp.src(`${bundle.input}/*.scss`).pipe(sass().on("error", fancyLog)),
      )
        .pipe(concat(minified ? `${bundle.name}.min.css` : `${bundle.name}.css`));
      if (minified) {
        pipeline = pipeline
        .pipe(postcss([autoperfix, minify]))
      }
      pipeline
      .pipe(gulpIf(isDev, sourcemaps.write("./")))
      .pipe(gulp.dest(bundle.output))
      .pipe(gulpIf(browserSync.active, browserSync.stream()));

    }

    buildPipeline({minified: true});
    buildPipeline({minified: false});
  }));
gulp.task("watchStyles", () =>
  gulp.watch(styleGlob, gulp.series("lintStyles", "bundleStyles")),
);

gulp.task(
  "build",
  gulp.series("lint", gulp.parallel("bundleScripts", "bundleStyles"))
);
gulp.task(
  "watch",
  gulp.series(
    "build",
    gulp.parallel("browserSync", "watchScripts", "watchStyles"),
  ),
);
