(function () {

    angular
      .module('public.layout')
      .directive('navigation', ['$window', navigation]);

    function navigation($window) {
        var directive = {
            link: link,
            restrict: 'C',
            templateUrl: 'app/sites/public/layout/views/navigation.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }
}());
