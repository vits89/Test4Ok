"use strict";

const gulp = require("gulp"),
    minify = require("gulp-minify"),
    sourcemaps = require("gulp-sourcemaps");

gulp.task("default", ["min"]);

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
