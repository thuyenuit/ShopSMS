/// <reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('categoryEditController', categoryEditController);

    categoryEditController.$inject = ['$scope', 'apiService', 'commonService',
                                            '$stateParams', '$state', 'notificationService'];

    function categoryEditController($scope, apiService, commonService, $stateParams,
                                            $state, notificationService) {
        $scope.CategoryInfo = {};

        $scope.LoadCategoryInfo = LoadCategoryInfo;
        function LoadCategoryInfo() {
            var consfig = {
                params: {
                    categoryId: $stateParams.Id
                }
            };

            var url = '/api/category/getbyid';
            $scope.promise = apiService.get(url, consfig, function (result) {
                $scope.CategoryInfo = result.data;
            }, function () {
                notificationService.displayError('Không tìm thấy thông tin danh mục! Vui lòng kiểm tra lại');
            })
            $scope.$parent.MethodShowLoading("Đang tải dữ liệu", $scope.promise);
        }
        LoadCategoryInfo();

        angular.element("input[name='order']").on('input', function () {
            this.value = this.value.replace(/[^\d\.\-]/g, '');
        });

        $scope.GetSEOTitle = GetSEOTitle;
        function GetSEOTitle() {
            $scope.CategoryInfo.CategoryAlias = commonService.getSeoTitle($scope.CategoryInfo.CategoryName);
        }

        //cap nhat
        $scope.UpdateCategory = UpdateCategory;
        function UpdateCategory() {
            var url = '/api/category/update';
            $scope.promise = apiService.put(url, $scope.CategoryInfo, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('categories');
            }, function (result) {
                notificationService.displayError(result.data);             
            });
            $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
        }

    }
})(angular.module('sms.category'));
