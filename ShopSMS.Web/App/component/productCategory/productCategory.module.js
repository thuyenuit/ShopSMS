/// <reference path="/Assets/Admin/libs/angular/angular.js" />

(function () {
    angular.module('sms.productCategory', ['sms.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        //if (authData.authenticationData.accessToken !== null && authData.authenticationData.accessToken !== 'undefined'
        //     && authData.authenticationData.accessToken.length > 0) {
            $stateProvider
               .state('productCategories', {
                   parent: 'baseView',
                   url: '/list-product-category',
                   templateUrl: '/app/component/productCategory/productCategoryListView.html',
                   controller: 'productCategoryListController'
               })
               .state('productCategoryAdd', {
                   parent: 'baseView',
                   url: '/add-product-category',
                   templateUrl: '/app/component/productCategory/productCategoryAddView.html',
                   controller: 'productCategoryAddController'
               })
               .state('productCategoryEdit', {
                   parent: 'baseView',
                   url: '/edit-product-category?categoryId',
                   templateUrl: '/app/component/productCategory/productCategoryEditView.html',
                   controller: 'productCategoryEditController'
               });
        //}
        //else {
        //    $urlRouterProvider.otherwise('/login');
        //}

    }
})();