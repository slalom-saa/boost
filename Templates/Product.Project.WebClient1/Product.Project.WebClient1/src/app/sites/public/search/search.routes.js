(function (angular) {
    angular.module('public.search').config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('public.search', {
                url: '/search',
                templateUrl: 'app/sites/public/search/views/search.html',
                secure: true,
                controller: function () {
                }
            });
    }]);
}(angular));