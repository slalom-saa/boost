(function (angular) {
    angular.module('public.account').config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('public.account', {
                url: '/account',
                templateUrl: 'app/sites/public/account/views/account.html',
                secure: true,
                controller: function () {
                }
            });
    }]);
}(angular));