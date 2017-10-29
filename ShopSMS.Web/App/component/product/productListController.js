//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productListController', productListController);

    productListController.$inject = ['$scope',
        'apiService', '$interval', '$filter', '$ngBootbox', '$state', 'notificationService'];

    function productListController($scope, apiService, $interval,
        $filter, $ngBootbox, $state, notificationService) {

        $scope.onClickAddProduct = function () {
            $state.go('productAdd');
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
        $scope.listStatus.StatusID = 0;
        function LoadStatus() {
            apiService.get('/api/other/getListStatus', null, function (result) {
                $scope.listStatus = result.data;
                $scope.listStatus.StatusID = 0;
            }, function () {
                notificationService.displayError('Không thể tải danh sách trạng thái');
            });
        }
        LoadStatus();

        $scope.categories = [];
        function LoadProCategory() {
            apiService.get('/api/productcategory/getallNoPage', null, function (result) {
                $scope.categories = result.data;
            }, function () {
                notificationService.displayError('Không thể tải danh sách thể loại');
            });
        }
        LoadProCategory();

        // list
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
            var categoryID = $scope.categories.ProductCategoryID;
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
            var url = '/api/product/getall';
            $scope.promise = apiService.get(url, consfig, function (result) {
                $scope.lstProduct = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                notificationService.displayError('Không thể tải danh sách sản phẩm');
            });

            $scope.$parent.MethodShowLoading("Đang tải dữ liệu", $scope.promise);
        }
        $scope.ListProduct();

        $scope.sortColumn = 'ProductName';
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

    }
})(angular.module('sms.product'));
