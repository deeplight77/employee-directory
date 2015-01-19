'use strict';
app.controller('modalUserController', function ($scope, $modalInstance, item) {

    $scope.userData = item || {
        userName: "",
        password: "",
        confirmPassword: "",
        roleName: "",
        fullName: "",
        officeLocation: "",
        officePhoneNumber: "",
        personalPhoneNumber: "",
        emailAddress: ""
    };

    $scope.ok = function () {

        if ($scope.userData.userName == "") {
            var uname = $scope.userData.fullName.replace(/\s+/g, '').toLowerCase();
            $scope.userData.userName = uname;
            $scope.userData.password = uname;
            $scope.userData.confirmPassword = uname;
        }

        $modalInstance.close($scope.userData);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});