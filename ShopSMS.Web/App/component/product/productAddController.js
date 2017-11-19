//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['$scope',
        'apiService', '$interval', '$filter', '$ngBootbox', '$state', 'notificationService'];

    function productAddController($scope, apiService, $interval,
        $filter, $ngBootbox, $state, notificationService) {
        $scope.producerInfo = {};
        $scope.listProducer = {};
        $scope.editProducer = {};
        $scope.productInfo = {};
        $scope.avatar = '/UploadFiles/images/no-image.jpg';

        $scope.productCategories = {};
        function LoadProductCategory() {
            apiService.get('/api/productcategory/getallNoPage', null, function (result) {
                $scope.productCategories = result.data;
            }, function () {
                notificationService.displayError('Không thể tải danh sách thể loại');
            });
        }
        LoadProductCategory();

        // NXS
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

        $scope.AddProducer = AddProducer;
        function AddProducer() {

            if ($scope.producerInfo.ProducerName == null || $scope.producerInfo.ProducerName == '') {
                notificationService.displayError('Vui lòng nhập tên nhà sản xuất!');
            }
            else {
                var url = '/api/nsx/create';
                $scope.promise = apiService.post(url, $scope.producerInfo, function (result) {
                    LoadProducer();
                    $scope.productInfo.ProducerID = result.data.ProducerID;
                    angular.element("input[id='txtProducerName']").val('');
                    angular.element("button[id='btnCloseProducer']").click();
                    notificationService.displaySuccess("Thêm mới thành công!");                    
                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        }

        $scope.onCreateProducer = onCreateProducer;
        function onCreateProducer()
        {
            AddProducer();
        }

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
            var msg = "Bạn có chắc muốn xóa nhà sản xuất <strong>" + name + "</strong> ?";
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

        // manager product
        $scope.moreImages = [];
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {

                    var index = $scope.moreImages.indexOf(fileUrl);
                    if (index > -1) {
                        notificationService.displayWarning('Ảnh đã được lựa chọn trước đó');
                    }
                    else {
                        $scope.moreImages.push(fileUrl);
                        $scope.avatar = $scope.moreImages[0];
                    }
                })
            }
            finder.popup();
        }

        $scope.ckeditorOptions = {
            language: 'vi',
            height: '200px'
        }

        $scope.fnRemoveImg = function (item) {
            var index = $scope.moreImages.indexOf(item);
            if (index > -1) {
                $scope.moreImages.splice(index, 1);
            }

            if ($scope.moreImages.length > 0) {
                $scope.avatar = $scope.moreImages[0];
            }
            else {
                $scope.avatar = '/UploadFiles/images/no-image.jpg';
            }
        }

        $scope.AddProduct = AddProduct;
        function AddProduct() {
            if ($scope.productInfo.ProductName === null || $scope.productInfo.ProductName === '') {
                notificationService.displayError('Vui lòng nhập tên sản phẩm!');
            }
            else {

                $scope.productInfo.Avatar = $scope.avatar,
                $scope.productInfo.MoreImages = JSON.stringify($scope.moreImages)
               
                var url = '/api/product/create';
                $scope.promise = apiService.post(url, $scope.productInfo, function (result) {
                    //LoadCategory();
                    notificationService.displaySuccess('Thêm mới thành công!');
                    //$scope.pcInfo.CategoryID = result.data.CategoryID;
                    //angular.element("input[id='txtCategoryName']").val('');
                    //angular.element("button[id='btnCloseFormCate']").click();

                }, function (result) {
                    notificationService.displayError(result.data);
                });
                $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
            }
        }

        $scope.showTab1 = function () {
            if ($("#tab2").hasClass("active")){
                $("#tab2").removeClass("active");
            }
            $("#tab1").addClass("active");
        }

        $scope.showTab2 = function () {
            if ($("#tab1").hasClass("active")) {
                $("#tab1").removeClass("active");
            }
            $("#tab2").addClass("active");
        }
    }
})(angular.module('sms.product'));

