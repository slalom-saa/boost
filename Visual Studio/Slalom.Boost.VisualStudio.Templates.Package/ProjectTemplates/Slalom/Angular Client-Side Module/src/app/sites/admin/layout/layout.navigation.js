(function () {

    angular
      .module('admin.layout')
      .directive('adminNavigation', ['$window', navigation]);

    function navigation($window) {
        var directive = {
            link: link,
            restrict: 'C',
            templateUrl: 'app/sites/admin/layout/views/navigation.html'
        };
        return directive;

        function link(scope, element, attrs) {
        }
    }
}());
