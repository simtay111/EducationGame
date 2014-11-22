mainModule.controller("summaryCtrl", ['$scope', '$http', 'storyService', function ($scope, $http, storyService) {
    $scope.user = storyService.getUser();
    $scope.dataReady = false;
    $scope.earnedPoints = storyService.getPoints();
    $scope.acctInfo = storyService.getAcctInfo();
    $scope.story = storyService.getStory();
    $scope.prizes = [];

    var request = {
        memberId: $scope.user.id,
        token: token,
        pointsEarned: storyService.getPoints()
    };

    $http.post("/Stories/FinishQuiz", request).success(function (response) {
        $scope.dataReady = true;
    });

    $scope.currentPoints = function () {
        return Math.round($scope.user.totalPoints + $scope.earnedPoints);
    };

    $scope.allDone = function () {
        $scope.$emit("Next", { destination: "/finished" });
    };
}]);
