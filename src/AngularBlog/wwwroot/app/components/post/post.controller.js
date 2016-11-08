(function () {
    'use strict';

    angular
        .module('post')
        .controller('PostController', PostController);
    
    PostController.$inject = ['$location', 'logger', 'PostFactory'];

    function PostController($location, logger, PostFactory) {
        /* jshint validthis:true */
        var self = this;
        self.title = 'Post';
        self.postsList = [];

        activate();

        function activate() {            
            PostFactory.getData().success(function (response) {            
               self.postsList = response;
               logger.info('Activated Post View');
            });
        }
    }
})();
