angular.module("MainModule").directive("loginPanel", ['$location', "$http", function ($location, $http) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            $scope.loginName = window.localStorage && window.localStorage["chiroUsername"];
            $scope.password = "";
            $scope.goToPanel = function (destination) {
                $location.path("/" + destination);
            };

            $scope.customerLogin = function () {
                $http.post('/login/login', { Username: $scope.loginName, Password: $scope.password }).
                    success(function(data) {
                        if (!data.successfulLogin) {
                            $scope.errors = "Could not login with the credentials you specified";
                            if (data.reason == "AccountNotVerified")
                                $scope.errors = "Your account is not verified yet, once we confirm it, you will able to login.";
                        } else {
                            window.localStorage["chiroUsername"] = $scope.loginName;
                            window.location = "/Home/UserHome";
                        }
                    });
            };
        },
    };
}]);