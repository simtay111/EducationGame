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
            $scope.step = response;
        });
        };

        getNextStep();
        $scope.finishStep = function() {
            $http.post("/stories/finishStep", { stepId: $scope.step.stepId }).success(function(response) {
                getNextStep();
            });
        };
    }
    ]);