/// <reference path="/Assets/Admin/libs/angular/angular.js" />

(function () {
    angular.module('sms.other', ['sms.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('otherView', {
                parent: 'baseView',
                url: '/other',
                templateUrl: '/app/component/other/otherView.html',
                controller: 'otherController'
            });
    }
})();