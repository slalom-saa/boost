(function (angular) {
    angular.module('public').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('public', {
                abstract: true,
                templateUrl: 'app/sites/public/layout/views/main.html',
                hideSidebar: true
            });
    }]);
}(angular));