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
            },
            {
                url: '/admin/post/:id',
                config: {
                    templateUrl: 'app/components/admin/post-details.html',
                    controller: 'AdminPostDetailsController',
                    //controllerAs: 'posts',
                    title: 'Post Details'
                }
            }
        ];
    }
})();