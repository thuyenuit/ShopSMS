//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController);

    productCategoryAddController.$inject = ['$http', '$scope', 'apiService',
        'commonService', '$state', 'notificationService', 'authenticationService'];

    function productCategoryAddController($http, $scope, apiService, commonService,
        $state, notificationService, authenticationService) {
        //angular.element("input").on('focus', function () {
        //    $(this).css("background-color", "#EEE9E9");
        //}).on('focusout', function () {
        //    $(this).css("background-color", "");
        //});

        angular.element("input[name='order']").on('input', function () {
            this.value = this.value.replace(/[^\d\.\-]/g, '');
        });

        // add category
        $scope.cateInfo = {};

        $scope.GetSEOTitleCategory = GetSEOTitleCategory;
        function GetSEOTitleCategory() {
            $scope.cateInfo.CategoryAlias = commonService.getSeoTitle($scope.cateInfo.CategoryName);
        }

        $scope.AddCategory = AddCategory;
        function AddCategory() {       
            var url = '/api/category/create';
            $scope.cateInfo.Status = true;
            $scope.cateInfo.CreateDate = new Date();

            $scope.promise = apiService.post(url, $scope.cateInfo, function (result) {
                LoadCategory();
                $scope.CategoryName = '';
                $scope.CategoryAlias = '';
                $scope.CategoryDescription = '';
                notificationService.displaySuccess('Thêm mới thành công.');
                $('#btnClose').click();

            }, function () {
                notificationService.displayError('Thêm mới thất bại. Vui lòng kiểm tra lại!');
            });

            $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
        }


        // add productCategory
        $scope.pcInfo = {};

        $scope.GetSEOTitle = GetSEOTitle;
        function GetSEOTitle() {
            $scope.pcInfo.ProductCategoryAlias = commonService.getSeoTitle($scope.pcInfo.ProductCategoryName);
        }

        function LoadCategory() {
            apiService.get('/api/category/getallNoPage', null, function (result) {
                $scope.categories = result.data;
            }, function () {
                notificationService.displayError('Không thể tải danh sách danh mục');
            });
            //$scope.$parent.MethodShowLoading("Đang tải dữ liệu", $scope.promise);
        }
        LoadCategory();

        $scope.AddProductCategory = AddProductCategory;
        function AddProductCategory() {

            if ($scope.pcInfo.CategoryID == null || $scope.pcInfo.CategoryID == ''
                || $scope.pcInfo.ProductCategoryName == null || $scope.pcInfo.ProductCategoryName == ''
                || $scope.pcInfo.ProductCategoryAlias == null || $scope.pcInfo.ProductCategoryAlias == '')
            {
                notificationService.displayError('Dữ liệu nhập không hợp lệ!');
            }
            else
            {
                var url = '/api/productcategory/create';
                $scope.promise = apiService.post(url, $scope.pcInfo, function (result) {
                    notificationService.displaySuccess(result.data);
                    $state.go('productCategories');
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }          
        }

    }
})(angular.module('sms.productCategory'));