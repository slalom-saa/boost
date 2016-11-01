(function (angular) {
    angular.module('sign-in').config([
        '$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('sign-in', {
                    url: '/sign-in',
                    templateUrl: 'app/sign-in/views/sign-in.html',
                    security: {
                        required: false
                    },
                    hideNavigation: true,
                    controller: 'sign-in.controller as vm'
                }).state('register', {
                    url: '/register',
                    templateUrl: 'app/sign-in/views/register.html',
                    security: {
                        required: false
                    },
                    hideNavigation: true,
                    controller: 'register.controller as vm'
                });
        }
    ])
}(angular));