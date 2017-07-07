(function () {
    "use strict";
    /**
     * function to detect browser and version of it
     * @returns {{get: 'get'}}
     * @constructor
     */
    function DetectBrowser() {
        return {
            'get': function () {
                var ua = navigator.userAgent,
                    tem,
                    M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*([\d\.]+)/i) || [];
                if (/trident/i.test(M[1])) {
                    tem = /\brv[ :]+(\d+(\.\d+)?)/g.exec(ua) || [];
                    return {
                        'name': 'IE',
                        'version': parseInt(tem[1] || 0, 10)
                    };
                }
                M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
                if ((tem = ua.match(/version\/([\.\d]+)/i)) != null) {
                    M[2] = tem[1];
                }
                return {
                    'name': M[0],
                    'version': M[1]
                };
            }
        };
    }

    /**
     * Function that wraps the http behavior
     * @param $rootScope
     * @param $http
     * @param $q
     * @param $objects
     * @returns {{METHODS: {POST: string, GET: string, PUT: string, DELETE: string, JSONP: string}, $do: '$do', $buildQueryString: '$buildQueryString', $encode: '$encode', $decode: '$decode'}}
     * @constructor
     */
    function CustomRequestHandler($rootScope, $http, $q) {
        var setHeader = function () {
            delete $http.defaults.headers.common['Authorization'];
            var token = window.localStorage.token;
            if (token !== null && token !== undefined) {
                var tokenObj = angular.fromJson(token);
                $http.defaults.headers.common['Authorization'] = "Bearer " + tokenObj.access_token;
            }
        };
        return {
            METHODS: {
                POST: 'POST',
                GET: 'GET',
                PUT: 'PUT',
                DELETE: 'DELETE',
                JSONP: 'JSONP'
            },
            '$do': function (url, method, data, _headers) {
                setHeader();
                var results = $q.defer();
                // make the request
                $rootScope.$broadcast('start_request');
                $http({
                    url: url,
                    method: method,
                    data: data,
                    headers: _headers
                }).success(function (body) {
                    if (!body.IsValid) {
                        $rootScope.$broadcast('error_request', body);
                    }
                    results.resolve(body);
                    if ($http.pendingRequests.length === 0) {
                        $rootScope.$broadcast('end_request');
                    }
                }).error(function (error_data) {
                    results.reject(error_data);
                    if ($http.pendingRequests.length === 0) {
                        $rootScope.$broadcast('end_request');
                    }
                });

                return results.promise;
            },
            '$dorequest': function (url, method, data, _headers, transformRequest) {
                setHeader();
                var results = $q.defer();
                // make the request
                $rootScope.$broadcast('start_request');
                $http({
                    url: url,
                    method: method,
                    data: data,
                    headers: _headers,
                    transformRequest: transformRequest
                }).success(function (body) {
                    if (!body.IsValid) {
                        $rootScope.$broadcast('error_request', body);
                    }
                    results.resolve(body);
                    if ($http.pendingRequests.length === 0) {
                        $rootScope.$broadcast('end_request');
                    }
                }).error(function (error_data) {
                    results.reject(error_data);
                    if ($http.pendingRequests.length === 0) {
                        $rootScope.$broadcast('end_request');
                    }
                });

                return results.promise;
            },
            '$buildQueryString': function (object) {
                var query = new ObjectUtils().toUrlString(object);
                if (query !== '') {
                    query = '?' + query;
                }
                return query;
            },
            '$encode': function (e) {
                var browser = new DetectBrowser().get();
                if (browser.name === 'IE' || browser.name === 'MSIE') {
                    return unescape(encodeURIComponent(e));
                }
                return e;
            },
            '$decode': function (e) {
                var browser = new DetectBrowser().get();
                if (browser.name === 'IE' || browser.name === 'MSIE') {
                    return decodeURIComponent(escape(s));
                }
                return e;
            }
        };
    }

    /**
     * Function that add utils to javascript objects
     * @returns {{isEmpty: 'isEmpty', cleanArray: cleanArray, toUrlString: 'toUrlString'}}
     * @constructor
     */
    function ObjectUtils() {
        return {
            'isEmpty': function (obj) {
                return Object.keys(obj).length === 0;
            },
            'cleanArray': function cleanArray(actual) {
                var newArray = [];
                for (var i = 0; i < actual.length; i++) {
                    if (actual[i]) {
                        newArray.push(actual[i]);
                    }
                }
                return newArray;
            },
            'toUrlString': function (obj) {
                var url = this.cleanArray(Object.keys(obj).map(function (k) {
                    if (!angular.isUndefined(obj[k]) && obj[k] !== null && obj[k] !== "") {
                        return encodeURIComponent(k) + '=' + encodeURIComponent(obj[k]);
                    }
                })).join('&');
                return url;
            }
        };
    }

    /**
     * Function that add utils to datetime objects manipulation
     * dateTime_to_thicks: Function that converts a datetime object to thicks taking in consideration the Timezone Offset.
     * @returns {{dateTime_to_thicks: 'dateTime_to_thicks'}}
     * @constructor
     */
    function DateTimeHelper(){
        return {
            'dateTime_to_thicks': function DateToThicks(date){
                if(angular.isDate(date)){
                    return date.getTime() + (date.getTimezoneOffset() * 1000 * 60);
                }
                return 0;
            }
        };
    }

    /**
     * function to return the current access token
     * @returns {{token: 'token'}}
     * @constructor
     */
    function GetAccessToken(){
        return {
            'token': function(){
                var token = window.localStorage.token;
                if (token !== undefined && token !== null) {
                    return "Bearer " +  angular.fromJson(token).access_token;
                }else{
                    return '';
                }
            }
        };
    }
  

    /**
 * Function that add utils to datetime objects manipulation
 * dateTime_to_thicks: Function that converts a datetime object to thicks taking in consideration the Timezone Offset.
 * @returns {{dateTime_to_thicks: 'dateTime_to_thicks'}}
 * @constructor
 */
    function EnumsHelper() {
        return {
            'get_project_type_enum_value': function DateToThicks(date) {
                if (angular.isDate(date)) {
                    return date.getTime() + (date.getTimezoneOffset() * 1000 * 60);
                }
                return 0;
            }
        };
    }

    /**
     * Function that handle response 
     * @returns {{dateTime_to_thicks: 'dateTime_to_thicks'}}
     * @constructor
     */
    function ResponseObserver(q, injector){
        return {
            'responseError': function(rejection) {
                switch (rejection.status) {
                    case 401:
                        window.localStorage.clear();
                        injector.get('$state').go('login');
                        break;
                    case 403:
                        injector.get('$state').go('base.static_403');
                        break;
                    case 404:
                        injector.get('$state').go('base.static_404');                        
                }
                return q.reject(rejection);
            }
        };
    }

//injectors
    CustomRequestHandler.$inject = ['$rootScope', '$http', '$q'];
    ResponseObserver.$inject = ['$q', '$injector'];
//ng module
    angular.module('AccommodationApp.Helpers', [])
        .factory('browser_detector', DetectBrowser)
        .factory('$requests', CustomRequestHandler)
        .factory('$objects', ObjectUtils)
        .factory('$date_time_helper', DateTimeHelper)
        .factory('$access_token', GetAccessToken)
        .factory('$responseObserver', ResponseObserver);
}());
