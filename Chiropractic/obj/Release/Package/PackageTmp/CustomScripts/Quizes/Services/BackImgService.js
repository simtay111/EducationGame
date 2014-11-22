angular.module("mainModule").service("backImgService", function () {
    var lastUsedId = 0;
    var secondToLastUsed = 0;
    var slideImages = [];
    return {
        setLastUsedId: function (id) {
            secondToLastUsed = lastUsedId;
            lastUsedId = id; 
        },
        getLastUsedId: function () {
            return lastUsedId;
        },
        getSecondToLastUsedId: function () {
            return lastUsedId;
        },
        setSlideImages: function (data) {
            slideImages = _.shuffle(data);
        },
        getNextSlideImage: function() {
            var nextImageId = slideImages.shift();
            slideImages.push(nextImageId);
            return nextImageId;
        }
    };
});