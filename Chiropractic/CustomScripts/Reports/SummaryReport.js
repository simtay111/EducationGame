reportingModule.controller("summaryReport", ['$scope','$http', function ($scope, $http) {
    $scope.summaries = [];
    $http.get("/Accounts/GetAccountInfoSummary").success(function (response) {
        $scope.accountInfo = response.acctInfo;
        $scope.summaries = response.summaries;
    });
}]);