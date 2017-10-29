(function (app) {
    app.factory('showLoadingService', showLoadingService);

    showLoadingService.$inject = ['$http'];

    function showLoadingService($http) {

        function showLoading(msg, myPromise, scope) {
            scope.delay = 0;
            scope.minDuration = 0;
            scope.message = msg;
            scope.backdrop = true;       
            scope.promise = myPromise;
        }    

        return {
            showLoading: showLoading
        }
    }
})(angular.module('sms.common'));