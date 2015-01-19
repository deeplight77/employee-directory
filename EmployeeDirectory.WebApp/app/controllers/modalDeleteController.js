'use strict';
app.controller('modalDeleteController', function ($scope, $modalInstance, item) {

    $scope.item = item;

    $scope.ok = function () {
        $modalInstance.close($scope.item);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});