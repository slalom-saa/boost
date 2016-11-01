(function(angular) {
    angular.module('develop', [
        'ngStorage',
        'ui.router',
        "ngAnimate",

        'develop.layout',
        'develop.home',
        'develop.icons',
        'develop.style'
    ]);

    angular.module('develop').run(configure);

    configure.$inject = ['config'];

    function configure(config) {

    }
}(angular));