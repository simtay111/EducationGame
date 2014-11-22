mainModule.directive("fadeIn", function () {
    return {
        link: function ($scope, elem, attr) {
            TweenMax.fromTo($(elem), 1.5, { opacity: 0 }, { opacity: 1 });
        }
    };
})