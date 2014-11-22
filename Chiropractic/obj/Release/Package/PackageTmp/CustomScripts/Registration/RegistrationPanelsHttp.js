mainModule.config(function ($httpProvider, $routeProvider) {
    $routeProvider.when("/firstPanel", {
        templateUrl: "/Templates/Registration/RegisterPanel.html"
    }).when("/basicInfoPanel", {
        templateUrl: "/Templates/Registration/BasicInfoPanel.html"
    }).when("/paymentSetup", {
        templateUrl: "/Templates/Registration/PaymentSetupReg.html"
    }).otherwise({
        templateUrl: "/Templates/Registration/RegisterPanel.html"
    });
});

mainModule.controller("registrationBaseController", ['$scope', '$location', function ($scope, $location) {
    $scope.order = ["firstPanel", "basicInfoPanel", "paymentSetup"];
    $scope.currentSlide = 1;
    $scope.isAuthorized = false;

    $scope.$on("nextSlide", function (data, sendingSlide) {
        $scope.currentSlide = _.indexOf($scope.order, sendingSlide) + 1;
        if ($scope.currentSlide == $scope.order.length)
            window.location = "/Home/UserHome";
        else {
            $location.path("/" + $scope.order[$scope.currentSlide]);
        }
    });
    $scope.goToPanel = function (destination) {
        $location.path("/" + destination);
    };
    $scope.login = function () {
        $scope.goToPanel("loginPanel");
    };
    $scope.skip = function () {
        $scope.$emit("nextSlide", $scope.order[$scope.currentSlide]);
    };

    $scope.currentPercentage = function() {
        return Math.floor(($scope.currentSlide / $scope.order.length) * 100);
    };

    if ($location.path().length < 5)
        $location.path("/firstPanel");
    $scope.currentSlide = $scope.order.indexOf($location.path().replace("/", ""));
}]);