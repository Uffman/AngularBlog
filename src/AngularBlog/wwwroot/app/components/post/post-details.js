(function () {
    'use strict';

    angular
        .module('post')
        .controller('PostDetailsController', PostDetailsController);

    PostDetailsController.$inject = ['$location', '$routeParams', 'PostFactory', '$sce'];

    function PostDetailsController($location, $routeParams, PostFactory, $sce) {
        /* jshint validthis:true */
        var self = this;
        self.title = 'Post Details';
        self.post = null;
        
        activate();

        function activate() {
            var id = $routeParams.id;
            PostFactory.getDataById(id).success(function (response) {
                self.post = response;
            });
        }

        self.allowHtml = function (value) {
            return $sce.trustAsHtml(value);
        };
    }
})();
