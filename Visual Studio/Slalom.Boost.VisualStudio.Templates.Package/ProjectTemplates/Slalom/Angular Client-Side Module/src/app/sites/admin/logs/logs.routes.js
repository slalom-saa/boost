(function (angular) {
    angular.module('admin.logs').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('admin.logs', {
                url: '/logs',
                templateUrl: 'app/sites/admin/logs/views/logs.html',
                controller: 'admin.logs.controller as vm'
            })
            .state('admin.logs.session', {
                url: '/session/:id',
                templateUrl: 'app/sites/admin/logs/views/session.html',
                controller: function () {
                }
            })
            .state('admin.logs.user', {
                url: '/user/:userName',
                templateUrl: 'app/sites/admin/logs/views/user.html',
                controller: function () {
                }
            })
            .state('admin.logs.command', {
                url: '/command/:commandName',
                templateUrl: 'app/sites/admin/logs/views/command.html',
                controller: function () {
                }
            })
            .state('admin.logs.success', {
                url: '/success/:success',
                templateUrl: 'app/sites/admin/logs/views/success.html',
                controller: function () {
                }
            });
    }]);
}(angular));