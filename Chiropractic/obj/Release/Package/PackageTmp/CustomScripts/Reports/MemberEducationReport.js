reportingModule.controller("memberEducationReport", ['$scope', '$http', function ($scope, $http) {
    $scope.rows = [];
    $scope.totalCount = 0;
    $http.get("/reports/memberEducationReport").success(function (response) {
        $scope.rows = _.sortBy(response.memberRow, function(row) {
            return row.quizzesCompleted * -1;
        });
        $scope.totalCount = response.totalQuizzes;
    });
    $scope.getWidth = function(row) {
        return (row.quizzesCompleted / $scope.totalCount) * 100;
    };
}]);