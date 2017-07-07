(function () {
    "use strict";
    /**
     * Config the routes for Home Module
     * @param stateProvider
     * @constructor
     */
    function Config(stateProvider) {

        stateProvider.state('base.location', {
            url: '/location/{id}',
            views: {
                "main": {
                    controller: 'LocationCtrl',
                    templateUrl: 'location/location.tpl.html'
                }
            },
            data: {
                title: 'Localidades'
            }
        });
    }

    /**
     * Home Controller for main view
     * @param rootScope
     * @constructor
     */
    function LocationController(scope, state, Location) {

        var locationId = state.current.data.locationId;
        scope.locations = [];

        scope.search_state = { State: { ID: state.params.id } };

        scope.searchState = function (value) {
            return (value.State.ID.toString() === scope.search_state.State.ID.toString());
        };

        Location.get_all(locationId).then(function (data) {
            scope.locations = data.ResponseData;
            scope.total_locations = data.ResponseData.length;
        });

    }




    //injectors
    Config.$inject = ['$stateProvider'];
    LocationController.$inject = ['$scope', '$state', 'Location'];

    angular.module('AccommodationApp.Location', ['ui.router', 'AccommodationApp.Base'])
        .config(Config)
        .controller('LocationCtrl', LocationController);
}());
