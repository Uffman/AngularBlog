(function () {
    'use strict';

    angular
        .module('home')
        .controller('Home', Home);

    Home.$inject = ['$location', 'logger', 'PostFactory'];

    function Home($location, logger, PostFactory) {
        /* jshint validthis:true */
        var self = this;
        self.title = 'home';
        self.postsList = [];

        activate();

        function activate() {
            PostFactory.getData().success(function (response) {
                self.postsList = response;
                self.postsList.forEach(function (post) {
                    post.createdDate = moment(post.createdDate).format('DD.MM.YYYY HH:mm');
                    post.modifiedDate = moment(post.modifiedDate).format('DD.MM.YYYY HH:mm');
                });
                logger.info('Activated Home View');
            });
        }
    }
})();
