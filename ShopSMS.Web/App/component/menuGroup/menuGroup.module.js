<reference path="/Assets/Admin/libs/angular/angular.js" />
(function () {
    angular.module('sms.menuGroup', ['sms.common', 'scrollable-table']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('menuGroups', {
                url: '/list-menu-group',
                templateUrl: '/app/component/menuGroup/menuGroupListView.html',
                controller: 'menuGroupListController'
            })
            .state('menuGroupAdd', {
                url: '/add-menu-group',
                templateUrl: '/app/component/menuGroup/menuGroupAddView.html',
                controller: 'menuGroupAddController'
            })
            .state('menuGroupEdit', {
                url: '/edit-menu-group?Id',
                templateUrl: '/app/component/menuGroup/menuGroupEditView.html',
                controller: 'menuGroupEditController'
            });
    }
})();