(function () {
    "use strict";
    /**
     * Config the routes for Home Module
     * @param stateProvider
     * @constructor
     */
    function Config(stateProvider) {
       
        stateProvider.state('base.service', {
            url: '/servicios',
            views: {
                "main": {
                    controller: 'HomeCtrl',
                    templateUrl: 'home/home.tpl.html'
                }
            },
            data: {
                title: ''
            }
        });

    }

    /**
     * Home Controller for main view
     * @param rootScope
     * @constructor
     */
    function OfferedServiceController(scope, state, OfferedService) {
        
        var serviceId = state.current.data.locationId;

        OfferedService.get_by_location(serviceId).then(function (data) {
            scope.services = data.ResponseData;
            scope.total_services = data.ResponseData.length;
        });

        scope.getTimeStamp = function () {
            var stamp = new Date().getTime().toString();
            return stamp;
        };
    }




//injectors
    Config.$inject = ['$stateProvider'];
    OfferedServiceController.$inject = ['$scope', '$state', 'OfferedService'];

    angular.module('AccommodationApp.OfferedService', ['ui.router', 'AccommodationApp.Base'])
        .config(Config)
        .controller('OfferedServiceCtrl', OfferedServiceController);
}());
