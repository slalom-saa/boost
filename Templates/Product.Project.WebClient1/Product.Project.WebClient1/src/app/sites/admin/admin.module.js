(function(angular) {
    angular.module('admin', [
        'ngStorage',
        'ui.router',
        "ngAnimate",

        'admin.users',
        'admin.logs',
        'admin.layout',
        'admin.settings',
        'admin.dashboard'
    ]);

    angular.module('admin').run(configure);

    configure.$inject = ['config'];

    function configure(config) {

    }
}(angular));