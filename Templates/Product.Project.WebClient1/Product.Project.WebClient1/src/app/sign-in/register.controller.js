(function (angular) {

    angular.module('sign-in')
        .controller('register.controller', ['auth.service', '$location', 'ui', '$state', controller]);

    function controller(service, $location, ui, $state) {
        var vm = this;
        vm.register = register;
        vm.data = {
            userName: '',
            password: ''
        };

        activate();


        function activate() {
            service.signOut();
        }

        function register() {
            //ui.block();
            service.register(vm.data).then(function () {
                $state.go('public.account');
            }).catch(function () {
            });
        }
    }
}(angular));