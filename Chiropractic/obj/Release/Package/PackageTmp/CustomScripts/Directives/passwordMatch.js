mainModule.directive('passwordMatch', [function () {
    return {
        restrict: 'A',
        scope: true,
        require: 'ngModel',
        link: function (scope, elem, attrs, control) {
            var checker = function () {

                //get the value of the first password
                var e1 = $(attrs.firstPass).val();

                //get the value of the other password  
                var e2 = $(attrs.secondPass).val();
                return e1 == e2;
            };
            scope.$watch(checker, function (n) {

                //set the form control to valid if both 
                //passwords are the same, else invalid
                control.$setValid();
            });
        }
    };
}]);