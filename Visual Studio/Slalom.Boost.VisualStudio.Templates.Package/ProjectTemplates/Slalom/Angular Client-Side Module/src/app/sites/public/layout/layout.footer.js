(function () {

    angular
      .module('public.layout')
      .directive('footer', ['$window', footer]);

    function footer($window) {
        var directive = {
            link: link,
            restrict: 'C',
            templateUrl: 'app/sites/public/layout/views/footer.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }
}());