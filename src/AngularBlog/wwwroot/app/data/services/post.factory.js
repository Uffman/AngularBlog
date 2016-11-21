(function () {
    'use strict';

    angular
        .module('core')
        .factory('PostFactory', PostFactory);

    PostFactory.$inject = ['$http', '$location', '$q', 'exception', 'logger'];

    function PostFactory($http,  $location, $q, exception, logger) {
        var apiUrl = 'api/post';
        var isPrimed = false;
        var primePromise;

        var service = {
            getData: getData,
            getDataById: getDataById,
            create: create,
            update: update,
            remove: remove,
            ready: ready
        };

        return service;

        /* Implementation */

        function getData(isPublished) {                      
            return $http(
                {
                    method: 'GET',
                    url: apiUrl                    
                })
                .then(getDataCompleted)
                .catch(function (error) {
                    exception.catcher('XHR Failed for getAvengers')(error);
                    $location.url('/');
                });

            function getDataCompleted(data) {               
                return data.data;
            }
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
                    url: apiUrl + '/' + post.id,
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

        function prime() {
            // This function can only be called once.            
            if (primePromise) {
                return primePromise;
            }

            primePromise = $q.when(true).then(success);
            return primePromise;

            function success() {
                isPrimed = true;
                logger.info('Primed data');
            }
        }

        function ready(nextPromises) {           
            var readyPromise = primePromise || prime();          
            return readyPromise
                .then(function() { return $q.all(nextPromises); })
                .catch(exception.catcher('"ready" function failed'));
        }
    }
})();