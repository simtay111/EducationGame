﻿reportingModule.controller("receiptReport", ['$scope','$http', function ($scope, $http) {
    $scope.report = {};
    $scope.start = moment().format("YYYY-MM-DD");
    $scope.end = moment().format("YYYY-MM-DD");
    $scope.runReport = function () {
        $scope.loading = true;
        $http.get("/reports/ReceiptReport?start=" + $scope.start + "&end=" + $scope.end).success(function (response) {
            $scope.report = response;
            $scope.loading = false;
        });
    };
}]);