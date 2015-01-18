'use strict';
app.controller('directoryController', ['$scope', '$location', 'authService', 'directoryService', '$modal', function ($scope, $location, authService, directoryService, $modal) {

    $scope.selectedRow = null;

    $scope.directory = new directoryService.directory();

    $scope.authentication = authService.authentication;

    $scope.template = {
        'directoryMenu': '/app/views/appMenu.html'
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
        $scope.showDetailsSwitch = "selected";
        $scope.selectedRow = index;
    }

    $scope.searchStr = "";

    $scope.$watch('searchStr', function (tmpStr) {
        if (!tmpStr || tmpStr.length == 0)
            return 0;
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

}]);