/// <reference path="/Assets/Admin/libs/angular/angular.js" />

(function (app) {
    app.controller('BaseController',
        ['$scope', 'authData', function ($scope, authData) {

            function LoadMenu(){
                
                console.log("Tên " ,authData.authenticationData.userName);
                console.log("Menu ", authData.authenticationData.listMenuInfo);
            }
            LoadMenu();

        }]);
})(angular.module('sms'));