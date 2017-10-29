//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('categoryListController', categoryListController)

    categoryListController.$inject = ['$scope',
        'apiService', '$interval', '$filter', '$ngBootbox', '$state', 'notificationService'];

    function categoryListController($scope, apiService, $interval,
        $filter, $ngBootbox, $state, notificationService) {

        $scope.onClickAddCategory = function () {
            $state.go('categoryAdd');         
        }

        // show
        $scope.options = [
            { name: 10, value: 10 },
            { name: 25, value: 25 },
            { name: 50, value: 50 },
            { name: 100, value: 100 }];
        $scope.valueShow = $scope.options[0].value;
        $scope.changeShow = function () {
            ListCategory();
        }

        $scope.onChangeStatus = function () {
            ListCategory();
        };

        $scope.keyWord = '';
        $scope.onSearch = function () {
            ListCategory();
        }

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

        // list
        $scope.lstCategory = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.totalCount = 0;
        $scope.showFrom = 0;
        $scope.showTo = 0;

        $scope.ListCategory = ListCategory;
        function ListCategory(page) {
          
            page = page || 0;

            var statusId = $scope.listStatus.StatusID;
            var consfig = {
                params: {                   
                    page: page,
                    pageSize: $scope.valueShow,
                    keyWord: $scope.keyWord,
                    status: statusId
                }
            }
            var url = '/api/category/getall';
            $scope.promise = apiService.get(url, consfig, function (result) {
                $scope.lstCategory = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                /*$scope.showFrom = result.data.ShowFrom;
                $scope.showTo = result.data.ShowTo;
                $scope.strDate = result.data.StrDate;
                $scope.strHour = result.data.StrHour;
                $scope.strUser = result.data.StrUser;*/

                /*if ($scope.strDate == null || $scope.strDate == "")
                {
                    $scope.showMsg = false;
                }
                else
                    $scope.showMsg = true;*/
               
            }, function () {
                notificationService.displayError('Không thể tải danh sách danh mục');
            });

            $scope.$parent.MethodShowLoading("Đang tải dữ liệu", $scope.promise);
        }
        $scope.ListCategory();

        $scope.sortColumn = 'CategoryName';
        $scope.reverse = true; // sắp xếp giảm dần
        $scope.sortData = function (column)
        {
            if ($scope.sortColumn == column)
                $scope.reverse = !$scope.reverse;
            else
                $scope.reverse = true;
            $scope.sortColumn = column;
        }
        $scope.getSortClass = function(column)
        {
            if ($scope.sortColumn == column)
            {
                return $scope.reverse ? 'arrow-down' : 'arrow-up';
            }
                
            return '';
        }

        // select multi
        $scope.selectDelete = false;
        $scope.selectAll = selectAll;
        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll == false) {
                angular.forEach($scope.lstCategory, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
                $scope.selectDelete = true;
            }
            else {
                angular.forEach($scope.lstCategory, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
                $scope.selectDelete = false;
            }
        }

        // Lắng nghe sự thay đổi của lstProductCategory,
        // co 2 tham so: 1 - lang nghe ten bien lstProductCategory
        //               2 - function (new, old) va filter nhung gia tri moi la true thi vao danh sach da dc checked
        $scope.$watch("lstCategory", function (newCheck, old) {
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
                console.log($scope.selected);
                $.each($scope.selected, function (i, item) {
                    listId.push(item.CategoryID);
                });
                if (listId.length > 0)
                {
                    var consfigs = {
                        params: {
                            jsonlistId: JSON.stringify(listId)
                        }
                    }
                    var url = '/api/category/deletemulti';
                    $scope.promise = apiService.del(url, consfigs, function (result) {
                        if (result.status == 400)
                            notificationService.displayError('Xóa không thành công! Vui lòng kiểm tra lại.');
                        else
                            notificationService.displaySuccess('Xóa thành công ' + result.data + ' bản ghi');
                        ListCategory();
                    }, function (result) {
                        notificationService.displayError(result.data);
                    });

                    $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);

                }
                else
                {
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
})(angular.module('sms.category'));
