(function (angular) {
    angular.module('admin').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('admin', {
                abstract: true,
                url: '/admin',
                security: {
                    roles: ['Administrator']
                },
                templateUrl: 'app/sites/admin/layout/views/main.html'
            });
    }]);
}(angular));