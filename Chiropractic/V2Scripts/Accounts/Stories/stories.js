angular.module("Stories", ['ui.router', 'ngResource']).controller("storiesViewController", [
        '$state', '$scope', '$stateParams', '$http', function ($state, $scope, params, $http) {
            $scope.stories = [];
            $http.get("/stories/getstoriesForAccount").success(function (response) {
                $scope.stories = response;
            });
        }
]).controller("storiesEditController", [
        '$state', '$scope', '$stateParams', '$http', function ($state, $scope, params, $http) {
            $scope.story = {};
            $scope.slides = {};
            $scope.questions = {};
            var getEntireStory = function () {
                $http.get("/stories/getEntireStory?storyId=" + params.storyId).success(function (response) {
                    $scope.story = response.story;
                    $scope.slides = response.slides;
                    $scope.questions = response.questions;
                });
            }
            getEntireStory();
            $scope.$on("StoryUpdated", function () {
                getEntireStory();
            });

            $scope.editSlide = function (slideId) {
                $state.go("stories.edit.slide", { slideId: slideId });
            }
            $scope.editQuestion = function (questionId) {
                $state.go("stories.edit.question", { questionId: questionId });
            }
            $scope.deleteSlide = function (slideId) {
                var confirmed = confirm("Are you sure you wish to delete this slide?");
                if (confirmed) {
                    $http.delete("/api/slides/" + slideId).success(function () {
                        $state.go("stories.edit");
                        getEntireStory();
                    });
                }
            };
            $scope.deleteQuestion = function (questionId) {
                var confirmed = confirm("Are you sure you wish to delete this question?");
                if (confirmed) {
                    $http.delete("/api/questions/" + questionId).success(function () {
                        $state.go("stories.edit");
                        getEntireStory();
                    });
                }
            };
        }
])
    .controller("questionEditController", [
        '$scope', '$stateParams', '$http', '$state', function ($scope, params, $http, $state) {
            $scope.question = {};
            if (params.questionId == -1) {
                $scope.question = { id: -1 };
            } else {
                $http.get("/api/questions/" + params.questionId).success(function (response) {
                    $scope.question = response;
                });
            }

            $scope.save = function () {
                $http.post("/api/questions/" + params.storyId, $scope.question).success(function (response) {
                    $scope.$emit("StoryUpdated");
                    if (params.questionId == -1)
                        $state.go("stories.edit");
                });
            }
        }
    ])
    .controller("slideEditController", [
        '$scope', '$stateParams', '$http', '$state', function ($scope, params, $http, $state) {
            $scope.slide = { id: -1 };
            if (params.slideId > 0)
                $http.get("/api/slides/" + params.slideId).success(function (response) {
                    $scope.slide = response;
                });

            $scope.save = function () {
                $http.post("/api/slides/" + params.storyId, $scope.slide).success(function () {
                    $scope.$emit("StoryUpdated");
                    if (params.slideId == -1) {
                        $state.go("stories.edit");
                    }
                });

            }
        }
    ]);