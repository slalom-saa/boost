(function (angular) {
    angular.module('public.home').config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('public.home', {
                url: '/home',
                templateUrl: 'app/sites/public/home/views/home.html',
                security: {
                    required: false
                },
                controller: function () {
                }
            });
    }]);
}(angular));