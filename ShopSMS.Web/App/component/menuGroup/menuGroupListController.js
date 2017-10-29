(function () {
    'use strict';

    angular
        .module('app')
        .controller('menuGroupListController', menuGroupListController);

    menuGroupListController.$inject = ['$location']; 

    function menuGroupListController($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'menuGroupListController';

        activate();

        function activate() { }
    }
})();
