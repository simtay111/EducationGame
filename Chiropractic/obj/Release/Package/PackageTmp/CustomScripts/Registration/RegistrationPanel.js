angular.module("RegisterModule").directive("customerRegisterPanel", ['$location', '$http', function ($location, $http) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            $scope.email = "";
            $scope.pwd = "";
            $scope.confPwd = "";
            $scope.showContractModal = function () {
                $scope.errors = "";
                if ($scope.pwd != $scope.confPwd) {
                    $scope.errors = "Passwords do not match.";
                    return;
                }
                $('#RegistrationDisclaimerModal').modal('show');
            };

            $scope.registerBtnClickHandler = function () {
                if ($scope.loginForm.$invalid) {
                    $scope.errors = "Please fill in all fields correctly before continuing";
                    return;
                }
                $scope.errors = "";
                $scope.isLoading = true;
                $http.post("/Register/RegisterUser", { username: $scope.email, password: $scope.pwd, confirmPassword: $scope.confPwd }).success(function (response) {
                    $scope.isLoading = false;
                    $('#RegistrationDisclaimerModal').modal('hide');
                    if (!response.Successful) {
                        $scope.errors = "Oops, something did not go right, please refill out the forms";
                        if (response.Reason == "EmailAlreadyInUse")
                            $scope.errors = "That email is already in use";
                        if (response.Reason == "PasswordMismatch")
                            $scope.errors = "The passwords do not match";
                        if (response.Reason == "InvalidFormat")
                            $scope.errors = "The email is not a valid email";
                        if (response.Reason == "RequestNotFinished")
                            $scope.errors = "Please make sure all fields are filled in the passwords match";
                        return;
                    }
                    $scope.isAuthorized = true;
                    _.delay(function () { $scope.$apply(function () { $scope.$emit("nextSlide", "firstPanel"); }) }, 200);
                });
            };
        },
    };
}]);
angular.module("RegisterModule").directive("theTerms", [function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: "/Templates/Registration/tos.html"
    };
}]);