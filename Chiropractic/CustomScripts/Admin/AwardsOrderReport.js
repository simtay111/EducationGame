adminModule.controller("awardsOrderController", ['$scope', '$http', function ($scope, $http) {

    $scope.report = {};
    $scope.runReport = function () {
        $scope.loading = true;
        $http.get("/master/AwardsOrderReport").success(function (response) {
            $scope.report = response;
            $scope.loading = false;
        });
    };

}]);