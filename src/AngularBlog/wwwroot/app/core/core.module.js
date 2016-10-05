(function () {
    'use strict';

    angular.module('core', [
        /*
         * Angular modules
         */
        'ngRoute', 'ngSanitize',

        // Custom modules 
        'core.logger',
        'core.router'
        // 3rd Party Modules
        
    ]);
})();