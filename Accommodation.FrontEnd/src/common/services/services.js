(function () {
    "use strict";

    function LocationService(applicationConfiguration, $requests) {
        var BASE_URL = applicationConfiguration.BASE_API_URL;
        return {
            'get_all': function () {
                var url = BASE_URL + '/locations';
                return $requests.$do(url, $requests.METHODS.GET);
            },
            'get_by_state': function (id) {
                var url = BASE_URL + '/locations-state/' + id;
                return $requests.$do(url, $requests.METHODS.GET);
            },
            'reload_cache': function () {
                var url = BASE_URL + '/locations/cache';
                return $requests.$do(url, $requests.METHODS.GET);
            }
        };

    }


    function LodgingService(applicationConfiguration, $requests) {
        var BASE_URL = applicationConfiguration.BASE_API_URL;
        return {
            'register': function (data) {
                var URL = applicationConfiguration.BASE_API_URL + "/lodgings";
                return $requests.$do(URL, $requests.METHODS.POST, data, { "Content-Type": "application/json" });
            },
            'update': function (data, id) {
                var URL = applicationConfiguration.BASE_API_URL + "/lodgings/" + id;
                return $requests.$do(URL, $requests.METHODS.PUT, data, { "Content-Type": "application/json" });
            },
            'get_by_location': function (id) {
                var url = BASE_URL + '/lodgings/' + id;
                return $requests.$do(url, $requests.METHODS.GET);
            },
            'get_by_id': function (id) {
                var url = BASE_URL + '/lodging/' + id;
                return $requests.$do(url, $requests.METHODS.GET);
            },
            'get_by_user': function () {
                var url = BASE_URL + '/lodgings-byuser';
                return $requests.$do(url, $requests.METHODS.GET);
            }
        };

    }

    function OfferedServiceService(applicationConfiguration, $requests) {
        var BASE_URL = applicationConfiguration.BASE_API_URL;
        return {
            'register': function (data) {
                var URL = applicationConfiguration.BASE_API_URL + "/services";
                return $requests.$do(URL, $requests.METHODS.POST, data, { "Content-Type": "application/json" });
            },
            'update': function (data, id) {
                var URL = applicationConfiguration.BASE_API_URL + "/services/" + id;
                return $requests.$do(URL, $requests.METHODS.PUT, data, { "Content-Type": "application/json" });
            },
            'get_by_location': function (id) {
                var url = BASE_URL + '/services-location/' + id;
                return $requests.$do(url, $requests.METHODS.GET);
            },
            'get_by_id': function (id) {
                var url = BASE_URL + '/service/' + id;
                return $requests.$do(url, $requests.METHODS.GET);
            },
            'get_by_user': function () {
                var url = BASE_URL + '/services-user';
                return $requests.$do(url, $requests.METHODS.GET);
            }
        };

    }


    function TourismService(applicationConfiguration, $requests) {
        var BASE_URL = applicationConfiguration.BASE_API_URL;
        return {
            'get_by_location': function (id) {
                var url = BASE_URL + '/tourisms/' + id;
                return $requests.$do(url, $requests.METHODS.GET);
            },
            'get_by_id': function (id) {
                var url = BASE_URL + '/tourism/' + id;
                return $requests.$do(url, $requests.METHODS.GET);
            }
        };

    }

    function HashTagService(applicationConfiguration, $requests) {
        var BASE_URL = applicationConfiguration.BASE_API_URL;
        return {
            'get_all': function () {
                var url = BASE_URL + '/hashtags';
                return $requests.$do(url, $requests.METHODS.GET);
            },
            'reload_cache': function () {
                var url = BASE_URL + '/hashtags/cache';
                return $requests.$do(url, $requests.METHODS.GET);
            }
        };

    }

    function AdvertisingService(applicationConfiguration, $requests) {
        var BASE_URL = applicationConfiguration.BASE_API_URL;
        return {
            'get_globals': function () {
                var url = BASE_URL + '/advertisings/global';
                return $requests.$do(url, $requests.METHODS.GET);
            },
            'get_by_location': function (id) {
                var url = BASE_URL + '/advertisings/' + id;
                return $requests.$do(url, $requests.METHODS.GET);
            }
        };

    }


    function UserService($rootScope,applicationConfiguration, $requests) {
        return {
            'me': function () {
                var URL = applicationConfiguration.BASE_API_URL + '/Account/UserInfo';
                return $requests.$do(URL, $requests.METHODS.GET);
            },
            'refresh': function(){
                this.me().then(function (response) {
                    $rootScope.global_user = response;
                });
            }
        };
    }


    function LoginService($rootScope, $requests, applicationConfiguration, User) {
        var URL = applicationConfiguration.BASE_API_URL + '/Token';
        return {
            'auth': function (data) {
                return $requests.$do(URL, $requests.METHODS.POST, data,
                    {"Content-Type": "application/x-www-form-urlencoded"});
            },
            'store_token': function (user_data) {
                try {
                    if (user_data == null || user_data === undefined) {
                        return false;
                    }
                    window.localStorage.token = angular.toJson(user_data);
                    $rootScope.$broadcast('$stateChangeSuccess');
                    return true;
                } catch (error) {
                    return false;
                }
            },
            'logout': function (token, expires) {
                window.localStorage.removeItem('token');
                $rootScope.is_logged_in = false;
                $rootScope.global_user = undefined;
                var URL = applicationConfiguration.BASE_API_URL + '/Account/Logout';
                return $requests.$do(URL, $requests.METHODS.POST);
            },
            'is_authenticated': function () {
                var auth_data = window.localStorage.token;
                if (auth_data) {
                    return true;
                }
                return false;
            },
            'register': function (data) {
                    var URL = applicationConfiguration.BASE_API_URL + "/Account/Register";
                    return $requests.$do(URL, $requests.METHODS.POST, data);
                }

        };
    }

    
    

    //Injectors
    var commonInjector = ['applicationConfiguration', '$requests'];
    LocationService.$inject = commonInjector;
    LodgingService.$inject = commonInjector;
    TourismService.$inject = commonInjector;
    HashTagService.$inject = commonInjector;
    OfferedServiceService.$inject = commonInjector;
    AdvertisingService.$inject = commonInjector;
    UserService.$inject = ['$rootScope','applicationConfiguration', '$requests'];
    LoginService.$inject = ['$rootScope', '$requests', 'applicationConfiguration', 'User'];
   

    angular.module('AccommodationApp.Services', ['AccommodationApp.Config', 'AccommodationApp.Helpers'])
        .factory('Location', LocationService)
        .factory('Lodging', LodgingService)
        .factory('Tourism', TourismService)
        .factory('HashTag', HashTagService)
        .factory('OfferedService', OfferedServiceService)
        .factory('Advertising', AdvertisingService)
        .factory('User', UserService)
        .factory('Login', LoginService);
}());