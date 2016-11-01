(function(angular) {
    angular.module('public', [
        'ngStorage',
        'ui.router',

        'public.home',
        'public.layout',
        'public.account',
        'public.locations',
        'public.messages',
        'public.plans',
        'public.search'
    ]);

    angular.module('public').run(configure);

    configure.$inject = ['config'];

    function configure(config) {

    }
}(angular));