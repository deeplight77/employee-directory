'use strict';
app.controller('directoryController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.template = {
        'directoryMenu': '/app/views/appMenu.html'
    };

    $scope.$on('offCanvas::hide', function (event, data) {
        
        $scope.offCanvasOpened = false;
    });

    $scope.$on('offCanvas::show', function (event, data) {

        $scope.offCanvasOpened = true;
    });

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }
}]);