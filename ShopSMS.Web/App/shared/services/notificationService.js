(function (app) {
    app.factory('notificationService', notificationService);
    function notificationService() {
        toastr.options = {
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "fadeIn": 300,
            "fadeOut": 1000,
            "timeOut": 4000,
            "extendedTimeOut": 1000
        };

        function displaySuccess(message) {
            toastr.success(message, 'Thông báo');
        }

        function displayError(error) {
            if (Array.isArray(error)) {
                error.each(function (err) {
                    toastr.error(err, 'Thông báo');
                })
            }
            else
                toastr.error(error, 'Thông báo');
        }

        function displayWarning(message) {
            toastr.warning(message, 'Thông báo');
        }

        function displayInfo(message) {
            toastr.info(message, 'Thông báo');
        }

        return {
            displaySuccess: displaySuccess,
            displayError: displayError,
            displayWarning: displayWarning,
            displayInfo: displayInfo
        }
    }
})(angular.module('sms.common'));