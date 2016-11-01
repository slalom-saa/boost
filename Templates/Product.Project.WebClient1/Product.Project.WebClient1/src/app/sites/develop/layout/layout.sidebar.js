(function () {

    angular
      .module('develop.layout')
      .directive('developSidebar', ['$window', sidebar]);

    function sidebar($window) {
        var directive = {
            link: link,
            restrict: 'C',
            templateUrl: 'app/sites/develop/layout/views/sidebar.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }
}());