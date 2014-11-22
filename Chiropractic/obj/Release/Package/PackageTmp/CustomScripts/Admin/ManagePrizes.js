adminModule.controller("managePrizesController", ['$scope', '$http', function ($scope, $http) {
    var getRewards = function () {
        $scope.loading = true;
        $http.get("/master/getcurrentawards").success(function (response) {
            $scope.currentAwards = response;
        });
        $http.get("/master/getAwardChoices").success(function (response) {
            $scope.availableChoices = response;
            $scope.loading = false;
        });
    };

    $scope.addToAvailable = function (item, reward) {
        $http.post("/master/addAward", { item: item, reward: reward }).success(function () {
            getRewards();
        });
    };

    $scope.removeAward = function (reward) {
        $http.post("/master/removeAward", reward).success(function () {
            getRewards();
        });
    };

    getRewards();
}]);