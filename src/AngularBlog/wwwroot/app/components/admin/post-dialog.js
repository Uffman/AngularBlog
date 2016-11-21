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

        $scope.imageUpload = function (files) {
            var imgNode = $('<img>').attr('src', 'https://assets-cdn.github.com/images/modules/dashboard/bootcamp/octocat_setup.png')[0];
            $scope.editor.summernote('insertNode', imgNode);

            console.log('image upload:', files);
            console.log('image upload\'s editable:', $scope.editable);
        }
    }
})();
