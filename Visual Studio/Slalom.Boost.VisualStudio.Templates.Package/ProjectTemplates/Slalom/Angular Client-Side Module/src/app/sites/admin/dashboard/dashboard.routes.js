(function (angular) {
    angular.module('admin.dashboard').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
		$stateProvider
            .state('admin.dashboard', {
                url: '/dashboard',
            	templateUrl: 'app/sites/admin/dashboard/views/dashboard.html',
            	controller: ['ui', function (ui) {
	            }]
		    });
	}]);
}(angular));