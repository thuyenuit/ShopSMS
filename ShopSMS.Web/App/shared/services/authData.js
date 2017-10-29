(function (app) {
    app.factory('authData', [
        function () {
            var authDataFactory = {};
            var authentication = {
                IsAuthenticated: true,
                userName: "",
                userCode: "",
                fullName: "",
                listMenuInfo : []
            };
            authDataFactory.authenticationData = authentication;

            return authDataFactory;
        }
    ]);
})(angular.module('sms.common'));