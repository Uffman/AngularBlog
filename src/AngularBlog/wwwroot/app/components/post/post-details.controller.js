(function () {
    'use strict';

    angular
        .module('post')
        .controller('PostDetailsController', PostDetailsController);

    PostDetailsController.$inject = ['$location', '$routeParams', 'PostFactory'];

    function PostDetailsController($location, $routeParams, PostFactory) {
        /* jshint validthis:true */
        var self = this;
        self.title = 'Post Details';
        self.post = null;
        
        activate();

        function activate() {
            debugger
            var id = $routeParams.id;
            PostFactory.getDataById(id).success(function (response) {
                self.post = response;
            });
        }
    }
})();
