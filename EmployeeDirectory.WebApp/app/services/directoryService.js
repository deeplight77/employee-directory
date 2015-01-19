'use strict';
app.factory('directoryService', ['$http', function ($http) {

    var serviceBase = 'http://localhost:22367/';

    var directoryServiceFactory = {};

    var directoryService = function () {
        this.items = [];
        this.busy = false;
        this.from = 0;
        this.loaderShow = true;
    };

    var _saveDirectoryEntry = function (entryData) {

        return $http.put(serviceBase + 'api/directory/' + entryData.userName, entryData).then(function (response) {
            return response;
        });
    }

    var _getDirectory = function (userName) {

        return $http.get(serviceBase + "api/directory?from=" + 0 + "&howMany=" + 1 + "&filter=" + userName);
    };

    var _nextPage = function (filter) {
        if (this.busy) return;
        this.busy = true;

        var url = serviceBase + "api/directory?from=" + this.from + "&howMany=" + 5 + "&filter=" + filter;

        $http.get(url).success(function (data) {

            if (data.length == 0) {
                this.loaderShow = false;
                this.busy = true;
                return;
            }

            for (var i = 0; i < data.length; i++) {
                this.items.push(data[i]);
            }
            this.from = this.items.length;
            this.busy = false;
        }.bind(this));
    };

    directoryService.prototype.nextPage = _nextPage;

    directoryServiceFactory.directory          = directoryService;
    directoryServiceFactory.getDirectory       = _getDirectory;
    directoryServiceFactory.saveDirectoryEntry = _saveDirectoryEntry;

    return directoryServiceFactory;
}]);