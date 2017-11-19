///<reference path="/Assets/Admin/libs/angular/angular.js" />


(function (app) {
    app.factory('apiService', apiService);

    apiService.$inject = ['$http', 'authenticationService', 'notificationService'];

    function apiService($http, authenticationService, notificationService) {
        return {
            get: get,
            post: post,
            put: put,
            del: del
        };

        function get(url, params, success, failed) {
            authenticationService.setHeader();
            return $http.get(url, params)
                .then(function (result) {
                    success(result);
                }, function (error) {
                    if (error.status === 401) {
                        notificationService.displayError('Authenticate is required.');
                        authenticationService.removeToken();
                    }
                    else if (failed !== null) {
                        failed(error);
                    }
                });
        }

        function post(url, data, success, failed) {
            authenticationService.setHeader();
            return $http.post(url, data)
               .then(function (result) {
                   success(result);
               }, function (error) {
                   if (error.status === 401) {
                       notificationService.displayError('Authenticate is required.');
                       authenticationService.removeToken();
                   }
                   else if (failed !== null) {
                       failed(error);
                   }
               });
        }

        function put(url, data, success, failed) {
            authenticationService.setHeader();
            return $http.put(url, data).then(function (result) {
                success(result);
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                    authenticationService.removeToken();
                }
                else if (failed !== null) {
                    failed(error);
                }

            });
        }

        function del(url, data, success, failed) {
            authenticationService.setHeader();
            $http.delete(url, data).then(function (result) {
                success(result);
            }, function (error) {
                if (error.status === 401) {
                    notificationService.displayError('Authenticate is required.');
                    authenticationService.removeToken();
                }
                else if (failed !== null) {
                    failed(error);
                }

            });
        }
    }

})(angular.module('sms.common'));