angular.module("MainModule").directive("forgotPasswordPanel", ['$location', '$http', function ($location, $http) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            $scope.email = "";
            $scope.errors = "";
            $scope.goToPanel = function (destination) {
                $location.path("/" + destination);
            };
            $scope.resetPassword = function() {
                $http.post("/Password/ForgotPassword", { username: $scope.email }).success(function(result) {
                    if (result.successful == false) {
                       $scope.errors = 'Hmm, no user found with that email. Mistype it?';
                    } else {
                        $scope.goToPanel('waitForPasswordEmailPanel');
                    }
                });
            };
        },
    };
}]);