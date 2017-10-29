 //<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('categoryAddController', categoryAddController);

    categoryAddController.$inject = ['$http', '$scope', 'apiService',
        'commonService', '$state', 'notificationService', 'authenticationService'];

    function categoryAddController($http, $scope, apiService, commonService,
        $state, notificationService, authenticationService) {

        angular.element("input[name='order']").on('input', function () {
            this.value = this.value.replace(/[^\d\.\-]/g, '');
        });

        $scope.CategoryInfo = {};
        $scope.GetSEOTitle = GetSEOTitle;
        function GetSEOTitle() {
            $scope.CategoryInfo.CategoryAlias = commonService.getSeoTitle($scope.CategoryInfo.CategoryName);
        }

        $scope.AddCategory = AddCategory;
        function AddCategory() {

            if ($scope.CategoryInfo.CategoryName === null || $scope.CategoryInfo.CategoryName === ''
                || $scope.CategoryInfo.CategoryAlias === null || $scope.CategoryInfo.CategoryAlias === '') {
                notificationService.displayError('Dữ liệu nhập không hợp lệ!');
            }
            else {
                var url = '/api/category/create';
                $scope.promise = apiService.post(url, $scope.CategoryInfo, function (result) {
                    notificationService.displaySuccess(result.data);
                    $state.go('categories');
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        }
    }
})(angular.module('sms.category'));
