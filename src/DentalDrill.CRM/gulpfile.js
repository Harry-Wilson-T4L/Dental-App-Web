"use strict";

// ReSharper disable PossiblyUnassignedProperty
// ReSharper disable UndeclaredGlobalVariableUsing
// ReSharper disable Es6Feature
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

(() => {
    const gulp = require("gulp");
    const sass = require("gulp-sass")(require("sass"));
    const concat = require("gulp-concat");
    const cssmin = require("gulp-cssmin");
    const uglify = require("gulp-uglify");
    const merge = require("merge-stream");
    const del = require("del");
    const through = require("through2");
    const log = require("fancy-log");
    const colors = require("ansi-colors");
    const path = require("path");

    const config = require("./bundles.json");

    function compileSass(done) {
        if (!(config.sass && Array.isArray(config.sass) && config.sass.length > 0)) {
            done();
            return undefined;
        }

        const tasks = config.sass.map(f => gulp
            .src(f)
            .pipe(verbose(chunk => `Compiling SASS: '${colors.green(chunk.path)}'`))
            .pipe(sass())
            .pipe(gulp.dest(f => f.base)));
        return merge(tasks);
    }

    function copyFiles(done) {
        if (!(config.copy && Array.isArray(config.copy) && config.copy.length > 0)) {
            done();
            return undefined;
        }

        const tasks = config.copy.map(f => gulp
            .src(f.src)
            .pipe(verbose(chunk => `Copying file '${colors.green(path.relative(chunk.cwd, chunk.path))}' to '${colors.green(f.dest)}'`))
            .pipe(gulp.dest(f.dest)));

        return merge(tasks);
    }

    function bundleScripts(done) {
        if (!(config.scripts && Array.isArray(config.scripts) && config.scripts.length > 0)) {
            done();
            return undefined;
        }

        const tasks = config.scripts.map(bundle => {
            let builder = gulp.src(bundle.input, { base: "." }).pipe(concat(bundle.output));
            builder = builder.pipe(verbose(chunk => `Bundling '${colors.green(bundle.output)}'`));

            if (bundle.options && bundle.options.uglify) {
                builder = builder.pipe(uglify(bundle.options.uglify.options ? bundle.options.uglify.options : undefined));
            }

            builder = builder.pipe(gulp.dest("."));
            return builder;
        });

        return merge(tasks);
    }

    function bundleStyles(done) {
        if (!(config.styles && Array.isArray(config.styles) && config.styles.length > 0)) {
            done();
            return undefined;
        }

        const tasks = config.styles.map(bundle => {
            let builder = gulp.src(bundle.input, { base: "." }).pipe(concat(bundle.output));
            builder = builder.pipe(verbose(chunk => `Bundling '${colors.green(bundle.output)}'`));

            if (bundle.options && bundle.options.minify) {
                builder = builder.pipe(cssmin());
            }

            builder = builder.pipe(gulp.dest("."));
            return builder;
        });

        return merge(tasks);
    }

    bundleStyles.displayName = "bundle:css:process";

    function watch() {
        config.scripts.forEach(bundle => {
            gulp.watch(bundle.input, gulp.series("bundle:js"));
        });

        config.styles.forEach(bundle => {
            gulp.watch(bundle.input, gulp.series("bundle:css"));
        });

        config.sass.forEach(pattern => {
            gulp.watch(pattern, gulp.series("sass"));
        });

        config.sass_watch.forEach(pattern => {
            gulp.watch(pattern, gulp.series("sass"));
        });
    }

    function watchSass() {
        config.sass.forEach(pattern => {
            gulp.watch(pattern, gulp.series("sass"));
        });

        config.sass_watch.forEach(pattern => {
            gulp.watch(pattern, gulp.series("sass"));
        });
    }

    gulp.task("sass", compileSass);
    gulp.task("copy", copyFiles);
    gulp.task("bundle:js", bundleScripts);
    gulp.task("bundle:css", gulp.series("sass", bundleStyles));
    gulp.task("bundle", gulp.series("bundle:js", "bundle:css"));

    gulp.task("build", gulp.series("bundle", "copy"));
    gulp.task("watch", watch);
    gulp.task("watch:sass", watchSass);

    function verbose(format) {
        if (format === undefined) {
            format = chunk => `Processing '${colors.green(chunk.path)}'`;
        }

        return through.obj((chunk, encoding, callback) => {
            log.info(format(chunk));
            callback(null, chunk);
        });
    }
})();