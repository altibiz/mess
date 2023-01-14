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

const workspaceGlob = "../Mess.{Modules,Themes}/*/Assets";
const styleGlob = "src/**/*.{css,scss}";
const bundleGlob = "src/**/*.{js,ts}";
const isDev = process.env.NODE_ENV === "development";
const outputDirs = process.env.MESS_CLIENT_OUTPUT_DIRS
  ? process.env.MESS_CLIENT_OUTPUT_DIRS.split(",")
  : [];
console.log(outputDirs);

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

gulp.task("lintBundle", () =>
  gulp
    .src(bundleGlob)
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
gulp.task("lint", gulp.parallel("lintBundle", "lintStyles"));

gulp.task("bundle", () =>
  browserify({
    basedir: ".",
    debug: isDev,
    entries: glob.sync(bundleGlob),
    cache: {},
    packageCache: {},
  })
    .plugin(tsify)
    .bundle()
    .on("error", fancyLog)
    .pipe(source("bundle.js"))
    .pipe(buffer())
    .pipe(gulpIf(isDev, sourcemaps.init()))
    .pipe(terser())
    .pipe(gulpIf(isDev, sourcemaps.write("./")))
    .pipeToOutputDirs()
    .pipe(gulpIf(browserSync.active, browserSync.stream())),
);
gulp.task("watchBundle", () =>
  gulp.watch(bundleGlob, gulp.series("lintBundle", "bundle")),
);

// NOTE: fix sourcemaps
gulp.task("styles", () =>
  merge(
    gulp.src("src/**/*.css").pipe(gulpIf(isDev, sourcemaps.init())),
    gulp.src("src/**/*.scss").pipe(sass().on("error", fancyLog)),
  )
    .pipe(concat("bundle.css"))
    .pipe(postcss([autoperfix, minify]))
    .pipe(gulpIf(isDev, sourcemaps.write("./")))
    .pipeToOutputDirs()
    .pipe(gulpIf(browserSync.active, browserSync.stream())),
);
gulp.task("watchStyles", () =>
  gulp.watch(styleGlob, gulp.series("lintStyles", "styles")),
);

gulp.task("build", gulp.series("lint", gulp.parallel("bundle", "styles")));
gulp.task(
  "watch",
  gulp.series(
    "lint",
    gulp.parallel("bundle", "styles"),
    gulp.parallel("browserSync", "watchBundle", "watchStyles"),
  ),
);

function buildAssetGroups() {
  const workspaces = glob.sync(workspaceGlob);
  for (const workspace of workspaces) {
    const bundles = glob.sync(`${workspace}/*`);
  }
}
