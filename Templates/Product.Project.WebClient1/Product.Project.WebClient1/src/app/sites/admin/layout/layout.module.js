(function (angular) {
    angular.module('admin.layout', ['ui.router', 'ui.bootstrap']);
    angular.module('admin.layout').run(configure);

    configure.$inject = ['$rootScope', 'ui', '$state'];

    function configure($rootScope, ui, $state) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams, options) {
            $rootScope.hideNavigation = getProperty(toState, 'hideNavigation', $state) === true;
            $rootScope.hideSidebar = getProperty(toState, 'hideSidebar', $state) === true;
            ui.unblock();
        });
    }

    function getProperty(state, name, $state) {
        if (state[name]) {
            return state[name];
        }
        if (state.name.indexOf('.') > -1) {
            var parent = $state.get(state.name.substring(0, state.name.lastIndexOf('.')));
            if (parent == null) {
                return null;
            }
            return getProperty(parent, name, $state);
        }
        return null;
    }
}(angular));




