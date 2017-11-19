(function (app) {
    app.controller('otherController', otherController);

    otherController.$inject = ['$scope', 'apiService', 'notificationService', '$filter', '$ngBootbox'];

    function otherController($scope, apiService, notificationService, $filter, $ngBootbox) {

        $scope.listStatus = {};
        function LoadStatus() {
            apiService.get('/api/other/getListStatus', null, function (result) {
                $scope.listStatus = result.data;
            }, function () {
                notificationService.displayError('Không thể tải danh sách trạng thái');
            });
        }
        LoadStatus();

        $scope.editingCaterogy = {};
        $scope.categories = {};
        function LoadCategory() {
            apiService.get('/api/category/getallNoPage', null, function (result) {
                $scope.categories = result.data;
                for(var i = 0, length = $scope.categories.length; i < length; i++) {
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
            var msg = "Bạn có chắc muốn xóa danh mục [<strong>" + cateName + "</strong>] ?";
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
            //LoadCategory();
        };

        $scope.cateInfo = {};
        $scope.AddCategory = AddCategory;
        function AddCategory() {
            if ($scope.cateInfo.CategoryName === null || $scope.cateInfo.CategoryName === '') {
                notificationService.displayError('Vui lòng nhập tên danh mục!');
            }
            else {
                var url = '/api/category/create';
                $scope.promise = apiService.post(url, $scope.cateInfo, function (result) {
                    $scope.cateInfo = null;
                    LoadCategory();
                    notificationService.displaySuccess('Thêm mới thành công!');
                    angular.element("button[id='btnCloseFormCate']").click();
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        }


        // Manager Product Category
        $scope.options = [
           { name: 10, value: 10 },
           { name: 25, value: 25 },
           { name: 50, value: 50 },
           { name: 100, value: 100 }];
        $scope.valueShow = $scope.options[0].value;
        $scope.changeShow = function () {
            ListProductCategory();
        };

        $scope.keyWord = '';
        $scope.onSearch = function () {
            ListProductCategory();
        };

        $scope.onChangeCategory = function () {
            ListProductCategory();
        }

        $scope.onChangeStatus = function () {
            ListProductCategory();
        }

        $scope.editProductCategory = {};
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
                    $scope.pcInfo = null;
                    angular.element("button[id='btnClosePC']").click();
                    notificationService.displaySuccess(result.data);
                    ListProductCategory();
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        }

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

        $scope.fnModifyPC = function (item) {
            $scope.editProductCategory[item.ProductCategoryID] = true;
        };

        $scope.fnDeletePC = function (item) {
            var pcateName = item.ProductCategoryName;
            var msg = "Bạn có chắc muốn xóa thể loại [<strong>" + pcateName + "</strong>] ?";
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
            //ListProductCategory();
        };

        //Manager NXS
        $scope.editProducer = {};
        $scope.listProducer = {};
        function LoadProducer() {
            apiService.get('/api/nsx/getall', null, function (result) {
                $scope.listProducer = result.data;
                for (var i = 0, length = $scope.listProducer.length; i < length; i++) {
                    $scope.editProducer[$scope.listProducer[i].ProducerID] = false;
                }
            }, function (result) {
                notificationService.displayError('Không thể tải danh sách nhà cung cấp');
            });
        }
        LoadProducer();

        // onclick Update Producer
        $scope.fnModifyProducer = function (item) {
            $scope.editProducer[item.ProducerID] = true;
        };
        // event Update Producer
        $scope.fnUpdateProducer = function (item) {
            if (item.ProducerName === '' || item.ProducerName === null) {
                notificationService.displayError('Tên nhà sản xuất không được bỏ trống!');
                var name = 'txtProducerName_' + item.ProducerID;
                angular.element('input[name=' + name + ']').focus();
            }
            else {
                var url = '/api/nsx/update';
                $scope.promise = apiService.put(url, item, function (result) {
                    notificationService.displaySuccess(result.data);
                    $scope.editProducer[item.ProducerID] = false;
                    LoadProducer();
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        };
        // event Delete Producer
        $scope.fnDeleteProducer = function (item) {
            var name = item.ProducerName;
            var msg = "Bạn có chắc muốn xóa nhà sản xuất [<strong>" + name + "</strong>] ?";
            $ngBootbox.confirm(msg).then(function () {
                var consfigs = {
                    params: {
                        id: item.ProducerID
                    }
                };
                var url = '/api/nsx/delete';
                $scope.promise = apiService.del(url, consfigs, function (result) {
                    if (result.status === 400)
                        notificationService.displayError(result.data);
                    else
                        notificationService.displaySuccess('Xóa thành công nhà sản xuất ' + name);
                    LoadProducer();
                }, function (result) {
                    notificationService.displayError(result.data);
                });

                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            });
        };

        $scope.fnCancelProducer = function (item) {
            $scope.editProducer[item.ProducerID] = false;
            LoadProducer();
        };

        $scope.producerInfo = {};
        $scope.AddProducer = AddProducer;
        function AddProducer() {

            if ($scope.producerInfo.ProducerName == null || $scope.producerInfo.ProducerName == '') {
                notificationService.displayError('Vui lòng nhập tên nhà sản xuất!');
            }
            else {
                var url = '/api/nsx/create';
                $scope.promise = apiService.post(url, $scope.producerInfo, function (result) {
                    $scope.producerInfo = null;
                    $("#btnCloseProducer").click();
                    LoadProducer();
                    $scope.productInfo.ProducerID = result.data.ProducerID;
                    notificationService.displaySuccess("Thêm mới thành công!");
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        }


        $scope.tab1 = true;
        $scope.tab2 = false;
        $scope.tab3 = false;
        $scope.showTab1 = function () {
            if ($("#tab2").hasClass("active")) {
                $("#tab2").removeClass("active");
            }
            else if ($("#tab3").hasClass("active")){
                $("#tab3").removeClass("active");
            }

            $("#tab1").addClass("active");
            $scope.tab1 = true;
            $scope.tab2 = false;
            $scope.tab3 = false;
        }
        $scope.showTab2 = function () {
            if ($("#tab1").hasClass("active")) {
                $("#tab1").removeClass("active");
            }
            else if ($("#tab3").hasClass("active"))
            {
                $("#tab3").removeClass("active");
            }
            $("#tab2").addClass("active");
            $scope.tab1 = false;
            $scope.tab2 = true;
            $scope.tab3 = false;
        }
        $scope.showTab3 = function () {
            if ($("#tab1").hasClass("active")) {
                $("#tab1").removeClass("active");
            }
            else if ($("#tab2").hasClass("active")) {
                $("#tab2").removeClass("active");
            }
            $("#tab3").addClass("active");

            $scope.tab1 = false;
            $scope.tab2 = false;
            $scope.tab3 = true;
        }
    }
})(angular.module('sms.other'));
