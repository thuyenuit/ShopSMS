//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productEditController', productEditController);

    productEditController.$inject = ['$scope',
        'apiService', '$interval', '$filter', '$ngBootbox', '$state', 'notificationService'];

    function productEditController($scope, apiService, $interval,
        $filter, $ngBootbox, $state, notificationService) {

    }
})(angular.module('sms.product'));


