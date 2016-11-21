(function () {
    'use strict';

    angular
        .module('admin')
        .controller('Admin', AdminController);

    AdminController.$inject = ['$scope', '$location', 'logger', 'PostFactory', 'ngDialog', '$controller', 'moment', '$q'];

    function AdminController($scope, $location, logger, PostFactory, ngDialog, $controller, moment, $q) {
        /* jshint validthis:true */
        var self = this;
        self.title = 'Admin';
        self.postsList = [];

        activate();

        function activate() {
            var promises = GetPostData();
            return $q.all(promises).then(function () {
                //logger.info('Activated Admin View');
            });
        }

        function GetPostData() {
            return PostFactory.getData().then(function (data) {
                self.postsList = data;
                self.postsList.forEach(function (post) {
                    post.createdDate = moment(post.createdDate).format('DD.MM.YYYY HH:mm');
                    post.modifiedDate = moment(post.modifiedDate).format('DD.MM.YYYY HH:mm');
                });
            });
        }

        self.openEditDialog = function (post) {
            ngDialog.open({
                scope: $scope,
                template: 'app/components/admin/post-dialog.html',
                controller: $controller('PostDialogController', {
                    $scope: $scope,
                    data: post
                })
            });
        };

        self.deletePost = function (post) {            
            PostFactory.remove(post.id).then(function () {  
                logger.success('Post deleted!');
                return GetPostData();
            });
        };
    }
})();