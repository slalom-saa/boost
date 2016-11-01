'use strict';

var gulp = require('gulp'),
    browserSync = require('browser-sync').get('default'),
    src = require('../config.json');

gulp.task('serve', ['serve:start']);

gulp.task('serve:start', function () {

    var server = {
        baseDir: src.paths.dist.root
    };

    browserSync.instance = browserSync.init({
        startPath: src.paths.start,
        server: server
    });

    gulp.start('watch');
});

