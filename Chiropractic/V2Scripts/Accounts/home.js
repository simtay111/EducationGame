angular.module("AccountsEducationGame", ['ui.router', 'ngResource', 'Stories']).controller("homeController", ['$state', '$scope', '$stateParams','$http', function ($state, $scope, params, $http) {
    $scope.stories = [];
}]).config(["$stateProvider",'$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/home');
    $stateProvider.state("home", {
        url: '/home',
        templateUrl: '/V2Templates/Accounts/home.html',
        controller: "homeController"
    }).state("stories", {
        url: '/stories',
        templateUrl: '/V2Templates/Accounts/Stories/stories_view.html',
        controller: "storiesViewController"
    }).state("stories.edit", {
        url: '/edit/:storyId',
        templateUrl: '/V2Templates/Accounts/Stories/stories_edit.html',
        controller: "storiesEditController"
    })
    .state("stories.edit.slide", {
        url: '/slide/:slideId',
        templateUrl: '/V2Templates/Accounts/Stories/slide_edit.html',
        controller: "slideEditController"
    }).state("stories.edit.question", {
        url: '/question/:questionId',
        templateUrl: '/V2Templates/Accounts/Stories/question_edit.html',
        controller: "questionEditController"
    });
}]);