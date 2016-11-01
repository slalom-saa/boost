(function (angular) {
    angular.module('public.plans').config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('public.plans', {
                url: '/plans',
                templateUrl: 'app/sites/public/plans/views/plans.html',
                secure: true,
                controller: function () {
                }
            });
    }]);
}(angular));