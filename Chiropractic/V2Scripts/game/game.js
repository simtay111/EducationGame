angular.module("Games", ['ui.router', 'ngResource'])
    .controller("gameStartController", ['$state', '$scope', '$stateParams', '$http', function ($state, $scope, params, $http) {
        $scope.story = {};
        $http.get("/stories/getStorySummary?gameId=" + params.gameId).success(function (story) {
            $scope.story = story;
        });

        $scope.start = function () {
            $http.post("/stories/startGame", { gameId: params.gameId }).success(function() {
                $state.go("game.step");
            });
        }
    }
    ]).controller("gameStepController", ['$state', '$scope', '$stateParams', '$http', function ($state, $scope, params, $http) {
        $scope.step = {}
        var getNextStep = function() {
        $http.get("/stories/getNextSlide?gameId=" + params.gameId).success(function (response) {
            if (response.noNextSlide)
                $state.go("game.finished");
            $scope.step = response;
        });
        };

        getNextStep();
        $scope.finishStep = function(stepId) {
            $http.post("/stories/finishStep", { stepId: stepId}).success(function(response) {
                getNextStep();
            });
        };

        $scope.answerWith = function (value) {
            if (value == $scope.step.answer) {
                $scope.step.wasAnswered = true;
                $scope.step.wasCorrect = true;
            }
            else {
                $scope.step.wasAnswered = true;
            }
            $http.post("/stories/answerQuestion", {
                stepId: $scope.step.stepId,
                answer: value
            }).success(function() { $scope.showNextButton = true; });
        };
    }
    ]).controller("gameFinishedController", ['$scope', '$http','$stateParams', function($scope, $http, params) {
    $http.post("/stories/finishGame", { gameId: params.gameId }).success(function(response) {
        $scope.summary = response;
    });
}]);