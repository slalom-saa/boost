(function () {

    angular
      .module('admin.layout')
      .directive('adminFooter', ['$window', footer]);

    function footer($window) {
        var directive = {
            link: link,
            restrict: 'C',
            templateUrl: 'app/sites/admin/layout/views/footer.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }
}());