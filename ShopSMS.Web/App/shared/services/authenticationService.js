(function (app) {
    'use strict'
    app.service('authenticationService', ['$http', '$q', '$window', 'localStorageService', 'authData',
        function ($http, $q, $window, localStorageService, authData) {

            var menuInfo;
            this.setMenuInfo = function (data) {
                menuInfo = data;
                localStorageService.set("menuInfo", menuInfo);
                this.initMenu();
            }

            this.initMenu = function () {
                var menuInfo2 = localStorageService.get("menuInfo");
                if (menuInfo2) {
                    authData.authenticationData.listMenuInfo = menuInfo2;
                }
            }

            var tokenInfo;
            this.setTokenInfo = function (data) {
                tokenInfo = data;
               // $window.sessionStorage["TokenInfo"] = JSON.stringify(tokenInfo);
                localStorageService.set("TokenInfo", JSON.stringify(tokenInfo));
                this.init();
            }

            this.getTokenInfo = function () {
                return tokenInfo;
            }

            this.removeToken = function () {
                tokenInfo = null;
                // $window.sessionStorage["TokenInfo"] = null;
                localStorageService.set("TokenInfo", null);
            }

            this.init = function () {
               // if ($window.sessionStorage["TokenInfo"])
                //     tokenInfo = JSON.parse($window.sessionStorage["TokenInfo"]);
                var tokenInfo = localStorageService.get("TokenInfo");
                if (tokenInfo)
                {
                    tokenInfo = JSON.parse(tokenInfo);
                    authData.authenticationData.IsAuthenticated = true;
                    authData.authenticationData.userName = tokenInfo.userName;
                    authData.authenticationData.userCode = tokenInfo.userCode;
                    authData.authenticationData.fullName = tokenInfo.fullName;
                    authData.authenticationData.accessToken = tokenInfo.accessToken;                   
                }
            }

            this.setHeader = function () {
                delete $http.defaults.headers.common['X-Requested-With'];
                if (authData.authenticationData != undefined
                    && authData.authenticationData.accessToken != undefined
                    && authData.authenticationData.accessToken != null
                    && authData.authenticationData.accessToken != "")
                {
                    $http.defaults.headers.common['Authorization'] = 'Bearer ' + authData.authenticationData.accessToken;
                    $http.defaults.headers.common['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
                }
            }

            this.validateRequest = function () {
                var url = 'api/home/TestMethod';
                var deferred = $q.defer();
                $http.get(url).then(function () {
                    deferred.resolve(null);
                }, function (error) {
                    deferred.reject(error);
                });
                return deferred.promise;
            }

            this.init();

    }]);
})(angular.module('sms.common'));