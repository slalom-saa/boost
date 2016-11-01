'use strict';

var gulp = require('gulp'),
    browserSync = require('browser-sync').get('default'),
    project = require('../config.json');


var src = project.paths.src,
    dist = project.paths.dist;

gulp.task('watch', ['watch:app', 'watch:index', 'watch:sass', 'watch:templates', 'watch:config']);


gulp.task('watch:templates', function () {
    gulp.watch(src.templates, ['build:templates']);
});

gulp.task('watch:app', function () {
    gulp.watch(src.js.concat([src.app]).concat(src.modules), ['build:js']);
});

gulp.task('watch:config', function () {
    gulp.watch(src.config, ['build:config']);
});

gulp.task('watch:index', function () {
    gulp.watch(src.index, ['build:index']);
});

gulp.task('watch:sass', function () {
    gulp.watch(src.sass.all.concat(src.sass.app), ['build:sass']);
});