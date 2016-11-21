(function () {
    'use strict';

    angular
        .module('app')
        .controller('shell', shell);

    shell.$inject = ['$timeout',  'logger']; 

    function shell($timeout,  logger) {
        /* jshint validthis:true */
        var self = this;
        
        //self.title = config.appTitle;
        self.isBusy = true;
        self.showSplash = true;

        activate();

        function activate() {
            //logger.success(config.appTitle + ' loaded!', null);
            logger.success('Shell loaded!', null);
            hideSplash();
        }

        function hideSplash() {
            $timeout(function () {
                self.showSplash = false;
            }, 1000);
        }
    }
})();
