(function () {
    'use strict';

    angular
        .module('app')
        .controller('menuGroupEditController', menuGroupEditController);

    menuGroupEditController.$inject = ['$location']; 

    function menuGroupEditController($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'menuGroupEditController';

        activate();

        function activate() { }
    }
})();
