angular.module("MainModule").directive("changePasswordPanel", ['$http', "$location", function ($http, $location) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            $scope.errors = "";
            $scope.uniqueToken = "";
            $scope.email = "";
            $scope.password = "";
            $scope.confirmPassword = "";

            $scope.goToPanel = function (destination) {
                $scope.locationModel = destination;
            };

            $scope.changePassword = function () {
                var data = {
                    email: $scope.email,
                    uniqueToken: $scope.uniqueToken,
                    password: $scope.password,
                    confirmPassword: $scope.confirmPassword
                };
                $http.post("/Password/ChangePassword", data).success(function (result) {
                    $scope.errors = "";
                    if (result.successful == false) {
                        if (result.reason == "invalidPassword")
                            $scope.errors = 'Looks like the password you wrote was too short (more than 6 characters please)';
                        if (result.reason == "password")
                            $scope.errors = 'Looks like the passwords you wrote didn\'t match. Try Again!';
                        if (result.reason == "badToken")
                            $scope.errors = 'That token didn\'t work with the email you provided. Try Again';
                        if (result.reason == "NoTokenForUser")
                            $scope.errors = 'There was no token for that email. Did you request one yet?';
                    } else {
                        alert("Success! Now try to login again!");
                       $location.path('/loginPanel');
                    }
                });
            };
        },
    };
}]);