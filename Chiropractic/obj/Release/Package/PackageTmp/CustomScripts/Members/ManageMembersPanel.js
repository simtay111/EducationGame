angular.module("MainModule").directive("manageMembersPanel", ['$location', '$http', function ($location, $http) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            $scope.goToPanel = function (destination) {
                $location.path("/" + destination);
            };
            $scope.$on("memberAdded", function () {
                $scope.$broadcast("outOfDate");
            });
            $scope.addNewMember = function () {
                $('#AddNewMemberDialog').modal('show');
            };
        },
    };
}]);
angular.module("MainModule").controller("curentMemberListController", ["$scope", "$http", function ($scope, $http) {
    $scope.fullMemberList = [];
    var loadMembers = function () {
        $http.get("/Members/GetMembers?includeInactive=" + $scope.includeInactive + "&buster=" + new Date().getTime()).success(function (response) {
            $scope.fullMemberList = _.sortBy(response, function (item) { return item.lastName.toUpperCase() + item.firstName.toUpperCase(); });
            $scope.users = angular.copy($scope.fullMemberList);
        });
    };
    $scope.memberQuizHistory = [];
    $scope.accountIsDone = true;
    $scope.includeInactive = false;
    $scope.inactiveChanged = function () {
        loadMembers();
    };
    $scope.reactivateMember = function () {
        $http.post("/Members/MarkActive", $scope.currentMember).success(function (response) {
            loadMembers();
        });
    };

    $scope.beginEditing = function (member) {
        $scope.currentMember = member;
        $('#EditMemberDialog').modal("show");

    };
    $scope.viewHistory = function (member) {
        $('#MemberHistoryDialog').modal('show');
        $http.get("/Members/GetQuizHistory?memberId=" + member.id + "&buster=" + new Date().getTime()).success(function (response) {
            $scope.memberQuizHistory = response;
        });
    };

    var updateListFunc = _.debounce(function (value) {
        value = value || "";
        $scope.$apply(function () {
            $scope.users = _.filter($scope.fullMemberList, function (item) {
                if (item.firstName && item.firstName.toUpperCase().indexOf(value.toUpperCase()) != -1)
                    return true;
                if (item.lastName && item.lastName.toUpperCase().indexOf(value.toUpperCase()) != -1)
                    return true;
                if (item.phoneNumber && item.phoneNumber.toUpperCase().indexOf(value.toUpperCase()) != -1)
                    return true;
                return false;
            });
        });
    }, 500);

    $scope.$watch("filter", function (newValue) {
        updateListFunc(newValue);
    });

    $scope.startEditing = function () {
        $scope.isEditing = true;
    };
    $scope.cancelEdit = function () {
        $scope.isEditing = false;
    };

    $scope.currentMember = {};

    $scope.updateMember = function () {
        $http.post("/Members/UpdateMember", $scope.currentMember).success(function () {
            loadMembers();
        });
    };
    $scope.markInactiveStarted = false;
    $scope.beginMark = function () {
        $scope.markInactiveStarted = true;
    };
    $scope.cancelMark = function () {
        $scope.markInactiveStarted = false;
    };
    $scope.markMemberInactive = function () {
        $http.post("/Members/MarkInactive", $scope.currentMember).success(function () {
            loadMembers();
            $scope.currentMember = {};
            $scope.markInactiveStarted = false;
        });
    };
    $scope.$on("outOfDate", function () {
        loadMembers();
    });
    $scope.viewPrizes = function (member) {
        $scope.awardedPrizes = [];
        $("#MemberPrizesDialog").modal("show");
        $http.get("/Prizes/GetAwardedPrizesForMember?memberId=" + member.id).success(function (response) {
            $scope.awardedPrizes = response;
        });
    };
    $scope.redeemPrize = function (prize) {
        $http.post("/Prizes/redeemPrize", prize).success(function (response) {
            prize.redeemed = true;
        });
    };
    $scope.refundPrize = function(prize) {
        var confirmed = confirm("Are you sure you want to refund this prize worth " + prize.prizePoints + "? This action only gives the points back to the patient whether they got the gift or not and does not undo the gift card that was sent or the payment for that gift card. This action cannot be undone.");
        if (!confirmed)
            return;

        $http.post("/Prizes/refundPrize", prize).success(function (response) {
            alert("Completed");
        });
        loadMembers();
    };

    $scope.sendStartGame = function(memberId) {
        $http.post("/Messaging/SendGameUrl", { memberId: memberId }).success(function(response) {
            if (response.error)
                alert("Looks like that patient is currently not allowed to play today. Here is the message they will see if they try to play:\n\n " + response.error);
        });
    };
    loadMembers();
}]);
angular.module("MainModule").directive("addNewMemberController", ['$http', function ($http) {
    return {
        restrict: 'A',
        link: function ($scope, elem, attr, ctrl) {
            var setNewMember = function () {
                $scope.newMember = {};
                $scope.newMember.firstName = "";
                $scope.newMember.lastName = "";
                $scope.newMember.phoneNumber = "";
                $scope.newMember.sendSms = true;
                $scope.newMemberForm.$setPristine();
            };
            setNewMember();
            $scope.cancelAdd = function () {
                $('#AddNewMemberDialog').modal('hide');
                setNewMember();
            };
            $scope.addMember = function () {
                $scope.errorMessage = "";
                if ($scope.newMemberForm.$invalid)
                    return;
                $http.post("/Members/AddMember", $scope.newMember).success(function (response) {
                    if (response.successful) {
                        $scope.$emit("memberAdded", angular.copy($scope.newMember));
                        $scope.cancelAdd();
                        $scope.newMember = {};
                    } else {
                        $scope.errorMessage = response.message;
                    }
                });
            };
            $scope.phoneNumberMasking = function (newValue) {
                $scope.newMember.phoneNumber = $scope.newMember.phoneNumber.replace("-", "").replace("(", "").replace(")", "").replace(" ", "");
            };
        },
        templateUrl: "/Templates/SubTemplates/AddNewMemberTemplate.html"
    };

}])