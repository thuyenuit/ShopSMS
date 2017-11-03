//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope', '$compile',
        'apiService', '$interval', '$filter', '$ngBootbox', '$state', 'notificationService'];

    function productCategoryListController($scope,$compile, apiService, $interval,
        $filter, $ngBootbox, $state, notificationService) {

            /*var htmlcontent = $('#loadhtml');
            htmlcontent.load('/App/component/productCategory/formCategory.html');
            $compile(htmlcontent.contents())($scope);*/

        // object the loai - them moi
        $scope.pcInfo = {};

        ///
        $scope.editProductCategory = {};
        $scope.editingCaterogy = {};

        // show
        $scope.options = [
            { name: 10, value: 10 },
            { name: 25, value: 25 },
            { name: 50, value: 50 },
            { name: 100, value: 100 }];
        $scope.valueShow = $scope.options[0].value;
        $scope.changeShow = function () {
            ListProductCategory();
        };

        $scope.onChangeStatus = function () {
            ListProductCategory();
        };

        $scope.categoryID = 0;
        $scope.onChangeCategory = function () {
            ListProductCategory();
        };

        $scope.keyWord = '';
        $scope.onSearch = function () {
            ListProductCategory();
        };

        $scope.listStatus = {};
        // $scope.listStatus.StatusID = 0;
        function LoadStatus() {
            apiService.get('/api/other/getListStatus', null, function (result) {
                $scope.listStatus = result.data;
                //  $scope.listStatus.StatusID = 0;
            }, function () {
                notificationService.displayError('Không thể tải danh sách trạng thái');
            });
        }
        LoadStatus();

        $scope.categories = {};
        function LoadCategory() {
            apiService.get('/api/category/getallNoPage', null, function (result) {
                $scope.categories = result.data;

                for (var i = 0, length = $scope.categories.length; i < length; i++) {
                    $scope.editingCaterogy[$scope.categories[i].CategoryID] = false;
                }           
            }, function () {
                notificationService.displayError('Không thể tải danh sách danh mục');
            });
        }
        LoadCategory();

        $scope.fnModifyCategory = function (item) {
            $scope.editingCaterogy[item.CategoryID] = true;
        };

        $scope.fnUpdateCategory = function (item) {
            if (item.CategoryName === '' || item.CategoryName === null) {
                notificationService.displayError('Tên thể loại không được bỏ trống!');
                var name = 'txtNameCate_' + item.CategoryID;
                angular.element('input[name=' + name + ']').focus();
            }
            else {
                var url = '/api/category/update';
                $scope.promise = apiService.put(url, item, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.editingCaterogy[item.CategoryID] = false;
                    LoadCategory();
                    ListProductCategory();
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        };

        $scope.fnDeleteCategory = function (item) {
            var cateName = item.CategoryName;
            var msg = "Bạn có chắc muốn xóa danh mục <strong>" + cateName + "</strong> ?";
            $ngBootbox.confirm(msg).then(function () {
               
                    var consfigs = {
                        params: {
                            id: item.CategoryID
                        }
                    };
                    var url = '/api/category/delete';
                    $scope.promise = apiService.del(url, consfigs, function (result) {
                        if (result.status === 400)
                            notificationService.displayError(result.data);
                        else
                            notificationService.displaySuccess('Xóa thành công danh mục ' + cateName);
                        LoadCategory();
                    }, function (result) {
                        notificationService.displayError(result.data);
                    });

                    $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            });
        };

        $scope.fnCancelCategory = function (item) {
            $scope.editingCaterogy[item.CategoryID] = false;
            LoadCategory();
        };

        // list
        $scope.lstProductCategory = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.showFrom = 0;
        $scope.showTo = 0;

        $scope.ListProductCategory = ListProductCategory;
        function ListProductCategory(page) {

            page = page || 0;

            var categoryID = $scope.categories.CategoryID;
            if (categoryID === undefined || categoryID === 'undefined' || categoryID === null)
                categoryID = 0;

            var statusId = $scope.listStatus.StatusID;
            if (statusId === undefined || statusId === 'undefined' || statusId === null)
                statusId = 0;

            var consfig = {
                params: {
                    page: page,
                    pageSize: $scope.valueShow,
                    keyWord: $scope.keyWord,
                    categoryID: categoryID,
                    status: statusId
                }
            };
            var url = '/api/productcategory/getall';
            $scope.promise = apiService.get(url, consfig, function (result) {
                $scope.lstProductCategory = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.showFrom = result.data.ShowFrom;
                $scope.showTo = result.data.ShowTo;

                for (var i = 0, length = $scope.lstProductCategory.length; i < length; i++) {
                    $scope.editProductCategory[$scope.lstProductCategory[i].ProductCategoryID] = false;
                }

            }, function () {
                notificationService.displayError('Không thể tải danh sách danh mục');
            });

            $scope.$parent.MethodShowLoading("Đang tải dữ liệu", $scope.promise);
        }
        $scope.ListProductCategory();

        $scope.fnModifyPC = function (item) {
            $scope.editProductCategory[item.ProductCategoryID] = true;
        };

        
        $scope.fnDeletePC = function (item) {
            var pcateName = item.ProductCategoryName;
            var msg = "Bạn có chắc muốn xóa thể loại <strong>" + pcateName + "</strong> ?";
            $ngBootbox.confirm(msg).then(function () {

                var consfigs = {
                    params: {
                        id: item.ProductCategoryID
                    }
                };
                var url = '/api/productcategory/delete';
                $scope.promise = apiService.del(url, consfigs, function (result) {
                    if (result.status === 400)
                        notificationService.displayError(result.data);
                    else
                        notificationService.displaySuccess('Xóa thành công thể loại ' + pcateName);
                    ListProductCategory();
                }, function (result) {
                    notificationService.displayError(result.data);
                });

                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            });
        };


        $scope.fnUpdatePC = function (item) {
            if (item.ProductCategoryName === '' || item.ProductCategoryName === null) {
                notificationService.displayError('Tên thể loại không được bỏ trống!');
                var name = 'txtName_' + item.ProductCategoryID;
                angular.element('input[name=' + name + ']').focus();
            }
            else {
                var url = '/api/productcategory/update';
                $scope.promise = apiService.put(url, item, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.editProductCategory[item.ProductCategoryID] = false;
                    ListProductCategory();
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        };

        $scope.fnCancelPC = function (item) {
            $scope.editProductCategory[item.ProductCategoryID] = false;
            ListProductCategory();
        };

        $scope.sortColumn = 'ProductCategoryName';
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
                angular.forEach($scope.lstProductCategory, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
                $scope.selectDelete = true;
            }
            else {
                angular.forEach($scope.lstProductCategory, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
                $scope.selectDelete = false;
            }
        }

        // Lắng nghe sự thay đổi của lstProductCategory,
        // co 2 tham so: 1 - lang nghe ten bien lstProductCategory
        //               2 - function (new, old) va filter nhung gia tri moi la true thi vao danh sach da dc checked
        $scope.$watch("lstProductCategory", function (newCheck, old) {
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
                    listId.push(item.ProductCategoryID);
                });
                if (listId.length > 0) {
                    var consfigs = {
                        params: {
                            jsonlistId: JSON.stringify(listId)
                        }
                    };
                    var url = '/api/productcategory/deletemulti';
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

        // add category
        $scope.cateInfo = {};
        $scope.AddCategory = AddCategory;
        function AddCategory() {
            if ($scope.cateInfo.CategoryName === null || $scope.cateInfo.CategoryName === '') {
                notificationService.displayError('Vui lòng nhập tên danh mục!');
            }
            else {
                var url = '/api/category/create';
                $scope.promise = apiService.post(url, $scope.cateInfo, function (result) {
                    LoadCategory();
                    console.log(result.data);
                    notificationService.displaySuccess('Thêm mới thành công!');                 
                    $scope.pcInfo.CategoryID = result.data.CategoryID;
                    angular.element("input[id='txtCategoryName']").val('');
                    angular.element("button[id='btnCloseFormCate']").click();
                    
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        }

        $scope.CreateCategory = function () {
            AddCategory();
        }

        // add product category
        $scope.AddProductCategory = AddProductCategory;
        function AddProductCategory() {

            if ($scope.pcInfo.CategoryID == null || $scope.pcInfo.CategoryID == ''
                || $scope.pcInfo.ProductCategoryName == null || $scope.pcInfo.ProductCategoryName == '') {
                notificationService.displayError('Dữ liệu nhập không hợp lệ!');
            }
            else {
                var url = '/api/productcategory/create';
                $scope.promise = apiService.post(url, $scope.pcInfo, function (result) {                  
                    angular.element("button[id='btnClosePC']").click();
                    notificationService.displaySuccess(result.data);
                    ListProductCategory();
                   // $scope.pcInfo = null;
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        }

    }
})(angular.module('sms.productCategory'));