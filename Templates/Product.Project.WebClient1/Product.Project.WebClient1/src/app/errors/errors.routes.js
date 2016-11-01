(function (angular) {
    angular.module('errors').config(['$stateProvider', function ($stateProvider) {

        $stateProvider.state('not-found', {
            url: '/not-found',
            templateUrl: 'app/errors/views/not-found.html',
            security : {
                required: false
            }
        });

        $stateProvider.state('unauthorized', {
            url: '/unauthorized',
            templateUrl: 'app/errors/views/unauthorized.html',
            security: {
                required: false
            }
        });
       
    }]);

}(angular));

