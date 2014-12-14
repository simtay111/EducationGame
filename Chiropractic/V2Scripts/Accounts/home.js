angular.module("AccountsEducationGame", ['ui.router', 'ngResource', 'Stories']).controller("homeController", ['$state', '$scope', '$stateParams','$http', function ($state, $scope, params, $http) {
    $scope.stories = [];
}]).config(["$stateProvider",'$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/home');
    $stateProvider.state("home", {
        url: '/home',
        templateUrl: '/V2Templates/Accounts/home.html',
        controller: 'homeController'
    }).state("stories", {
        url: '/stories',
        templateUrl: '/V2Templates/Accounts/Stories/stories_view.html',
        controller: 'storiesViewController'
    });
}]);