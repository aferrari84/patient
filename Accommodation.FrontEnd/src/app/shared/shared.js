(function () {
    "use strict";
    function LeavingPageController(scope, modalInstance) {
        scope.confirm = function () {
            modalInstance.close(true);
        };
        scope.close = function () {
            modalInstance.dismiss();
        };
    }
    LeavingPageController.$inject = ['$scope', '$modalInstance'];
    angular.module('AccommodationApp.Shared', ['ui.router', 'ui.bootstrap'])
            .controller('LeavingPageCtrl', LeavingPageController);
})();