angular.module("Game", ['ui.router']).controller("gameBase", ['$state', '$scope', '$stateParams', function ($state, $scope, params) {
    $scope.start = function () {
        $state.go('game.slide', { slideId: 1 });
    };
}]).controller("slide", ['$state', '$scope', '$stateParams', function ($state, $scope, params) {
    $scope.slideId = parseInt(params.slideId);
    $scope.next = function () {
        if ($scope.slideId < 4)
            $state.go('game.slide', { slideId: $scope.slideId + 1 });
        else {
            $state.go('game.question', { questionId: 1 });
        }
    };
}]).controller("question", ['$state', '$scope', '$stateParams', function ($state, $scope, params) {
    $scope.questionId = parseInt(params.questionId);
    $scope.next = function () {
        if ($scope.questionId < 3)
            $state.go('game.question', { questionId: $scope.questionId + 1 });
        else {
            $state.go('game.finished');
        }
    };
}]).controller("finished", ['$state', '$scope', '$stateParams', function ($state, $scope, params) {
    $scope.browseRewards = function () {
        $state.go("rewards.browse");
    };
}]).config(["$stateProvider", function ($stateProvider) {
    $stateProvider.state("game", {
        url: '/game/:id',
        templateUrl: '/V2Templates/game/game_base.html',
        controller: 'gameBase'
    }).state("game.slide", {
        url: "/slide/:slideId",
        controller: "slide",
        template: "<h1>Slide: {{slideId}}</h1><button ng-click='next()'>Next</button>"
    }).state("game.question", {
        url: "/question/:questionId",
        controller: "question",
        template: "<h1>Question: {{questionId}}</h1><button ng-click='next()'>Next</button>"
    }).state("game.finished", {
        url: "/finished",
        controller: "finished",
        template: "<h1>All Done</h1><button ng-click='browseRewards()'>Rewards</button>"
    });
}]);