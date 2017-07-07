(function () {
    "use strict";
    /**
     * Config the routes for Home Module
     * @param stateProvider
     * @constructor
     */
    function Config(stateProvider) {
       
        stateProvider.state('base.accommodation', {
            url: '/alojamientos',
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
    function AccommodationController(rootScope, scope, state, Lodging) {
        

        var lodgingId = state.current.data.locationId;

        Lodging.get_by_location(lodgingId).then(function (data) {
            scope.accommodations = data.ResponseData;
            scope.total_accommodations = data.ResponseData.length;
        });

        scope.getTimeStamp = function () {
            var stamp = new Date().getTime().toString();
            return stamp;
        };
    }




//injectors
    Config.$inject = ['$stateProvider'];
    AccommodationController.$inject = ['$rootScope', '$scope', '$state', 'Lodging'];

    angular.module('AccommodationApp.Accommodation', ['ui.router', 'AccommodationApp.Base'])
        .config(Config)
        .controller('AccommodationCtrl', AccommodationController);
}());
