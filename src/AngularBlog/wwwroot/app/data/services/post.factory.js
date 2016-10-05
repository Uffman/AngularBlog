(function () {
    'use strict';

    angular
        .module('app.data')
        .factory('PostFactory', PostFactory);

    PostFactory.$inject = ['$http'];

    function PostFactory($http) {
        var apiUrl = 'api/post';

        var service = {
            getData: getData,
            getDataById: getDataById,
            create: create,
            update: update,
            remove: remove
        };

        return service;

        /* Implementation */

        function getData() {
            return $http(
            {
                method: 'Get',
                url: apiUrl
            });
        }

        function getDataById(id) {
            return $http(
            {
                method: 'GET',
                url: apiUrl + '/' + id
            });
        }

        function create(post) {
            return $http({
                method: 'POST',
                url: apiUrl,
                data: post
            });
        }

        function update(post) {
            return $http(
                {
                    method: 'PUT',
                    url: apiUrl + '/' + post.Id,
                    data: post
                });
        }

        function remove(id) {
            return $http(
                {
                    method: 'DELETE',
                    url: apiUrl + '/' + id,
                    data: id
                });
        }
    }
})();