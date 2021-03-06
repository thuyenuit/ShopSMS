﻿//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productListController', productListController);

    productListController.$inject = ['$scope', '$http', '$sce',
        'apiService', '$interval', '$filter', '$ngBootbox', '$state', 'notificationService', 'authenticationService'];

    function productListController($scope, $http, $sce, apiService, $interval,
        $filter, $ngBootbox, $state, notificationService, authenticationService) {

        $scope.onClickAddProduct = function () {
            $state.go('productAdd');
        };

        $scope.onClickImportProduct = function () {
            $state.go('productImport');
        };

        // show
        $scope.options = [
            { name: 10, value: 10 },
            { name: 25, value: 25 },
            { name: 50, value: 50 },
            { name: 100, value: 100 }];
        $scope.valueShow = $scope.options[0].value;
        $scope.changeShow = function () {
            ListProduct();
        };

        $scope.onChangeStatus = function () {
            ListProduct();
        };

        $scope.categoryID = 0;
        $scope.onChangePCategory = function () {
            ListProduct();
        };

        $scope.keyWord = '';
        $scope.onSearch = function () {
            ListProduct();
        };

        $scope.listStatus = [];
       // $scope.listStatus.StatusID = 0;
        function LoadStatus() {
            apiService.get('/api/other/getListStatus', null, function (result) {
                $scope.listStatus = result.data;
               // $scope.listStatus.StatusID = 0;
            }, function (result) {
                notificationService.displayError('Không thể tải danh sách trạng thái');
            });
        }
        LoadStatus();
      
        $scope.listPCategories = {};
        $scope.listCategories = {};
        function LoadCategory() {
            apiService.get('/api/category/getallNoPage', null, function (result) {
                $scope.listCategories = result.data;
            }, function () {
                notificationService.displayError('Không thể tải danh sách danh mục');
            });
        }
        LoadCategory();

        $scope.funChangeCategory = function funChangeCategory() {
            var categoryId = $scope.listCategories.CategoryID;
            if (categoryId > 0) {
                LoadProductCategory(categoryId);
            }
            else {
                $scope.listPCategories = {};
            }
        }

        $scope.LoadProductCategory = LoadProductCategory;
        function LoadProductCategory(categoryId) {
            var consfig = {
                params: {
                    categoryId: categoryId
                }
            };
            apiService.get('/api/productcategory/getByCategoryId', consfig, function (result) {
                $scope.listPCategories = result.data;
            }, function () {
                notificationService.displayError('Không thể tải danh sách thể loại');
            });
        }

        // list
        $scope.editProduct = [];
        $scope.lstProduct = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.showFrom = 0;
        $scope.showTo = 0;

        $scope.ListProduct = ListProduct;
        function ListProduct(page) {

            page = page || 0;

            var statusId = $scope.listStatus.StatusID;
            if (statusId === undefined || statusId === 'undefined' || statusId === null)
                statusId = 0;

            var categoryID = $scope.productCategoryID;
            if (categoryID === undefined || categoryID === 'undefined' || categoryID === null)
                categoryID = 0;

            var consfig = {
                params: {
                    page: page,
                    pageSize: $scope.valueShow,
                    keyWord: $scope.keyWord,
                    categoryID: categoryID,
                    status: statusId
                }
            };
            var url = '/api/product/getallpaging';
            $scope.promise = apiService.get(url, consfig, function (result) {
                $scope.lstProduct = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;

                for (var i = 0, length = $scope.lstProduct.length; i < length; i++) {
                    $scope.editProduct[$scope.lstProduct[i].ProductID] = false;
                }

            }, function () {
                notificationService.displayError('Không thể tải danh sách sản phẩm');
            });

            $scope.$parent.MethodShowLoading("Đang tải dữ liệu", $scope.promise);
        }
        $scope.ListProduct();

        /*$scope.sortColumn = 'ProductName';
        $scope.reverse = true; // sắp xếp giảm dần
        $scope.sortData = function (column) {
            if ($scope.sortColumn === column)
                $scope.reverse = !$scope.reverse;
            else
                $scope.reverse = true;
            $scope.sortColumn = column;
        };
        $scope.getSortClass = function (column) {
            if ($scope.sortColumn === column) {
                return $scope.reverse ? 'arrow-down' : 'arrow-up';
            }

            return '';
        };*/

        // onclick Update Product
        $scope.fnModifyProduct = function (item) {
            if (item.Status === false) {
                notificationService.displayWarning('Sảm phẩm đã ngừng kinh doanh.');
            }
            else {
                $scope.editProduct[item.ProductID] = true;
            }
           
        };
        // event Update ProductName, PriceSell
        $scope.fnUpdateProduct = function (item) {
            if (item.ProductName === '' || item.ProductName === null) {
                notificationService.displayError('Vui lòng nhập tên sản phẩm!');
                var name = 'txtProductName_' + item.ProductID;
                angular.element('input[name=' + name + ']').focus();
            }
            else {
                var url = '/api/product/updateNameAndPriceSell';
                $scope.promise = apiService.put(url, item, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.editProduct[item.ProductID] = false;
                    ListProduct();
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        };
        $scope.fnCancelProduct = function (item) {
            $scope.editProduct[item.ProductID] = false;
            ListProduct();
        };

        // select multi
        $scope.selectDelete = false;
        $scope.selectAll = selectAll;
        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.lstProduct, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
                $scope.selectDelete = true;
            }
            else {
                angular.forEach($scope.lstProduct, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
                $scope.selectDelete = false;
            }
        }

        // Lắng nghe sự thay đổi của lstProductCategory,
        // co 2 tham so: 1 - lang nghe ten bien lstProductCategory
        //               2 - function (new, old) va filter nhung gia tri moi la true thi vao danh sach da dc checked
        $scope.$watch("lstProduct", function (newCheck, old) {
            var checked = $filter("filter")(newCheck, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDeleteMulti').removeAttr('disabled');
                $scope.selectDelete = true;
            } else {
                $('#btnDeleteMulti').attr('disabled', 'disabled');
                $scope.selectDelete = false;
            }
        }, true);

        $scope.deleteMulti = deleteMulti;

        function deleteMulti() {
            $ngBootbox.confirm('Bạn có muốn chắc xóa những mục đã chọn?').then(function () {

                var listId = [];
                $.each($scope.selected, function (i, item) {
                    listId.push(item.ProductID);
                });
                if (listId.length > 0) {
                    var consfigs = {
                        params: {
                            jsonlistId: JSON.stringify(listId)
                        }
                    };
                    var url = '/api/product/deletemulti';
                    $scope.promise = apiService.del(url, consfigs, function (result) {
                        if (result.status === 400)
                            notificationService.displayError('Xóa không thành công! Vui lòng kiểm tra lại.');
                        else
                            notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi');
                        ListProductCategory();
                    }, function () {
                        notificationService.displayError('Xóa không thành công! Vui lòng kiểm tra lại.');
                    });

                    $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);

                }
                else {
                    notificationService.displayError('Không có bản ghi nào được lựa chọn! Vui lòng kiểm tra lại.');
                }
            });
        }

        // phục hồi thể loại
        $scope.RefreshProductCategory = RefreshProductCategory;
        function RefreshProductCategory(productCategoryId, productCategoryName) {
            var name = '<strong> ' + productCategoryName + '</strong>';
            $ngBootbox.confirm(' Bạn có muốn phục hồi thể loại' + name + '?').then(function () {

                alert('ok ' + productCategoryName);
            });
        }

        // Export product
        $scope.ExportProduct = ExportProduct;
        function ExportProduct() {
            var productCategoryID = $scope.productCategoryID;
            if (productCategoryID === undefined || productCategoryID === 'undefined' || productCategoryID === null)
                productCategoryID = 0;
            var statusId = $scope.listStatus.StatusID;
            if (statusId === undefined || statusId === 'undefined' || statusId === null)
                statusId = 0;

            var config = {
                params: {
                    keyword: $scope.keyWord,
                    productCategoryId: productCategoryID,
                    statusID: statusId
                }
            }
            var url = '/api/product/exportExcel';
            $scope.promise = apiService.get(url, config, function (result) {
                window.open(result.data.Message);
            }, function (result) {
                notificationService.displayError(result.data);
            });
            $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
        }    

        // show image
        $scope.onShowMoreImages = function (item) {
            console.log(item);
            //angular.element("button[id='btnCloseImportExcel']").click();
            angular.element("button[name='btnshowImages']").click();
        };
    }
})(angular.module('sms.product'));
