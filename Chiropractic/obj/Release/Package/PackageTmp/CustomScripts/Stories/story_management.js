mainModule.controller("storyManagement", ['$scope', '$http', function ($scope, $http) {
    $scope.stories = [];
    $scope.slides = [];
    $scope.questions = [];
    $scope.storyBeingEdited = {};
    $scope.isEditing = false;
    $scope.orderSaved = true;
    var pristineStories = [];

    var setStoriesOnScope = function (stories) {
        $scope.stories = _.sortBy(stories, function (item) {
            return item.storyOrder;
        });
    };

    var loadAllStories = function () {
        $http.get("/stories/getAllStories").success(function (response) {
            setStoriesOnScope(response);
            pristineStories = angular.copy(response);
        });
    };

    loadAllStories();

    $scope.editStory = function (story) {
        $scope.isEditing = true;
        $scope.storyBeingEdited = story;
        $scope.editStoryForm.$setPristine();
        $scope.loading = 0;
        $http.get("/stories/getSlides?storyId=" + story.id).success(function (response) {
            $scope.slides = response;
            $scope.loading++;
        });
        $http.get("/stories/getQuestions?storyId=" + story.id).success(function (response) {
            $scope.questions = response;
            $scope.loading++;
        });
    };

    $scope.cancelEdit = function () {
        $scope.isEditing = false;
        setStoriesOnScope(pristineStories);
    };
    $scope.saveEdit = function () {
        if ($scope.editStoryForm.$invalid) {
            $scope.errors = "Please fix the form to continue.  Make sure all fields are filled out!";
            return;
        }

        $scope.loading = 0;
        $http.post("/stories/updateEverything", { slides: $scope.slides, story: $scope.storyBeingEdited, questions: $scope.questions })
            .success(function (response) {
                $scope.loading = 2;
                loadAllStories();
                $scope.isEditing = false;
            });
    };

    $scope.addNew = function () {
        $scope.storyBeingEdited = {};
        $scope.slides = [{}, {}, {}, {}];
        $scope.questions = [{}, {}, {}];
        $scope.isEditing = true;
        $scope.loading = 2;
    };

    var saveOrder = _.debounce(function () {
        $http.post("/stories/updateOrder", { stories: $scope.stories }).success(function (response) {
            $scope.orderSaved = true;
        });
    }, 2000);

    $scope.moveUp = function (current) {
        $scope.orderSaved = false;
        var existing = _.find($scope.stories, function (item) {
            return item.storyOrder == (current.storyOrder - 1);
        });
        existing.storyOrder++;
        current.storyOrder--;
        setStoriesOnScope($scope.stories);
        saveOrder();
    };
    $scope.moveDown = function (current) {
        $scope.orderSaved = false;
        var existing = _.find($scope.stories, function (item) {
            return item.storyOrder == (current.storyOrder + 1);
        });
        current.storyOrder++;
        existing.storyOrder--;
        setStoriesOnScope($scope.stories);
        saveOrder();
    };
}]);
