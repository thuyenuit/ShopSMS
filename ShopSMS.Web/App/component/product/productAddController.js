//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['$scope',
        'apiService', '$interval', '$filter', '$ngBootbox', '$state', 'notificationService'];

    function productAddController($scope, apiService, $interval,
        $filter, $ngBootbox, $state, notificationService) {

        $scope.moreImages = [];

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                //console.log(fileUrl);
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                })               
            }
            finder.popup();       
        }
    }
})(angular.module('sms.product'));

