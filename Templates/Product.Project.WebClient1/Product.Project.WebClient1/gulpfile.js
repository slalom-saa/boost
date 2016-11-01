﻿/**
*  Welcome to your gulpfile!
*  The gulp tasks are splitted in several files in the gulp directory
*  because putting all here was really too long
*/

'use strict';

var gulp = require('gulp'),
    wrench = require('wrench'),
    browserSync = require('browser-sync').create('default');

/**
 *  This will load all js or coffee files in the gulp directory
 *  in order to load all gulp tasks
 */

wrench.readdirSyncRecursive('./gulp').filter(function (file) {
    return (/\.(js|coffee)$/i).test(file);
}).map(function (file) {
    require('./gulp/' + file);
});


/**
 *  Default task clean temporaries directories and launch the
 *  main optimization build task
 */
gulp.task('default', ['build', 'serve'], function () {

});