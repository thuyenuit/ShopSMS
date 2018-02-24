(function (app) {
    app.controller('productImportController', productImportController);

    productImportController.$inject = ['apiService', '$http', 'authenticationService', '$scope', 'notificationService', '$state', 'commonService'];

    function productImportController(apiService, $http, authenticationService, $scope, notificationService, $state, commonService) {

        $scope.listPCategories = {};
        $scope.listCategories = {};
        function LoadCategory() {
            apiService.get('/api/category/getallNoPage', null, function (result) {
                $scope.listCategories = result.data;
                console.log(result.data);
            }, function () {
                notificationService.displayError('Không thể tải danh sách danh mục');
            });
        }
        LoadCategory();

        $scope.funChangeCategory = function funChangeCategory() {
            var categoryId = $scope.categoryID;
            console.log(categoryId);
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

        // tải file mẫu
        $scope.DownloadTemplate = DownloadTemplate;
        function DownloadTemplate() {
            var url = '/api/product/downloadTemplate';
            $("#btnCloseImportExcel").click();
            $scope.promise = apiService.get(url, null, function (result) {
                window.open(result.data.Message);
            }, function (result) {
                notificationService.displayError(result.data);
            });
            $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
        }
      
        $scope.files = [];      
        $scope.$on("fileSelected", function (event, args) {
            $scope.$apply(function () {
                $scope.files.push(args.file);
            });
        });

        $scope.listErrores = {};
        $scope.filename = '';
        $scope.ImportProduct = ImportProduct;
        function ImportProduct() {
            if ($scope.files.length <= 0) {
                notificationService.displayError("Vui lòng chọn File excel cần import");
            }
            else {

                var ext = angular.element("input[name='file']").val().match(/\.([^\.]+)$/)[1];

                if (ext === 'xls' || ext === 'xlsx') {
                    authenticationService.setHeader();
                    var url = '/api/product/ImportExcel';                   
                    $scope.promise = $http({
                        method: "POST",
                        url: url,
                        headers: { 'Content-Type': undefined },
                        transformRequest: function (data) {
                            var formData = new FormData();
                            formData.append("categoryID", $scope.categoryID);
                            formData.append("productCategoryID", $scope.productCategoryID);
                            
                            for (var i = 0; i < data.files.length; i++) {
                                formData.append("file" + i, data.files[i]);
                            }
                            return formData;
                        },
                        data: { files: $scope.files }
                    }).then(function (result, status, headers, config) {
                        notificationService.displaySuccess(result.data);                      
                    }, function (result, status, headers, config) {

                        if (result.status === 502) {
                             $scope.listErrores = result.data;
                             $("button[name='btnShowError']").click();
                        }
                        else {
                            notificationService.displayError(result.data.ExceptionMessage);
                        }

                    });

                    $scope.$parent.MethodShowLoading("Đang xử lý", $scope.promise);
                }
                else {
                    notificationService.displayError("File không hợp lệ. Vui lòng chọn file excel");
                }
            }

            $scope.files = [];
            angular.element("input[name='file']").val('');
        }      
    }

})(angular.module('sms.product'));