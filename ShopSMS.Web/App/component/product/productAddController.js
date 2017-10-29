//<reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('productAddController', productAddController);

    productAddController.$inject = ['$scope',
        'apiService', '$interval', '$filter', '$ngBootbox', '$state', 'notificationService'];

    function productAddController($scope, apiService, $interval,
        $filter, $ngBootbox, $state, notificationService) {

    }
})(angular.module('sms.product'));

