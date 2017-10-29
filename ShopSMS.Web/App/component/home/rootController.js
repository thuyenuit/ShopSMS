
(function (app) {
    app.controller('rootController',
        ['$scope', '$state', 'showLoadingService', 'authData', 'authenticationService',
            'loginService', 'apiService', '$http',
            function ($scope, $state, showLoadingService, authData,
                authenticationService, loginService, apiService, $http) {

                $scope.authentication = authData.authenticationData;

                $scope.MethodShowLoading = function (msg, myPromise) {
                    showLoadingService.showLoading(msg, myPromise, $scope);
                }

                $scope.resData = {};
                function LoadResources() {
                    $http.get('/api/other/getListResources', null)
                    .then(function (result) {
                        $scope.resData = result.data;
                    }, function (result) {
                        notificationService.displayError(result.data);
                    });
                }
                LoadResources();

                $scope.logOut = function () {
                    loginService.logOut();
                    $state.go('login');
                }
            }]);
})(angular.module('sms'));
