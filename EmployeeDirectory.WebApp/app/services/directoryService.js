'use strict';
app.factory('directoryService', ['$http', function ($http) {

    var serviceBase = 'http://localhost:22367/';
    var directoryServiceFactory = {};

    var _getDirectory = function () {

        return $http.get(serviceBase + 'api/directory').then(function (results) {
            return results;
        });
    };

    directoryServiceFactory.getDirectory = _getDirectory;

    return directoryServiceFactory;

}]);