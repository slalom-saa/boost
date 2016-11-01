var gulp = require('gulp'),
    browserSync = require('browser-sync').get('default'),
    templateCache = require('gulp-angular-templatecache'),
    es = require('event-stream'),
    angularFilesort = require('gulp-angular-filesort'),
    sass = require('gulp-sass'),
    buffer = require('vinyl-buffer'),
    cssmin = require('gulp-cssmin'),
    concat = require('gulp-concat'),
    sourcemaps = require('gulp-sourcemaps'),
    wiredep = require('wiredep'),
    plugins = require('gulp-load-plugins')(),
    plumber = require('gulp-plumber'),
    uglify = require('gulp-uglify'),
    min = require('gulp-htmlmin'),
    spritesmith = require('gulp.spritesmith'),
    imagemin = require('gulp-imagemin'),
    project = require('../config.json');
    

var src = project.paths.src,
    dist = project.paths.dist;

gulp.task('build', ['build:assets', 'build:js', 'build:templates', 'build:sass', 'build:index', 'build:config']);

gulp.task('build:config', function () {
    gulp.src(src.config)
        .pipe(gulp.dest(dist.root))
        .pipe(browserSync.reload({ stream: true }));
});

gulp.task('build:assets', function () {

    gulp.src([src.root + '*.*'])
        .pipe(gulp.dest(dist.root));

    gulp.src(src.fonts)
        .pipe(gulp.dest(dist.fonts));

    gulp.src(src.images)
       .pipe(gulp.dest(dist.images));

    var spriteData =
        gulp.src('./src/images/icons/*.*') // source path of the sprite images
            .pipe(spritesmith({
                imgName: 'icons.png',
                imgPath : '/images/icons.png',
                cssName: 'icons.css'
            }));

    var imgStream = spriteData.img
    // DEV: We must buffer our stream into a Buffer for `imagemin`
    .pipe(buffer())
    .pipe(imagemin())
    .pipe(gulp.dest('./dist/images/'));

    var cssStream = spriteData.css
    .pipe(cssmin())
    .pipe(gulp.dest('./dist/css/'));

    gulp.start('build:index');
});

gulp.task('build:templates', function () {
    gulp.src(src.templates)
        .pipe(plumber())
        .pipe(min({ collapseWhitespace: true }))
        .pipe(plumber())
        .pipe(templateCache({
            root: 'app/',
            module: 'app'
        }))
        .pipe(concat('templates.js'))
        .pipe(uglify())
        .pipe(gulp.dest(dist.js))
        .pipe(browserSync.reload({ stream: true }));
});

gulp.task('build:js', function () {
    es.merge(gulp.src(src.js), gulp.src(src.app), gulp.src(src.modules)
        .pipe(plumber())
        .pipe(angularFilesort()))
        .pipe(sourcemaps.init())
        .pipe(concat('main.js'))
        .pipe(uglify())
        .pipe(sourcemaps.write())
        .pipe(gulp.dest(dist.js))
        .pipe(browserSync.reload({ stream: true }));
});

gulp.task('build:sass', function () {
    gulp.src(src.sass.main)
       .pipe(plumber())
       .pipe(sass())
       .pipe(concat('main.css'))
       .pipe(cssmin())
       .pipe(gulp.dest(dist.css))
       .pipe(browserSync.reload({ stream: true }));

    gulp.src(src.sass.app)
       .pipe(plumber())
       .pipe(sass())
       .pipe(concat('app.css'))
       .pipe(cssmin())
       .pipe(gulp.dest(dist.css))
       .pipe(browserSync.reload({ stream: true }));
});

gulp.task('build:index', ['vendor:css', 'vendor:js'], function () {

    return gulp.src(src.index)
      .pipe(wiredep.stream({
          fileTypes: {
              html: {
                  replace: {
                      js: function (filePath) {
                          return '<script src="vendor/' + filePath.split('/').pop() + '"></script>';
                      },
                      css: function (filePath) {
                          return '<link rel="stylesheet" href="vendor/' + filePath.split('/').pop() + '"/>';
                      }
                  }
              }
          }
      }))
      .pipe(plugins.inject(
        gulp.src(dist.js + '*', { read: false }), {
            addRootSlash: false,
            transform: function (filePath, file, i, length) {
                return '<script src="' + filePath.replace(dist.root.substring(2), '') + '"></script>';
            }
        }))

      .pipe(plugins.inject(
        gulp.src([dist.css + 'main.css', dist.css + '*'], { read: false }), {
            addRootSlash: false,
            transform: function (filePath, file, i, length) {
                return '<link rel="stylesheet" href="' + filePath.replace(dist.root.substring(2), '') + '"/>';
            }
        }))

      .pipe(gulp.dest(dist.root))
      .pipe(browserSync.reload({ stream: true }));
});
