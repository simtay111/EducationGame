mainModule.controller("questionCtrl", ['$scope', 'storyService', '$routeParams', function ($scope, storyService, $routeParams) {
    $scope.currentQuestionId = parseInt($routeParams.questionId);
    $scope.question = storyService.getQuestion($scope.currentQuestionId);
    $scope.user = storyService.getUser();
    $("#QuestionSlide").css({ 'min-height': window.screen.height + "px"});
    $scope.earnedPoints = storyService.getPoints();
    $scope.prizes = storyService.getPrizes();
    $scope.answerWith = function (value) {
            TweenMax.fromTo($('.expand'), 3.0, {css: {'max-height' : 0}},{ css: {
                 'max-height': 1000
            } });
        if (value == $scope.question.answerBool) {
            $scope.question.wasAnswered = true;
            $scope.question.wasCorrect = true;
            storyService.addPointsToScore(33.33);
            $scope.earnedPoints = storyService.getPoints();
            $scope.$emit("LoadPrizeSummary");
        }
        else {
            $scope.question.wasAnswered = true;
        }
    };

    $scope.currentPoints = function () {
        return Math.round($scope.user.totalPoints + $scope.earnedPoints);
    };

    $scope.next = function () {
        var totalQuestions = storyService.getNumberOfQuestions();
        if ($scope.currentQuestionId >= totalQuestions)
            $scope.$emit("Next", { destination: "/summary" });
        else {
            $scope.$emit("Next", { destination: "/question/" + ($scope.currentQuestionId + 1) });
        }
    };
}]);
