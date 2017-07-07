(function () {
    "use strict";

    var $stateProviderRef = null;

    /**
     * Configuration for module Base
     * @param stateProvider
     * @constructor
     */
    function Config(stateProvider) {

        $stateProviderRef = stateProvider;

        stateProvider.state('base', {
            abstract: true,
            views: {
                "container": {
                    templateUrl: "base/base.tpl.html"
                }
            }
        });

        stateProvider.state('base.static_401', {
            url: '/error_401',
            views: {
                "main": {
                    controller: 'StaticPagesCtrl',
                    templateUrl: 'static_pages/401.tpl.html'
                }
            },
            data: { title: '401 Unauthorized'}
        });    
        
        stateProvider.state('base.static_403', {
            url: '/error_403',
            views: {
                "main": {
                    controller: 'StaticPagesCtrl',
                    templateUrl: 'static_pages/403.tpl.html'
                }
            },
            data: { title: '403 Forbbiden'}
        }); 

        stateProvider.state('base.static_404', {
            url: '/error_404',
            views: {
                "main": {
                    controller: 'StaticPagesCtrl',
                    templateUrl: 'static_pages/404.tpl.html'
                }
            },
            data: { title: '404 Not Found'}
        });

    }

    function Run(state, urlRouter, location, HashTag, Location) {

        var path = location.path().replace('/', 'base.');


        Location.get_all().then(function (data) {
            angular.forEach(data.ResponseData, function (value, key) {
                AddLocationStateProvider(value);
            });

            HashTag.get_all().then(function (data) {
                angular.forEach(data.ResponseData, function (value, key) {
                    AddHashTagStateProvider(value);
                });

                if (state.get(path)) {
                    state.go(path);
                } else {
                    if (path !== "base." && path !== "base.login" && path !== "base.logout") {
                        state.go('base.static_404');
                    }
                }
            });
        });

        

        urlRouter.sync();
        urlRouter.listen();

        if (state.get(path)) {
            state.go(path);
        }


        HashTag.reload_cache().then(function (data) { });
        Location.reload_cache().then(function (data) { });
    }

    function AddLocationStateProvider(value)
    {
        var stateName = value.Hashtag.split(' ').join('').toLowerCase();
        
        var state = {
            "url": "/" + stateName,
            "views": {
                "main": {
                    controller: 'HomeLocationCtrl',
                    templateUrl: 'home/home_location.tpl.html'
                },
                "menu": {
                    controller: 'LeftSideMenuCtrl',
                    templateUrl: 'base/left_side.tpl.html'
                }
            },
            data: {
                title: value.Name,
                hashtag: value.Hashtag,
                locationId: value.ID
            }
        };

        var stateAccommodation = {
            "url": "/" + stateName + "/alojamientos",
            "views": {
                "main": {
                    controller: 'AccommodationCtrl',
                    templateUrl: 'accommodation/accommodation.tpl.html'
                }
            },
            data: {
                title: value.Name + ' | Alojamientos',
                hashtag: value.Hashtag,
                locationId: value.ID
            }
        };

        var stateService = {
            "url": "/" + stateName + "/servicios",
            "views": {
                "main": {
                    controller: 'OfferedServiceCtrl',
                    templateUrl: 'service/service.tpl.html'
                }
            },
            data: {
                title: value.Name + ' | Servicios',
                hashtag: value.Hashtag,
                locationId: value.ID
            }
        };

        var stateTourism = {
            "url": "/" + stateName + "/turismo",
            "views": {
                "main": {
                    controller: 'TourismCtrl',
                    templateUrl: 'tourism/tourism.tpl.html'
                }
            },
            data: {
                title: value.Name + ' | Turismo',
                hashtag: value.Hashtag,
                locationId: value.ID
            }
        };

        $stateProviderRef.state('base.' + stateName, state);
        $stateProviderRef.state('base.' + stateName + '/alojamientos', stateAccommodation);
        $stateProviderRef.state('base.' + stateName + '/servicios', stateService);
        $stateProviderRef.state('base.' + stateName + '/turismo', stateTourism);
    }

    function AddHashTagStateProvider(value) {
        var stateName = value.HashTag.split(' ').join('').toLowerCase();

        var state = {
            "url": "/" + stateName,
            "views": {
                "main": {
                    controller: 'Home' + value.Type + 'Ctrl',
                    templateUrl: 'home/home_' + value.Type.toLowerCase() + '.tpl.html'
                },
                "menu": {
                    controller: 'LeftSideMenuCtrl',
                    templateUrl: 'base/left_side.tpl.html'
                }
            },
            data: {
                title: value.Name,
                hashtag: value.LocationTag,
                ID: value.ID
            }
        };


        $stateProviderRef.state('base.' + stateName, state);
    }

    function LeftSideMenuController(scope, rootScope, state, applicationConfiguration) {

         scope.location = state.current.data.hashtag;

    }

    Config.$inject = ['$stateProvider'];
    Run.$inject = ['$state', '$urlRouter', '$location', 'HashTag', 'Location'];
    LeftSideMenuController.$inject = ['$scope', '$rootScope', '$state', 'applicationConfiguration'];


    angular.module('AccommodationApp.Base', ['ui.router', 'AccommodationApp.Services', 'AccommodationApp.StaticPages']).config(Config).run(Run)
    .controller('LeftSideMenuCtrl', LeftSideMenuController);
}());