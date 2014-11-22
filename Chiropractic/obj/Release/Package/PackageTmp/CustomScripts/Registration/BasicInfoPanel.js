angular.module("RegisterModule").directive("basicInfoPanel", ['$location', "$http", "numberFilter", function ($location, $http, numberFilter) {
    return {
        restrict: 'A',
        controller: function ($scope) {
            $scope.accountInformation = {};
            var getAccountInformation = function () {
                $http.get('/Accounts/GetAccountInformation').success(function (response) {
                    $scope.accountInformation = response;
                });
            };

            $scope.errors = "";

            $scope.updateAccountInfo = function () {
                $scope.errors = "";
                if ($scope.accountInfoEditForm.$invalid) {
                    $scope.errors = "Please fill in all the fields to save.";
                    return;
                }
                $http.post("/Accounts/UpdateAccountInformation", $scope.accountInformation).success(function () {
                    $scope.accountInfoEditForm.$setPristine();
                    $scope.$emit("nextSlide", "basicInfoPanel");
                });
            };

            getAccountInformation();

            $scope.phoneNumberMasking = function (newValue) {
                $scope.accountInformation.officePhone = $scope.accountInformation.officePhone.replace("-", "").replace("(", "").replace(")", "").replace(" ", "");
            };
        }
    };
}]);