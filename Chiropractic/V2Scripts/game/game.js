angular.module("Games", ['ui.router', 'ngResource'])
    .controller("gameStartController", ['$state', '$scope', '$stateParams', '$http', function ($state, $scope, params, $http) {
        $scope.story = {};
        $http.get("/stories/getStorySummary?gameId=" +  params.gameId).success(function (story) {
            $scope.story = story;
        });

    }
    ]).controller("gameStepController", ['$state', '$scope', '$stateParams', '$http', function ($state, $scope, params, $http) {
    }
    ]);