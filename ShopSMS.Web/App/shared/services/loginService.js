(function (app) {
    app.service('loginService', ['$http', '$q', 'authenticationService', 'authData',
        function ($http, $q, authenticationService, authData) {

            var userInfo;
            var deferred;

            this.login = function (userName, password) {
                deferred = $q.defer();
                var data = "grant_type=password&username=" + userName + "&password=" + password;
                $http.post('/oauth/token', data, {
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                }).then(function (response) {

                    userInfo = {
                        accessToken: response.data.access_token,
                        userName: userName,
                        userCode: response.data.userCode,
                        fullName: response.data.fullName

                    };
                    authenticationService.setTokenInfo(userInfo);
                    authData.authenticationData.IsAuthenticated = true;
                    authData.authenticationData.userName = userName;
                    deferred.resolve(null);
                   
                }, function (error, status) {
                    authData.authenticationData.IsAuthenticated = false;
                    authData.authenticationData.userName = "";
                    authData.authenticationData.userCode = "";
                    authData.authenticationData.fullName = "";
                    deferred.resolve(error);
                });
                return deferred.promise;
            }

            this.logOut = function () {
                authenticationService.removeToken();              
                authData.authenticationData.accessToken = "";
                authData.authenticationData.IsAuthenticated = false;
                authData.authenticationData.userName = "";
                authData.authenticationData.userCode = "";
                authData.authenticationData.fullName = "";
                authData.authenticationData.listMenuInfo = [];
            }
    }]);
})(angular.module('sms.common'))