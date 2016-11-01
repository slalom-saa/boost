(function (angular) {
    angular.module('public.messages').config(['$stateProvider', function ($stateProvider) {
        $stateProvider
            .state('public.messages', {
                url: '/messages',
                templateUrl: 'app/sites/public/messages/views/messages.html',
                secure: true,
                controller: function () {
                }
            });
    }]);
}(angular));