(function () {
    "use strict";
    /**
     * Config the routes for Home Module
     * @param stateProvider
     * @constructor
     */
    function Config(stateProvider) {
        stateProvider.state('base.home', {
            url: '/home',
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
    function HomeController(rootScope, state) {

        //rootScope.page_title = state.current.data.title;

    }

    /**
   * Home Controller for main view
   * @param rootScope
   * @constructor
   */
    function HomeLocationController(rootScope, scope, state, Advertising) {

        rootScope.page_title = state.current.data.title;

        scope.hashtag = state.current.data.hashtag;

        scope.locationName = state.current.data.title;

        Advertising.get_by_location(state.current.data.locationId).then(function (data) {
            var ad_list = data.ResponseData;

            scope.lodgingAds = [];
            scope.serviceAds = [];

            angular.forEach(ad_list, function (ad) {
                if (ad.IsAccommodation === true) {
                    scope.lodgingAds.push({ img: ad.ImageURL, link: ad.Link, external: ad.ExternalLink });
                } else {
                    scope.serviceAds.push({ img: ad.ImageURL, link: ad.Link, external: ad.ExternalLink });
                }
            });

            var standardLodgingAds = 16 - scope.lodgingAds.length;

            for (var i = 0; i < standardLodgingAds; i++) {
                scope.lodgingAds.push({ img: "../../assets/images/ads2.gif", link: "publish", external: false });
            }

            var standardServiceAds = 16 - scope.serviceAds.length;

            for (var j = 0; j < standardServiceAds; j++) {
                scope.serviceAds.push({ img: "../../assets/images/ads2.gif", link: "publish", external: false });
            }
        });

    }


    function HomeLodgingController(scope, state, Lodging, Advertising) {

        var lodgingId = state.current.data.ID;

        scope.dataHasLoaded = false;

        Lodging.get_by_id(lodgingId).then(function (data) {
            var lodging = data.ResponseData;

            scope.name = lodging.Name;
            scope.type = lodging.AccommodationType.Name;
            scope.phone = lodging.Phone;
            scope.capacity = lodging.Capacity;
            scope.email = lodging.Email;
            scope.webpage = lodging.WebPage;

            scope.location = lodging.Location.Name;

            scope.htmlToBind = lodging.Description;

            scope.images = [];

            angular.forEach(lodging.Photos, function (photo) {
                scope.images.push({ thumb: photo.URL, img: photo.URL, description: photo.Description });
            });

            scope.dataHasLoaded = true;

            Advertising.get_by_location(lodging.Location.ID).then(function (data) {
                var ad_list = data.ResponseData;

                scope.ads = [];

                angular.forEach(ad_list, function (ad) {
                    if (ad.IsAccommodation === true) {
                        scope.ads.push({ img: ad.ImageURL, link: ad.Link, external: ad.ExternalLink });
                    }
                });

                var standardAds = 16 - scope.ads.length;

                for (var i = 0; i < standardAds; i++) {
                    scope.ads.push({ img: "../../assets/images/ads2.gif", link: "publish", external: false });
                }
            });
        });
        
    }


    function HomeTourismController(scope, state, Tourism, Advertising) {

        var tourismId = state.current.data.ID;

        scope.dataHasLoaded = false;

        Tourism.get_by_id(tourismId).then(function (data) {
            var tourism = data.ResponseData;

            scope.name = tourism.Name;
            scope.type = tourism.TourismType.Name;
            scope.phone = tourism.Phone;
            scope.email = tourism.Email;
            scope.webpage = tourism.WebPage;

            scope.location = tourism.Location.Name;

            scope.htmlToBind = tourism.Description;

            scope.images = [];

            angular.forEach(tourism.Photos, function (photo) {
                scope.images.push({ thumb: photo.URL, img: photo.URL, description: photo.Description });
            });

            scope.dataHasLoaded = true;

            Advertising.get_by_location(tourism.Location.ID).then(function (data) {
                var ad_list = data.ResponseData;

                scope.ads = [];

                angular.forEach(ad_list, function (ad) {
                    scope.ads.push({ img: ad.ImageURL, link: ad.Link, external: ad.ExternalLink });
                });

                for (var i = 0; i < 16 - ad_list.length; i++) {
                    scope.ads.push({ img: "../../assets/images/ads2.gif", link: "publish", external: false });
                }
            });
        });

    }


    function HomeServiceController(scope, state, OfferedService, Advertising) {

        var serviceId = state.current.data.ID;

        scope.dataHasLoaded = false;

        OfferedService.get_by_id(serviceId).then(function (data) {
            var service = data.ResponseData;

            scope.name = service.Name;
            scope.type = service.ServiceType.Name;
            scope.phone = service.Phone;
            scope.capacity = service.Capacity;
            scope.email = service.Email;
            scope.webpage = service.WebPage;

            scope.location = service.Location.Name;

            scope.htmlToBind = service.Description;

            scope.images = [];

            angular.forEach(service.Photos, function (photo) {
                scope.images.push({ thumb: photo.URL, img: photo.URL, description: photo.Description });
            });

            scope.dataHasLoaded = true;

            Advertising.get_by_location(service.Location.ID).then(function (data) {
                var ad_list = data.ResponseData;

                scope.ads = [];

                angular.forEach(ad_list, function (ad) {
                    if (ad.IsAccommodation === false) {
                        scope.ads.push({ img: ad.ImageURL, link: ad.Link, external: ad.ExternalLink });
                    }
                });

                var standardAds = 16 - scope.ads.length;

                for (var i = 0; i < standardAds; i++) {
                    scope.ads.push({ img: "../../assets/images/ads2.gif", link: "publish", external: false });
                }
            });
        });

    }


//injectors
    Config.$inject = ['$stateProvider'];
    HomeController.$inject = ['$rootScope', '$state'];
    HomeLocationController.$inject = ['$rootScope', '$scope', '$state', 'Advertising'];
    HomeLodgingController.$inject = ['$scope', '$state', 'Lodging', 'Advertising'];
    HomeTourismController.$inject = ['$scope', '$state', 'Tourism', 'Advertising'];
    HomeServiceController.$inject = ['$scope', '$state', 'OfferedService', 'Advertising'];

    angular.module('AccommodationApp.Home', ['ui.router', 'AccommodationApp.Base'])
        .config(Config)
        .controller('HomeLodgingCtrl', HomeLodgingController)
        .controller('HomeTourismCtrl', HomeTourismController)
        .controller('HomeServiceCtrl', HomeServiceController)
        .controller('HomeLocationCtrl', HomeLocationController)
        .controller('HomeCtrl', HomeController);
}());
