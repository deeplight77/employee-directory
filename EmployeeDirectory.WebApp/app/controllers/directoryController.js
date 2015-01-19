'use strict';
app.controller('directoryController', ['$scope', '$window', '$modal', '$location', 'authService', 'directoryService', 'localStorageService',
    function ($scope, $window, $modal, $location, authService, directoryService, localStorageService) {

    $scope.selectedRow = null;

    $scope.directory = new directoryService.directory();

    $scope.authentication = authService.authentication;

    $scope.template = {
        'header': '/app/views/header.html',
        'directoryMenu': '/app/views/appMenu.html',
        'details': '/app/views/details.html'
    };

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    $scope.delete = function () {
        if ($scope.selectedRow == null) {
            alert("Please select an employee from the list to allow deletion");
            return;
        }

        var modalInstance = $modal.open({
            templateUrl: '/app/views/modalDelete.html',
            controller: 'modalDeleteController',
            resolve: {
                item: function () {
                    return $scope.directory.items[$scope.selectedRow];
                }
            }
        });

        modalInstance.result.then(function (item) {

            authService.deleteRegistration(item).then(function () {
                alert("User deleted successfully");
                delete $scope.directory.items[$scope.selectedRow];
            });
        });
    }

    $scope.showDetailsSwitch = "none";

    $scope.showDetails = function (index) {
        var item = $scope.directory.items[index];
        $scope.selectedItem = item;
        $scope.showDetailsSwitch = "selected";
        $scope.selectedRow = index;

        localStorageService.set('selectedItem', { selectedItem: $scope.selectedItem, showDetailsSwitch: $scope.showDetailsSwitch, selectedRow: $scope.selectedRow });

        if (!angular.element(".directory-details").is(":visible")) {
            $location.path('/details');
        }
    }

    $scope.$on("$routeChangeSuccess", function ($currentRoute, $previousRoute) {

        var itemData = localStorageService.get('selectedItem');
        if (itemData) {
            $scope.selectedItem = itemData.selectedItem;
            $scope.showDetailsSwitch = itemData.showDetailsSwitch;
            $scope.selectedRow = itemData.selectedRow;
        }
    });

    $scope.searchStr = "";

    $scope.$watch('searchStr', function (tmpStr) {
        // if searchStr is still the same..
        // go ahead and retrieve the data
        if (tmpStr === $scope.searchStr) {

            $scope.directory.items = [];
            $scope.directory.busy = false;
            $scope.directory.from = 0;
            $scope.directory.loaderShow = true;

            $scope.directory.nextPage($scope.searchStr);
        }
    });

    angular.element($window).bind("resize", function () {

        if ($location.$$url == "/details") {
            $location.path("/directory");
        }
    });

}]);