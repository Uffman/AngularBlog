(function () {
    'use strict';

    var core = angular.module('core');

    core.config(toastrConfig);
    toastrConfig.$inject = ['toastr'];

    function toastrConfig(toastr) {
        toastr.options.timeOut = 4000;
        toastr.options.positionClass = 'toast-bottom-right';
    }

    var settings = {
        appTitle: 'Angular Blog',
        appErrorPrefix: '[AB-Error]'
    }

    core.value('config', settings);

    core.config(configure);
    configure.$inject = ["$logProvider", "$routeProvider", "routehelperConfigProvider"];

    function configure($logProvider, $routeProvider, routehelperConfigProvider, exceptionHandlerProvider) {
        // turn debugging off/on (no info or warn)
        if ($logProvider.debugEnabled) {
            $logProvider.debugEnabled(true);
        }

        // Configure the common route provider
        routehelperConfigProvider.config.$routeProvider = $routeProvider;
        routehelperConfigProvider.config.docTitle = 'NG-Modular: ';
        var resolveAlways = { /* @ngInject */
            //ready: function () {
            //    return true;
            //}
            ready: function (PostFactory) {               
                return PostFactory.ready();
            }
            // ready: ['dataservice', function (dataservice) {
            //    return dataservice.ready();
            // }]
        };
        routehelperConfigProvider.config.resolveAlways = resolveAlways;

        // Configure the common exception handler
       // exceptionHandlerProvider.configure(settings.appErrorPrefix);
    }
})();