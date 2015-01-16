'use strict';
app.controller('homeController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

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
    $scope.savedSuccessfully = false;
    $scope.SignUpMessage = "";

    $scope.registration = {
        userName: "",
        fullName: "",
        password: "",
        confirmPassword: "",
        email: ""
    };

    $scope.signUp = function () {

        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.SignUpMessage = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
            startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.SignUpMessage = "Failed to register user due to:" + errors.join(' ');
         });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/login');
        }, 2000);
    }

}]);