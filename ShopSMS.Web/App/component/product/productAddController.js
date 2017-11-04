//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['$scope',
        'apiService', '$interval', '$filter', '$ngBootbox', '$state', 'notificationService'];

    function productAddController($scope, apiService, $interval,
        $filter, $ngBootbox, $state, notificationService) {

        $scope.avatar = '/UploadFiles/images/no-image.jpg';
        $scope.productInfo = {};

        $scope.moreImages = [];
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {

                    var index = $scope.moreImages.indexOf(fileUrl);
                    if (index > -1) {
                        notificationService.displayWarning('Ảnh đã được lựa chọn trước đó');
                    }
                    else
                    {
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
    }
})(angular.module('sms.product'));

