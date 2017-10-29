/// <reference path="/Assets/Admin/libs/angular/angular.js" />

(function () {
    angular.module('sms.category', ['sms.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('categories', {
                parent: 'baseView',
                url: '/list-category',
                templateUrl: '/app/component/category/categoryListView.html',
                controller: 'categoryListController'
            })
            .state('categoryAdd', {
                parent: 'baseView',
                url: '/add-category',
                templateUrl: '/app/component/category/categoryAddView.html',
                controller: 'categoryAddController'
            })
            .state('categoryEdit', {
                parent: 'baseView',
                url: '/edit-category?Id',
                templateUrl: '/app/component/category/categoryEditView.html',
                controller: 'categoryEditController'
            });
    }
})();