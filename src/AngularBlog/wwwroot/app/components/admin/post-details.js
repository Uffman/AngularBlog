(function () {
    'use strict';

    angular
        .module('post')
        .controller('AdminPostDetailsController', AdminPostDetailsController);

    AdminPostDetailsController.$inject = ['$scope', '$location', '$routeParams', 'PostFactory', 'logger'];

    function AdminPostDetailsController($scope, $location, $routeParams, PostFactory, logger) {
        /* jshint validthis:true */
        var self = this;
        self.title = 'Post Details';
        self.post = null;
        
        activate();
        function activate() {
            var id = $routeParams.id;
            if (id !== '' && id !== 'new') {
                PostFactory.getDataById(id).success(function (response) {
                    self.post = response;
                });
            }
        }

        self.savePost = function (post) {           
            if (post.id && post.id !== '') {
                PostFactory.update(post).success(function (response) {
                    logger.success('Post saved!');
                    $location.path('/admin');
                });
            } else {
                PostFactory.create(post).success(function (response) {
                    logger.success('Post created!');
                    $location.path('/admin');
                });
            }
        }

        self.cancel = function () {
            $location.path('/admin');
        };
    }
})();
