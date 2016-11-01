(function (angular) {
    angular.module('develop.icons').controller('icons.controller', ['icon-names', '$scope', controller]);

    function controller(names, $scope) {
        var vm = this;

        current = names.icon54;

        vm.searchText = '';
        vm.total = current.length;
        vm.increment =  vm.current = 500;
        vm.filtered = current;
        vm.names = current;

        vm.names = current.slice(0, vm.current);

        $scope.$watch('vm.searchText',
            function (a, b) {
                vm.filtered = current.filter(function (item) {
                    return item.toLowerCase().indexOf(a.toLowerCase()) > -1;
                });
                vm.current = vm.increment;
                vm.names = vm.filtered.slice(0, vm.current);
            });

        vm.search = function (name) {
            return name.indexOf(vm.searchText) > -1;
        };


        vm.copyToClipboard = function(name) {
            window.prompt("Copy to clipboard: Ctrl+C, Enter", name);
        };

        vm.onScrollLimitReached = function () {
            vm.current = vm.current + vm.increment;
            vm.names = vm.filtered.slice(0, vm.current);
        };
        return vm;
    };

}(angular));