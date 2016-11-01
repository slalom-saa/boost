(function (angular) {
    angular.module('auth', ['ngStorage']);
    angular.module('auth').run(configure);

    configure.$inject = ['$rootScope', 'auth.service', '$state'];

    function getSecurity($state, toState) {
        if (toState) {
            if (toState.security) {
                return toState.security;
            }
            var name = toState.name;
            if (name.indexOf('.') > -1) {
                name = name.substring(0, name.lastIndexOf('.'));
                var state = $state.get(name);
                return state.security || getSecurity(state);
            }
        }
        return null;
    }

    function intersectSafe(a, b) {
        var ai = 0, bi = 0;
        var result = [];

        while (ai < a.length && bi < b.length) {
            if (a[ai] < b[bi]) { ai++; }
            else if (a[ai] > b[bi]) { bi++; }
            else 
            {
                result.push(a[ai]);
                ai++;
                bi++;
            }
        }

        return result;
    }

    function containsRole(user, roles) {
        return intersectSafe(user.roles, roles).length > 0;
    };

    function configure($rootScope, service, $state) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams, options) {

            var authenticated = $rootScope.user !== undefined && $rootScope.user.authenticated === true && new Date($rootScope.user.expires) > new Date();
            var security = getSecurity($state, toState);
            var required = !security || security.required === true || security.roles;
            if (!authenticated && required) {
                event.preventDefault();
                window.location = '/#/sign-in';
            } else {
                if (security && security.roles && !containsRole($rootScope.user, security.roles)) {
                    event.preventDefault();
                    window.location = '/#/unauthorized';
                }
            }
        });
    }
}(angular));
