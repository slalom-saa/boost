(function () {

    angular
      .module('develop.layout')
      .directive('developNavigation', ['$window', navigation]);

    function navigation($window) {
        var directive = {
            link: link,
            restrict: 'C',
            templateUrl: 'app/sites/develop/layout/views/navigation.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }
}());
