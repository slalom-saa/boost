(function (angular) {
    angular.module('admin.users').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('admin.users', {
                url: '/users',
                templateUrl: 'app/sites/admin/users/views/users.html',
                controller: function () {
                }
            });
    }]);
}(angular));