mainModule.controller("finishedCtrl", ['$scope',  'storyService', function ($scope, storyService) {
    $scope.acctInfo = storyService.getAcctInfo();
    $scope.bkgImageUrl = "/Files/AcctLogos/" + $scope.acctInfo.id + ".jpg" + "?" + new Date().getTime();;

}]);
