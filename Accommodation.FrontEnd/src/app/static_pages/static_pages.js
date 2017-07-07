(function () {
    "use strict";

    function Config(stateProvider) {

    }



    function StaticPagesController(scope, state, rootScope) {
    }

    //Injections
    Config.$inject = ['$stateProvider'];
    StaticPagesController.$inject = ['$scope', '$state','$rootScope'];
    //ng module
    angular.module('AccommodationApp.StaticPages', ['ui.router', 'ui.mask', 'ngTagsInput', 'ui.bootstrap', 'AccommodationApp.Base'])
        .config(Config)
        .controller('StaticPagesCtrl', StaticPagesController);
}());