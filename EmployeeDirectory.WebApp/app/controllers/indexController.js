'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    

    $scope.authentication = authService.authentication;

}]);