(function () {
    "use strict";
    /**
     * Config the routes for Home Module
     * @param stateProvider
     * @constructor
     */
    function Config(stateProvider) {
        stateProvider.state('base.tourism', {
            url: '/turismo',
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
    function TourismController(scope, state, Tourism) {
        
        var tourismId = state.current.data.locationId;

        Tourism.get_by_location(tourismId).then(function (data) {
            scope.tourisms = data.ResponseData;
            scope.total_tourisms = data.ResponseData.length;
        });

    }




//injectors
    Config.$inject = ['$stateProvider'];
    TourismController.$inject = ['$scope', '$state', 'Tourism'];

    angular.module('AccommodationApp.Tourism', ['ui.router', 'AccommodationApp.Base'])
        .config(Config)
        .controller('TourismCtrl', TourismController);
}());
