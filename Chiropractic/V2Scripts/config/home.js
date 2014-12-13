angular.module("EducationGame", ['ui.router', 'Games', 'ngResource']).controller("homeController", ['$state', '$scope', '$stateParams','$http', function ($state, $scope, params, $http) {
    $scope.stories = [];

    $http.get("/stories/getPublicStories").success(function(response) {
        $scope.stories = response;
    });
    $scope.start = function (story) {
        $state.go('game.start', { gameId: story.id });
    };
}]).config(["$stateProvider",'$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/home');

    $stateProvider.state("homeController", {
        url: '/home',
        templateUrl: '/V2Templates/game/game_base.html',
        controller: 'homeController'
    }).state("game", {
        url: '/game/:gameId',
        abstract: true,
        template: '<ui-view/>'
    }).state("game.start", {
        url: '/start',
        templateUrl: '/V2Templates/game/game_start.html',
        controller: "gameStartController"
    }).state("game.step", {
        url: '/step',
        templateUrl: '/V2Templates/game/game_step.html',
        controller: "gameStepController"
    });
}]);