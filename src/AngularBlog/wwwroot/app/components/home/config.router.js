(function () {
    'use strict';

    angular.module('home')
        .run(appRun);

    appRun.$inject = ['routehelper'];

    function appRun(routehelper) {
        routehelper.configureRoutes(getRoutes());
    }

    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/components/home/home.template.html',
                    controller: 'Home',
                    //controllerAs: 'home',
                    title: 'home'
                }
            }
        ];
    }
})();