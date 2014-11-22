mainModule.controller("storySummary", ['$scope', '$http', function ($scope, $http) {
    $scope.stories = [];

    $scope.runReport = function () {
        $http.get("/Stories/GetStoryDump").success(function (response) {
            $scope.stories = response;
        });
    };

    var getBlackListSummary = function () {
        $http.get("/Stories/GetBlackListSummary").success(function (response) {
            $scope.blackListSummary = response;
        });
    };

    $scope.toggleBlackListed = function (story) {
        $http.post("/Stories/ToggleBlackList", { storyId: story.story.id, isBlackListed: story.isBlackListed });
    };
    getBlackListSummary();
}]);
