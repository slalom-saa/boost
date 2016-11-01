(function (angular) {
    angular.module('admin.settings').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('admin.settings', {
                url: '/settings',
                templateUrl: 'app/sites/admin/settings/views/settings.html',
                controller: function () {
                }
            });
    }]);
}(angular));