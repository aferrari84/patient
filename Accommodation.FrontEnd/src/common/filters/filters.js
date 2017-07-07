(function () {
    "use strict";
    /**
     * function to detect browser and version of it
     * @returns {{get: 'get'}}
     * @constructor
     */
    angular.module('AccommodationApp.Filters', [])
        .filter('asList', function () {
            return function (collection, property) {
                return collection.map(function (elem) { return elem[property]; }).join(", ");
            };
        });

}());