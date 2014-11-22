angular.module("MainModule").directive("waitForPasswordEmailPanel", ['$location', function ($location) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            $scope.goToPanel = function (destination) {
                $location.path("/" + destination);
            };
        },
    };
}]);