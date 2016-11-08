(function () {
    'use strict';

    angular.module('post')
        .run(appRun);

    appRun.$inject = ['routehelper'];

    function appRun(routehelper) {
        routehelper.configureRoutes(getRoutes());
    }

    function getRoutes() {
        return [
            {
                url: '/posts',
                config: {
                    templateUrl: 'app/components/post/posts.html',
                    controller: 'PostController',
                    //controllerAs: 'post',
                    title: 'Post'
                }
            },
            {
                url: '/post/:id',
                config: {
                    templateUrl: 'app/components/post/post-details.html',
                    controller: 'PostDetailsController',
                    //controllerAs: 'posts',
                    title: 'Post Details'
                }
            }
        ];
    }
})();