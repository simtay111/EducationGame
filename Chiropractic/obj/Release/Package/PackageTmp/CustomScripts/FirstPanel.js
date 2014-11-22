angular.module("MainModule").directive("firstPanel", ['$location', "$http", function ($location, $http) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            $scope.pva = 24;
            $scope.netPvaIncrease = 4;
            $scope.newPatientsPerMonth = 28;
            $scope.averageCollectionsPerVisit = 60;
            $scope.goToPanel = function (destination) {
                $location.path("/" + destination);
            };
            $scope.login = function () {
                window.location = "/Login";
            };
            $scope.register = function () {
                window.location = "/Register";
            };
        },
    };
}]);