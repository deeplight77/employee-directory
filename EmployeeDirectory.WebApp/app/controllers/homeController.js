'use strict';
app.controller('homeController', ['$scope', '$location', 'authService', 'directoryService', '$timeout', function ($scope, $location, authService, directoryService, $timeout) {

    /*Login*/
    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.LoginMessage = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            $location.path('/directory');

        },
         function (err) {
             $scope.LoginMessage = err.error_description;
         });
    };

    /*Sign Up*/
    $scope.registrationScreen = "first";
    $scope.signedUpSuccessfully = false;
    $scope.savedSuccessfully = false;
    $scope.SignUpMessage = "";
    $scope.SavedMessage = "";

    $scope.registration = {
        userName: "",
        password: "",
        confirmPassword: "",
        roleName: "",
        fullName: "",
        officeLocation: "",
        officePhoneNumber: "",
        personalPhoneNumber: "",
        emailAddress: "",
        gender: ""
    };

    $scope.continueRegistration = function () {
        $scope.registrationScreen = "second"
    }

    $scope.signUp = function () {

        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.signedUpSuccessfully = true;
            $scope.SignUpMessage = "Registration successful!! Would you like to continue filling out your personal information?";
        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.SignUpMessage = "Error: " + errors.join(' ');
         });
    };

    $scope.saveUserData = function () {
        
        directoryService.saveDirectoryEntry($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.SavedMessage = "All your data was saved correctly, you will be redirected in 2 seconds";
            startTimer();
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
    }

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/directory');
        }, 2000);
    }

}]);