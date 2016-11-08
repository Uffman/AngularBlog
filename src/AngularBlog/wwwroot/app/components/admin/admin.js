(function () {
    'use strict';

    angular
        .module('admin')
        .controller('Admin', AdminController);

    AdminController.$inject = ['$scope', '$location', 'logger', 'PostFactory', 'ngDialog', '$controller', 'moment'];

    function AdminController($scope,  $location, logger, PostFactory, ngDialog, $controller, moment) {
        /* jshint validthis:true */
        var self = this;
        self.title = 'Admin';
        self.postsList = [];

        activate();

        function activate() {
            PostFactory.getData().success(function (response) {
                self.postsList = response;
                self.postsList.forEach(function (post) {
                    post.createdDate = moment(post.createdDate).format('DD.MM.YYYY HH:mm');
                    post.modifiedDate = moment(post.modifiedDate).format('DD.MM.YYYY HH:mm');
                });
                logger.info('Activated Admin View');
            });
        }
       
        self.openEditDialog = function (post) {
            ngDialog.open(
                {
                    scope: $scope,
                    template: 'app/components/admin/post-dialog.html',
                    controller: $controller('PostDialogController', {
                        $scope: $scope,
                        data: post
                    })
                });
        };
    }
})();
