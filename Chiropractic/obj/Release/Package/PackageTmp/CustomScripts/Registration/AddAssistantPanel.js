angular.module("RegisterModule").directive("addAssistantPanel", ['$http', function ($http) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            $scope.email = "";
            $scope.pwd = "";
            $scope.confPwd = "";
            $scope.isAddingAcct = false;
            $scope.accounts = [];

            $scope.deleteAccount = function (index) {
                var deletedAcct = $scope.accounts.splice(index, 1)[0];
                $http.post("/Accounts/DeleteAsstAcct", deletedAcct);
            };

            var getExistingAccounts = function () {
                $http.get("/Accounts/GetAssistantAccounts").success(function (response) {
                    $scope.accounts = response.accounts;
                });
            };

            $scope.finish = function () {
                $scope.$emit("nextSlide", "addAssistantPanel");
            };
            $scope.addAssistant = function () {
                $scope.isAddingAcct = true;
            };
            $scope.cancelAdd = function () {
                $scope.isAddingAcct = false;
            };

            $scope.saveAccount = function () {
                $scope.errors = "";
                if ($scope.pwd != $scope.confPwd) {
                    $scope.errors = "Passwords do not match.";
                    return;
                }
                if ($scope.loginForm.$invalid) {
                    $scope.errors = "Please fill in all fields correctly before continuing";
                    return;
                }
                $scope.errors = "";
                $http.post("/Register/RegisterAssistant", { displayName: $scope.displayName, username: $scope.email, password: $scope.pwd, confirmPassword: $scope.confPwd }).success(function (response) {
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
                    getExistingAccounts();
                    $scope.email = "";
                    $scope.pwd = "";
                    $scope.confPwd = "";
                    $scope.isAddingAcct = false;
                });
            };

            getExistingAccounts();
        },
    };
}]);