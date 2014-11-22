angular.module("RegisterModule").directive("setupPrizesPanel", ['$http', function ($http) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            var getPrizes = function () {
                $http.get("/prizes/getForAccount").success(function (response) {
                    $scope.prizes = response;
                });
            };
            $scope.errors = "";
            $scope.prizes = [{}];
            $scope.pointsChanged = function (index) {
                $scope.errors = "";
            };
            $scope.savePrizes = function () {
                $scope.errors = "";
                $http.post("/prizes/update", $scope.prizes).success(function () {
                    $scope.setupPrizesForm.$setPristine();
                    $scope.$emit("nextSlide", "setupPrizesPanel");
                });
            };

            getPrizes();
        },
    };
}]);