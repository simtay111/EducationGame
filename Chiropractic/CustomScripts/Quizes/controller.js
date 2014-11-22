var mainModule = angular.module('mainModule', ['ngResource', 'ngRoute', "ngTouch", 'ui.utils']);

mainModule.directive('backImg', ['backImgService', function (imgService) {
    return function (scope, element, attrs) {
        return;
    };
}]);
mainModule.directive('lessonImage', ['backImgService', function (imgService) {
    return function (scope, element, attrs) {
        var url = "";
        var extension = ".jpg";
        url = "/Images/LessonImages/Lesson" + imgService.getNextSlideImage() + extension;
        element.attr({
            'src': url
        });
    };
}]);


mainModule.controller("myController", ['$scope', '$location', 'storyService', '$http', "backImgService", function ($scope, $location, storyService, $http, imgService) {
    $location.path('/storyPage');
    $scope.token = token;
    $scope.memberId = memberId;
    $scope.currentLocation = "";
    imgService.setSlideImages([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);

    $scope.$on("Next", function (e, data) {
        $scope.currentLocation = data.destination;
        $location.path(data.destination);
    });

    $scope.showBasePrizeSummary = function () {
        if ($scope.currentLocation == "/summary" || $scope.currentLocation == "/finished")
            return false;
        return true;
    };

    $scope.member = {};
    $scope.prizes = {};
    $scope.$on("LoadPrizeSummary", function () {
        $scope.member = storyService.getUser();
        if ($scope.token != 9999)
            $scope.storyName = storyService.getStory().name;
        $scope.currentPoints = Math.ceil($scope.member.totalPoints + storyService.getPoints());
    });
}]);

mainModule.config(function ($routeProvider) {
    $routeProvider.when("/storyPage", {
        templateUrl: "/templates/quizes/story_start.html",
        controller: "storyCtrl"
    }).when("/slide/:slideId", {
        templateUrl: "/templates/quizes/slide.html",
        controller: "slideCtrl"
    }).when("/question/:questionId", {
        templateUrl: "/templates/quizes/question.html",
        controller: "questionCtrl"
    }).when("/summary", {
        templateUrl: "/templates/quizes/summary.html",
        controller: "summaryCtrl"
    }).when("/browsePrizes", {
        templateUrl: "/templates/quizes/browse_prizes_panel.html",
    }).when("/error", {
        templateUrl: "/templates/quizes/error.html"
    }).when("/finished", {
        templateUrl: "/templates/quizes/finished.html",
        controller: "finishedCtrl"
    }).otherwise({
        template: '<h3>Oops, something went wrong. <a href="/">Press here to start over</a></h3>'
    });
});

