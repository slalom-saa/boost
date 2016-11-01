(function (angular) {

    angular.module('sign-in')
        .controller('sign-in.controller', ['auth.service', '$location', 'ui', controller]);

    function controller(service, $location, ui) {
        var vm = this;
        vm.signIn = signIn;
        vm.data = {
            userName: '',
            password: ''
        };


        activate();


        function activate() {
            service.signOut();
        }

        function signIn() {
            ui.block();
            service.signIn(vm.data).then(function () {
                if (vm.redirect) {
                    if ($state.get(vm.redirect)) {
                        $state.go(vm.redirect);
                    } else {
                        $location.path(vm.redirect);
                    }
                } else {
                    window.location = '/';
                }
            }).catch(function () {
                ui.unblock();
            });
        }
    }
}(angular));