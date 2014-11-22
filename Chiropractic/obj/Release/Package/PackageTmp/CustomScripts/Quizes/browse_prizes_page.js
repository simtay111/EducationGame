mainModule.directive("browsePrizesPage", ['storyService', '$http', function (storyService, $http) {
    return {
        scope: {
            currentPoints: '='
        },
        link: function ($scope, elem, attr) {
            $scope.acctInfo = storyService.getAcctInfo();
            $scope.email = "";
            $scope.email2 = "";
            $scope.ordering = false;
            $scope.allPrizes = [];
            var acctInfo = storyService.getAcctInfo().id;
            $scope.acctLogo = "/Files/AcctLogos/" + acctInfo + ".jpg" + "?" + new Date().getTime();;

            $http.get("/Prizes/GetForAccountWithId?acctId=" + acctInfo).success(function (response) {
                $scope.allPrizes = response.publicPrizes;
                _.each($scope.allPrizes, function (item) {
                    item.preferred = false;
                });
                _.each(response.prizes, function (item) {
                    item.preferred = true;
                    $scope.allPrizes.push(item);
                });
                $scope.allPrizes = _.sortBy($scope.allPrizes, function (item) {
                    return item.points;
                });
            });

            $scope.orderPrize = function () {
                $scope.ordering = true;
                $http.post("/Prizes/OrderPrize", { email: $scope.email, memberId: memberId, prizeId: $scope.selectedPrize.id, preferred: $scope.selectedPrize.preferred }).success(function (response) {
                    if (response.error) {
                        alert(response.error);
                        return;
                    }
                    alert("Your reward was ordered! Check your email for your E Gift Card or the front desk for rewards they are providing!");
                    $("#ExaminePrizeDialog").modal("hide");
                    setTimeout(function () {
                        $scope.$apply(function () {
                            $scope.$emit("Next", { destination: "/" + attr.browsePrizesPage });
                        });
                    }, 500);
                }).error(function () {
                    alert("Something went wrong in the order process! We're on it!");
                    $scope.ordering = false;
                });
            };

            $scope.emailsMatch = function () {
                return $scope.email == $scope.email2;
            };

            $scope.selectItem = function (item) {
                $scope.selectedPrize = item;
                $scope.email = "";
                $scope.email2 = "";
                $http.get("/Prizes/CanRedeem?memberId=" + memberId + "&prizeId=" + $scope.selectedPrize.id + "&preferred=" + $scope.selectedPrize.preferred || false).success(function (response) {
                    $scope.serverCurrentPoints = response.currentPoints;
                    $scope.canRedeem = response.canRedeem;
                    $scope.disclaimer = response.disclaimer;
                    $("#ExaminePrizeDialog").modal("show");
                });
            };
        },
        templateUrl: "/templates/quizes/browse_prizes.html"
    };
}]);
