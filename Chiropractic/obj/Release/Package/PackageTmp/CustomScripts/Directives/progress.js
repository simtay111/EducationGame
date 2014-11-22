mainModule.directive("progress", function () {
    return {
        scope: {
            current: '=',
            totalPoints: '='
        },
        link: function ($scope, elem, attr) {
            var buildKnob = function () {
                $(elem).val(getValue());
                $(elem).val(getValue());
                $(elem).knob({ min: 0, max: 100, format: function(v) {return v +  "%"} });
            };
            $scope.$watch("current", function () {
                $(elem).val(getValue());
                $(elem).val(getValue());
                $(elem).trigger('change');
            });
            $scope.$watch("totalPoints", function () {
                $(elem).val(getValue());
                $(elem).trigger('change');
            });
            var getValue = function() {
                return Math.round(($scope.current / $scope.totalPoints) * 100);
            };

            buildKnob();
        }
    };
})