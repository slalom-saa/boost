(function (angular) {
    angular.module('develop.home').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
		$stateProvider
            .state('develop.home', {
                url: '/home',
            	templateUrl: 'app/sites/develop/home/views/home.html',
            	controller: ['ui', function (ui) {
	            }]
		    });
	}]);
}(angular));