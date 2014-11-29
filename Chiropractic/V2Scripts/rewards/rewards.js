angular.module("Rewards", ['ui.router']).controller("rewardsBase", ['$state', '$scope', '$stateParams', function ($state, $scope, params) {
    $scope.items = [{ id: 1, name: 'biscuit' }, { id: 2, name: "item2" }];
}]).directive("rewardItem", ['$state', function ($state) {
    return {
        replace: true,
        scope: {
            rewardItem: '='
        },
        link: function ($scope, elem, attr) {
            $scope.selectItem = function (id) {
                $state.go("rewards.view", { rewardId: id });
            }
        },
        templateUrl: "/V2Templates/rewards/reward_item.html"
    }
}]).controller("viewReward", ['$state', '$scope', '$stateParams', function ($state, $scope, params) {
    $scope.rewardId = parseInt(params.rewardId);
}]).config(["$stateProvider", function ($stateProvider) {
    $stateProvider.state("rewards", {
        url: '/rewards',
        template: '<div ui-view></div>',
        controller: 'rewardsBase'
    }).state("rewards.browse", {
        url: "/browse",
        controller: function () { },
        templateUrl: "/V2Templates/rewards/browse_rewards.html"
    }).state("rewards.view", {
        url: "/view/:rewardId",
        controller: 'viewReward',
        template: "<h1>Reward {{rewardId}}</h1>"
    });
}]);