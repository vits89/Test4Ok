const { src, dest } = require("gulp"),
    minify = require("gulp-minify");

exports.default = function() {
    return src("wwwroot/js/script.js", { sourcemaps: true })
        .pipe(minify({
            ext: { min: ".min.js" },
            noSource: true
        }))
        .pipe(dest("wwwroot/js", { sourcemaps: "." }));
};
