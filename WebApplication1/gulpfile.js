"use strict";

const gulp = require("gulp"),
    minify = require("gulp-minify"),
    sourcemaps = require("gulp-sourcemaps");

gulp.task("default", ["copy", "min"]);

gulp.task("copy", ["copy:jquery"])

gulp.task("copy:jquery", () => {
    let result = gulp.src("node_modules/jquery/dist/jquery.min.*")
        .pipe(gulp.dest("wwwroot/lib/jquery"));

    return result;
});

gulp.task("min", ["min:js"]);

gulp.task("min:js", () => {
    let result = gulp.src("wwwroot/js/script.js")
        .pipe(sourcemaps.init())
        .pipe(minify({
            ext: { min: ".min.js" },
            noSource: true
        }))
        .pipe(sourcemaps.write("./", {
            mapFile: mapFilePath => mapFilePath.replace(".js.map", ".map")
        }))
        .pipe(gulp.dest("wwwroot/js"));

    return result;
});
