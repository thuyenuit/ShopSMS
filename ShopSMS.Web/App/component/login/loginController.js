(function (app) {
    app.controller('loginController',
        ['$scope', 'loginService', '$injector', 'notificationService', '$http', 'authenticationService',
        function ($scope, loginService, $injector, notificationService, $http, authenticationService) {
            $scope.loginData = {
                userName: "",
                password: ""
            };
             
            $scope.loginSubmit = function () {
                var username = $scope.loginData.userName;
                var password = $scope.loginData.password;
                   /* $scope.delay = 0;
                    $scope.minDuration = 0;
                    $scope.message = "Đang đăng nhập";
                    $scope.backdrop = true;*/
                    $scope.promise = loginService.login(username, password, $scope.promise)
                    .then(function (response) {
                        if (response != null && response.data.error != 'undefined') {
                            notificationService.displayError("Tài khoản hoặc mật khẩu không hợp lệ");
                        }
                        else if (response == null) {                    
                            $scope.promise = $http.get('/api/home/getMenuPer', null, {
                                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
                            }).then(function (res) {
                                 authenticationService.setMenuInfo(res.data);
                                 var stateService = $injector.get('$state');
                                 stateService.go('home');
                            }, function () { });
                            $scope.$parent.MethodShowLoading("Đang đăng nhập", $scope.promise);                         
                        }
                    });

                    $scope.$parent.MethodShowLoading("Đang đăng nhập", $scope.promise);
            }
        }]);
})(angular.module('sms'));
