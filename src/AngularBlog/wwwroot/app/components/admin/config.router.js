(function () {
    'use strict';

    angular.module('admin')
        .run(appRun);

    appRun.$inject = ['routehelper'];

    function appRun(routehelper) {
        routehelper.configureRoutes(getRoutes());
    }

    function getRoutes() {
        return [
            {
                url: '/admin',
                config: {
                    templateUrl: 'app/components/admin/admin.html',
                    controller: 'Admin',
                    //controllerAs: 'home',
                    title: 'Admin'
                }
            }
        ];
    }
})();