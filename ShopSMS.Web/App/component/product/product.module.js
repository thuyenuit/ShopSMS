/// <reference path="/Assets/Admin/libs/angular/angular.js" />

(function () {
    angular.module('sms.product', ['sms.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('products', {
                parent: 'baseView',
                url: '/list-product',
                templateUrl: '/app/component/product/productListView.html',
                controller: 'productListController'
            })
            .state('productAdd', {
                parent: 'baseView',
                url: '/list-product/add-product',
                templateUrl: '/app/component/product/productAddView.html',
                controller: 'productAddController'
            })
            .state('productEdit', {
                parent: 'baseView',
                url: '/list-product/edit-product?productID',
                templateUrl: '/app/component/product/productEditView.html',
                controller: 'productEditController'
            }).state('productImport', {
                 parent: 'baseView',
                 url: '/list-product/import-product',
                 templateUrl: '/app/component/product/productImportView.html',
                 controller: 'productImportController'
             });
    }
})();