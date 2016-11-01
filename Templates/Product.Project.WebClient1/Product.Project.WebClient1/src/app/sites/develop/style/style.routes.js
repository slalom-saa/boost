(function (angular) {
    angular.module('develop.style').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
		$stateProvider
            .state('develop.style', {
                url: '/style',
            	templateUrl: 'app/sites/develop/style/views/style.html',
            	controller: ['ui', function (ui) {
	            }]
		    });
	}]);
}(angular));