angular.module("Quizzes").directive("quizItem", function($state) {
    return {
        scope: {
            quizItem: '='
        },
        link: function($scope, elem, attr) {
            $scope.selectItem = function(id) {
                $state.go("game", { id: id });
            }
        },
        replace: true,
        templateUrl: "/V2Templates/quizzes/quiz_item.html"
    }
});