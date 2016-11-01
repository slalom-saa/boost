(function (angular) {
    angular.module('develop.icons').config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
		$stateProvider
            .state('develop.icons', {
                url: '/icons',
            	templateUrl: 'app/sites/develop/icons/views/icons.html',
            	controller: 'icons.controller as vm'
		    });
	}]);
}(angular));