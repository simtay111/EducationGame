mainModule.config(function ($httpProvider, $routeProvider) {
    $httpProvider.interceptors.push(function ($q, $location) {
        return {
            'responseError': function (response) {
                alert('It appears you are not logged in or you do not have enough permissions(ie only available to the chiropractor) to view this page.');
                window.location = "/Login";
                return "";
            },
        };
    });

    $routeProvider.when("/firstPanel", {
        template: "<h3>Select an option to begin</h3>"
    }).when("/basicInfoPanel", {
        templateUrl: "/Templates/Registration/BasicInfoPanel.html"
    }).when("/addAssistantPanel", {
        templateUrl: "/Templates/Registration/AddAssistantPanel.html"
    }).when("/paymentSetup", {
        templateUrl: "/Templates/Registration/PaymentSetupPanel.html"
    }).when("/logoUploadPanel", {
        templateUrl: "/Templates/Registration/LogoUploadPanel.html"
    }).when("/setupPrizesPanel", {
        templateUrl: "/Templates/Registration/SetupPrizesPanel.html"
    }).otherwise({
        template: "<h3>Select an option to begin</h3>"
    });
});

mainModule.controller("managementBaseController", ['$scope', '$location', '$http', function ($scope, $location, $http) {
    $scope.goToPanel = function (url) {
        $location.path("/" + url);
    };
    $http.post("/Login/CheckAuthorization").success(function (response) {
        $scope.isAuthorized = response.isAuthorized;
    });
}]);