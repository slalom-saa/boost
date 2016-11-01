(function (angular) {
    angular.module('app').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.when('', '/home');
        $urlRouterProvider.when('/', '/home');
        $urlRouterProvider.otherwise('/not-found');
    }]);
}(angular));