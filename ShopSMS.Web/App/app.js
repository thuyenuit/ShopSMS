/// <reference path="/Assets/Admin/libs/angular/angular.js" />

(function () {
    angular.module('sms',
        ['sms.product',
        'sms.productCategory',
        'sms.category',
        'sms.common']).config(config).config(configAuthentication);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];

    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider
             .state('baseView', {             
                 templateUrl: '/app/shared/views/baseView.html',
                 abstract: true
             })
            .state('login', {
                url: '/login',
                templateUrl: '/app/component/login/loginView.html',
                controller: 'loginController'
            })
            .state('home', {    
                parent: 'baseView',
                url: '/dashboard',
                templateUrl: '/app/component/home/homeView.html',
                controller: 'homeController'
            });
        $urlRouterProvider.otherwise('/login');
        $locationProvider.html5Mode(true);
        $locationProvider.hashPrefix('!');


    }

    configAuthentication.$inject = ['$httpProvider'];
    function configAuthentication($httpProvider){
        $httpProvider.interceptors.push(function ($q, $location) {
            return {
                request: function (config) {
                    return config;
                },
                requestError: function(rejection){
                    return $q.reject(rejection);
                },
                response: function (response) {
                    if (response.status == "401")
                        $location.path('/login');

                    return response;
                },
                responseError: function (rejection) {
                    if(rejection.status == "401")
                        $location.path('/login');

                    return $q.reject(rejection);
                }
            }
        })
    }
})();