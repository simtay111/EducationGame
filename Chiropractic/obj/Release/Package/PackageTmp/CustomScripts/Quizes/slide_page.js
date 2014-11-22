mainModule.controller("slideCtrl", ['$scope', '$location', 'storyService', '$routeParams', function ($scope, $location, storyService, $routeParams) {
    var currentSlide = parseInt($routeParams.slideId);
    $scope.name = "Slide " + $routeParams.slideId;
    $scope.slide = storyService.getSlide(currentSlide);
    $scope.story = storyService.getStory();
    $scope.prizes = storyService.getPrizes();
    $scope.user = storyService.getUser();

    $scope.currentPercentage = function() {
        return (($routeParams.slideId) / 5) * 100;
    };

    $scope.getImageFloat = function() {
        return (new Date().getMilliseconds() % 2 == 0) ? 'right' : 'left';
    };

    $scope.next = function () {
        var totalSlides = storyService.getNumberOfSlides();
        if (currentSlide >= totalSlides)
            $scope.$emit("Next", { destination: "/question/1" });
        else {
            $scope.$emit("Next", { destination: "/slide/" + (currentSlide + 1) });
        }
    };
}]);
