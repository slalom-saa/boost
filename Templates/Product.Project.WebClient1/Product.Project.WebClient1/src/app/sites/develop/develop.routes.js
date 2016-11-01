(function (angular) {
    angular.module('develop').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('develop', {
                abstract: true,
                url: '/develop',
                security: {
                    roles: ['Developer']
                },
                templateUrl: 'app/sites/develop/layout/views/main.html'
            });
    }]);
}(angular));