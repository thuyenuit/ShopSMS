(function (app) {
    app.filter('homeflagFilter', function () {
        return function (input) {
            if (input == true)
                return '';
            else
                return '';
        }
    })
})(angular.module('sms.common'));