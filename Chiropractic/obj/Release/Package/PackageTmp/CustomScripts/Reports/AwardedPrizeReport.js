reportingModule.controller("awardedPrizesReport", ['$scope', '$http', function ($scope, $http) {
    $scope.report = {};
    var currentDate = new Date();
    $scope.start = moment().format("YYYY-MM-DD");
    $scope.end = moment().format("YYYY-MM-DD");
    $scope.runReport = function () {
        $scope.loading = true;
       
        $http.get("/reports/AwardedPrizeReport?start=" + $scope.start + "&end=" + $scope.end).success(function (response) {
            $scope.report = response;
            $scope.loading = false;
        }).error(function(response) {
            $scope.loading = false;
        });
    };
}]);