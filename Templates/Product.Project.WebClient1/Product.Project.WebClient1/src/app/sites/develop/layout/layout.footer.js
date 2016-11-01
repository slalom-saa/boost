(function () {

    angular
      .module('develop.layout')
      .directive('developFooter', ['$window', footer]);

    function footer($window) {
        var directive = {
            link: link,
            restrict: 'C',
            templateUrl: 'app/sites/develop/layout/views/footer.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }
}());