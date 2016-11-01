(function (angular) {

    angular.module('app', [

        // third-party
        'ngStorage',
        'ui.router',
        'toastr',
        
        // custom
        'auth',
        'errors',
        'sign-in',
        'admin',
        'public',
        'develop']);

    angular.module('app').constant('config', config);

    angular.module('app').run([
        '$rootScope', function ($rootScope) {

            $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams, options) {

                setTimeout(function () {
                    $('[autofocus]').focus();
                    $('body').removeClass('modal-open').removeClass('scroll-lock');
                    window.scroll(0, 0);
                });

            });
        }]);

}(angular));




