angular.module("Stories", ['ui.router', 'ngResource']).controller("storiesViewController", [
    '$state', '$scope', '$stateParams', '$http', function($state, $scope, params, $http) {
        $scope.stories = [];
    }
]);