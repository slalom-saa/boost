'use strict';

var gulp = require('gulp'),
    wiredep = require('wiredep'),
    cssmin = require('gulp-cssmin'),
    concat = require('gulp-concat'),
    plugins = require('gulp-load-plugins')(),
    src = require('../config.json');


gulp.task("vendor:js", function () {
    var js = wiredep().js;
    if (js) {
        return gulp.src(wiredep().js)
            .pipe(gulp.dest(src.paths.dist.vendor));
    }
    return null;
});

gulp.task("vendor:css", function () {
    var css = wiredep().css;
    if (css) {
        return gulp.src(wiredep().css)
            .pipe(gulp.dest(src.paths.dist.vendor));
    }
    return null;
});