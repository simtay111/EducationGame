mainModule.directive("prizeSummary", ['$compile', function ($compile) {
    return {
        scope: {
            currentPoints: '='
        },
        link: function ($scope, elem, attr) {
            TweenMax.fromTo($("#PointAmountTxt"), .75, { 'color': '#26cd32', repeat: -1, yoyo: true }, { 'color': "#00b800", repeat: -1, yoyo: true });
            $scope.$watch("currentPoints", function () {
                $("#CurrentPointsLabel").effect("highlight", { color: 'chartreuse' });
            });

            $scope.browsePrizes = function () {
                $scope.$emit("Next", { destination: "/browsePrizes" });
            };
        },
        templateUrl: "/Templates/Quizes/prizes_summary.html"
    };
}]);