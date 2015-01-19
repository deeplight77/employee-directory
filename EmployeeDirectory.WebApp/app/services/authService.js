'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', function ($http, $q, localStorageService) {

    var serviceBase = 'http://localhost:22367/';
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        roles: []
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {

            _login(registration);
            return response;
        });
    };

    var _deleteRegistration = function (registration) {

        return $http.post(serviceBase + 'api/account/remove', registration).then(function (response) {

            return response;
        });
    };

    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, roles: [] });

            _getRoles(loginData.userName).then(function (results) {

                localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, roles: results.data })

                _authentication.isAuth = true;
                _authentication.userName = loginData.userName;
                _authentication.roles = results.data;

                deferred.resolve(response);
            },
            function (error) {
                _logOut();
                deferred.reject(err);
            });

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _getRoles = function (userName) {

        return $http.get(serviceBase + 'api/account/getroles?userName=' + userName).then(function (results) {
            return results;
        });
    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');
        localStorageService.remove('selectedItem');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.roles = [];
    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.roles = authData.roles;
        }
    }

    authServiceFactory.getRoles = _getRoles;
    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.deleteRegistration = _deleteRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;

    return authServiceFactory;
}]);