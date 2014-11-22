adminModule.controller("setNewAcctIdController", ['$scope', '$http', function ($scope, $http) {
    $scope.newId = 1;
    $scope.setIt = function () {
        $http.post("/master/updateAcctId", { acctId: $scope.newId });
    };
}]);