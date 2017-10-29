/// <reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productCategoryEditController', productCategoryEditController);

    productCategoryEditController.$inject = ['$scope', 'apiService', 'commonService',
                                            '$stateParams', '$state', 'notificationService'];

    function productCategoryEditController($scope, apiService, commonService, $stateParams, 
                                            $state, notificationService) {
        $scope.pcEditInfo = {};
         // thong tin the loai
        $scope.LoadCategoryInfo = LoadCategoryInfo;
        function LoadCategoryInfo() {
            var consfig = {
                params: {
                    categoryId: $stateParams.categoryId
                }
            };

            var url = '/api/productcategory/getbyid';
            $scope.promise = apiService.get(url, consfig, function (result) {
                $scope.pcEditInfo = result.data;
            }, function () {
                notificationService.displayError('Không tìm thấy thông tin thể loại! Vui lòng kiểm tra lại');
            })
            $scope.$parent.MethodShowLoading("Đang tải dữ liệu", $scope.promise);
        }
        LoadCategoryInfo();

        angular.element("input[name='order']").on('input', function () {
            this.value = this.value.replace(/[^\d\.\-]/g, '');
        });

        $scope.GetSEOTitle = GetSEOTitle;
        function GetSEOTitle() {
            $scope.pcEditInfo.ProductCategoryAlias = commonService.getSeoTitle($scope.pcEditInfo.ProductCategoryName);
        }

        $scope.categories = {};
         function LoadCategory() {
            apiService.get('/api/category/getallNoPage', null, function (result) {
                $scope.categories = result.data;             
            }, function () {
                notificationService.displayError('Không thể tải danh sách danh mục');
            });
        }
        LoadCategory();

        //cap nhat
        $scope.UpdateProductCategory = UpdateProductCategory;
        function UpdateProductCategory() {            
            var url = '/api/productcategory/update';
            $scope.promise = apiService.put(url, $scope.pcEditInfo, function (result) {
                notificationService.displaySuccess(result.data);
                $state.go('productCategories');
            }, function (result) {
                notificationService.displayError(result.data);
                //notificationService.displayError('Cập nhật thất bại. Vui lòng kiểm tra lại!');
            });
            $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
        }

    }
})(angular.module('sms.productCategory'));