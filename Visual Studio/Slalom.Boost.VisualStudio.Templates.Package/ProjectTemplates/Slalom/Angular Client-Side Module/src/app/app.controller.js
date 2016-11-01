(function (angular) {


    angular.module('app').controller('app.controller', [
           'config', '$scope', '$localStorage', '$sessionStorage', '$state', '$window', '$rootScope', '$stateParams', function (config, $scope, $localStorage, session, $state,
               $window, $rootScope, $stateParams) {


               $window.document.title = config.name;

               $scope.app = config;
               $scope.storage = $localStorage;
               $scope.user = $rootScope.user;
               $scope.$state = $state;
               $scope.sesion = session;
               $scope.$rootScope = $rootScope;
            $scope.$stateParams = $stateParams;

               if (angular.isDefined($localStorage.state)) {
                   $scope.app.state = $localStorage.state;
               } else {
                   $localStorage.state = $scope.app.state;
               }

               $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams, options) {
                   $scope.user = $rootScope.user;
               });
           }
    ]);
}(angular));