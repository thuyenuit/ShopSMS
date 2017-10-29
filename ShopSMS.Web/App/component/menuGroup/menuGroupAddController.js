(function () {
    'use strict';

    angular
        .module('app')
        .controller('menuGroupAddController', menuGroupAddController);

    menuGroupAddController.$inject = ['$location'];

    function menuGroupAddController($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'menuGroupAddController';

        activate();

        function activate() { }
    }
})();
