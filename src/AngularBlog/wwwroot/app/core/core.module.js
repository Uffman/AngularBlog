(function () {
    'use strict';

    angular.module('core', [
        /*
         * Angular modules
         */
        'ngRoute', 'ngSanitize', 'summernote',

        // Custom modules 
        'core.logger',
        'core.exception',
        'core.router',
        'app.data'
        // 3rd Party Modules
        
    ]);
})();