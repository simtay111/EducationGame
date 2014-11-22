mainModule.controller("storyCtrl", ['$scope', "storyService", "$http", function ($scope, storyService, $http) {
    $scope.dataLoaded = false;
    //$scope.isOnlyViewingPrizes = true;
    $http.get("/Stories/GetStoryForMember?memberId=" + memberId + "&token=" + token).success(function (response) {

        //var response = { member: { firstName: "bilbo", totalPoints: 50 }, prizes: [{ name: "prize1", points: 100 }, { name: "prize2", points: 300 }] };
        //response.story = { name: "THeStory Name" };
        //response.slides = [{ body: "slide 1 body more content more content tent of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in" }];//, { body: "slide2 body" }, { body: "slide3Body" }];
        //response.questions = [{ query: "slide 1 query", answerBool: true }];//, { query: "slide2 query", answerBool: true }];
        if (!response.successful) {
            $scope.$emit("Next", { destination: "/error" });
            return;
        }
        storyService.loadService(response.story, response.slides, response.questions, response.member, response.prizes, response.accountInfo);
        $scope.story = storyService.getStory();
        $scope.member = storyService.getUser();
        $scope.prizes = storyService.getPrizes();
        $scope.dataLoaded = true;
        $scope.$emit("LoadPrizeSummary");
        if (token == 9999)
            _.delay(function() {$scope.$apply(function() {$scope.browsePrizes()})}, 500);
    });
    $scope.browsePrizes = function () {
        $scope.$emit("Next", { destination: "/browsePrizes" });
    };

    $scope.next = function () {
        $scope.$emit("Next", { destination: "/slide/1" });
        //$scope.$emit("Next", { destination: "/finished" });
    };
}]);
