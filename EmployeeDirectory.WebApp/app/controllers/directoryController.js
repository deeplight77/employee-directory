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

    $scope.myProfile = {};

    $scope.myUserImageS = "http://www.gurucul.com/wp-content/uploads/2014/02/anonymous-user.png";
    $scope.myUserImageL = "http://www.gurucul.com/wp-content/uploads/2014/02/anonymous-user.png";

    directoryService.getDirectory($scope.authentication.userName).success(function (results) {

        var myProfile = results.length > 0 ? results[0] : {};
        var url = "http://api.randomuser.me/portraits/";
        if (myProfile.gender == "female") {
            $scope.myUserImageS = url + "med/women/54.jpg";
            $scope.myUserImageL = url + "women/54.jpg"
        }
        else {
            $scope.myUserImageS = url + "med/men/54.jpg";
            $scope.myUserImageL = url + "men/54.jpg"
        }
    });

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    $scope.add = function () {
        var modalInstance = $modal.open({
            templateUrl: '/app/views/modalUser.html',
            controller: 'modalUserController',
            resolve: {
                item: function () {
                    return null;
                }
            }
        });

        modalInstance.result.then(function (userData) {

            authService.saveRegistrationData(userData).then(function () {
                
                directoryService.saveDirectoryEntry(userData).then(function (response) {

                    alert("Everything saved correctly!!");
                    $scope.directory.items.push(userData);
                },
                function (response) {
                    var errors = [];
                    for (var key in response.data.modelState) {
                        for (var i = 0; i < response.data.modelState[key].length; i++) {
                            errors.push(response.data.modelState[key][i]);
                        }
                    }
                    $scope.SavedMessage = "Error: " + errors.join(' ');
                });
            });
        });
    }

    $scope.modify = function () {
        if ($scope.selectedRow == null) {
            alert("Please select an employee from the list to update information");
            return;
        }

        var modalInstance = $modal.open({
            templateUrl: '/app/views/modalUser.html',
            controller: 'modalUserController',
            resolve: {
                item: function () {
                    return $scope.directory.items[$scope.selectedRow];
                }
            }
        });

        modalInstance.result.then(function (userData) {

            directoryService.saveDirectoryEntry(userData).then(function (response) {

                alert("Everything saved correctly!!");
            },
            function (response) {
                var errors = [];
                for (var key in response.data.modelState) {
                    for (var i = 0; i < response.data.modelState[key].length; i++) {
                        errors.push(response.data.modelState[key][i]);
                    }
                }
                $scope.SavedMessage = "Error: " + errors.join(' ');
            });
        });
    }

    $scope.modifyMe = function () {
        
        directoryService.getDirectory($scope.authentication.userName).success(function (results) {

            var myItem = results.length > 0 ? results[0] : null;

            var modalInstance = $modal.open({
                templateUrl: '/app/views/modalUser.html',
                controller: 'modalUserController',
                resolve: {
                    item: function () {
                        return myItem;
                    }
                }
            });

            modalInstance.result.then(function (userData) {

                directoryService.saveDirectoryEntry(userData).then(function (response) {

                    alert("Everything saved correctly!!");
                },
                function (response) {
                    var errors = [];
                    for (var key in response.data.modelState) {
                        for (var i = 0; i < response.data.modelState[key].length; i++) {
                            errors.push(response.data.modelState[key][i]);
                        }
                    }
                    $scope.SavedMessage = "Error: " + errors.join(' ');
                });
            });
        });
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
                $scope.directory.items.splice($scope.selectedRow, 1);
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
            angular.element(".the-list").scroll();
        }
    });

    angular.element($window).bind("resize", function () {

        if ($location.$$url == "/details") {
            $location.path("/directory");
        }
    });

}]);