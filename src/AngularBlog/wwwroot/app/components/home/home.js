(function () {
    'use strict';

    angular
        .module('home')
        .controller('Home', Home);

    Home.$inject = ['$location', 'logger', 'PostFactory', '$sce', '$q', 'dataservice'];

    function Home($location, logger, PostFactory, $sce, $q, dataservice) {
        /* jshint validthis:true */
        var self = this;
        self.title = 'home';
        self.postsList = [];

        activate();

        function activate() {
            var promises = GetPostData(true);
            return $q.all(promises).then(function () {
            });
        }

        self.allowHtml = function (value) {
            return $sce.trustAsHtml(value);
        };

        function GetPostData(isPublished) {
            return PostFactory.getData(isPublished).then(function (data) {
                var dataList = data;                
                dataList.forEach(function (post) {
                    if(post.isPublished){
                        post.createdDate = moment(post.createdDate).format('DD.MM.YYYY HH:mm');
                        post.modifiedDate = moment(post.modifiedDate).format('DD.MM.YYYY HH:mm');
                        self.postsList.push(post);
                    }                    
                });                
                return self.postsList;
            });
        }
    }
})();
