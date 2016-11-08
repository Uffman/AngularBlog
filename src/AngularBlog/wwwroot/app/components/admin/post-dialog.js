(function () {
    'use strict';

    angular
        .module('admin')
        .controller('PostDialogController', PostDialogController);

    PostDialogController.$inject = ['$scope', 'data', 'PostFactory', 'ngDialog'];

    function PostDialogController($scope, data, PostFactory, ngDialog) {
        /* jshint validthis:true */
        $scope.post = data;
        
        activate();

        function activate() {
            //debugger;
            //var id = post.id;
            //PostFactory.getDataById(id).success(function (response) {
            //    self.post = response;
            //});
        }

        $scope.savePost = function (post) {
            if (post.id) {
                PostFactory.update(post).success(function (response) {
                    ngDialog.close();
                });
            } else {
                PostFactory.create(post).success(function (response) {
                    ngDialog.close();
                });
            }
        }
    }
})();
