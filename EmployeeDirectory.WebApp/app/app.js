var app = angular.module("EmployeeDirectoryApp", ["ngRoute", "LocalStorageModule", "mm.foundation"]);

app.config(["$routeProvider", "$httpProvider", function ($routeProvider, $httpProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    })
    .when("/directory", {
        controller: "ordersController",
        templateUrl: "/app/views/directory.html"
    })
    .otherwise({ redirectTo: "/home" });

    $httpProvider.interceptors.push("authInterceptorService");
}]);

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);
