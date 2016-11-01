var gulp = require('gulp'),
    rimraf = require('gulp-rimraf'),
    src = require('../config.json');

gulp.task('clean', ['clean:root']);

gulp.task('clean:root', function () {
    return gulp.src(src.paths.dist.root + '/*', { read: false })
        .pipe(rimraf());
});
