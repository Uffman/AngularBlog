(function () {
    'use strict';

    angular.module('core', [
        /*
         * Angular modules
         */
        'ngRoute', 'ngSanitize', 'summernote',

        // Custom modules 
        'core.logger',
        'core.router'
        // 3rd Party Modules
        
    ]);
})();