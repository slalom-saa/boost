(function () {

    angular
      .module('admin.layout')
      .directive('adminSidebar', ['$window', sidebar]);

    function sidebar($window) {
        var directive = {
            link: link,
            restrict: 'C',
            templateUrl: 'app/sites/admin/layout/views/sidebar.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }
}());