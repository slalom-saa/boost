(function (angular) {
    angular.module('public.locations').config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('public.locations', {
                url: '/locations',
                templateUrl: 'app/sites/public/locations/views/locations.html',
                secure: true,
                controller: function () {
                }
            });
    }]);
}(angular));