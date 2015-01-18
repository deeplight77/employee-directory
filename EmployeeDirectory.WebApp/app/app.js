var app = angular.module("EmployeeDirectoryApp", ["ngRoute", "LocalStorageModule", "mm.foundation", "infinite-scroll"]);

app.config(["$routeProvider", "$httpProvider", function ($routeProvider, $httpProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    })
    .when("/directory", {
        controller: "directoryController",
        templateUrl: "/app/views/directory.html"
    })
    .otherwise({ redirectTo: "/home" });

    $httpProvider.interceptors.push("authInterceptorService");
}]);

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);


app.filter('phoneNumber', function () {
    return function (phoneNumber) {
        if (!phoneNumber) { return ''; }

        var value = phoneNumber.toString().trim().replace(/^\+/, '');

        if (value.match(/[^0-9]/)) {
            return phoneNumber;
        }

        var country, city, number;

        switch (value.length) {
            case 10: // +1PPP####### -> C (PPP) ###-####
                country = 1;
                city = value.slice(0, 3);
                number = value.slice(3);
                break;

            case 11: // +CPPP####### -> CCC (PP) ###-####
                country = value[0];
                city = value.slice(1, 4);
                number = value.slice(4);
                break;

            case 12: // +CCCPP####### -> CCC (PP) ###-####
                country = value.slice(0, 3);
                city = value.slice(3, 5);
                number = value.slice(5);
                break;

            default:
                return phoneNumber;
        }

        if (country == 1) {
            country = "";
        }

        number = number.slice(0, 3) + '-' + number.slice(3);

        return (country + " (" + city + ") " + number).trim();
    };
});