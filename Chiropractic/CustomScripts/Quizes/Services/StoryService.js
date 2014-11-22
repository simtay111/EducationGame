mainModule.service("storyService", function () {
    var slides = [];
    var questions = [];
    var story = {};
    var user = {};
    var prizes = {};
    var currentPoints = 0;
    var accountInformation = {};
    return {
        loadService: function(newStory, newSlides, newQuestions, newUser, newPrizes, acctInfo) {
            story = newStory;
            slides = newSlides;
            questions = newQuestions;
            user = newUser;
            prizes= newPrizes;
            currentPoints = 0;
            accountInformation = acctInfo;
        },
        addPointsToScore: function(points) {
            currentPoints += points;
        },
        getAcctInfo: function() {
            return accountInformation;
        },
        getPoints: function() {
            return currentPoints;
        },
        getUser: function() {
            return user;
        },
        getPrizes: function() {
            return prizes;
        },
        getStory: function () {
            return story;
        },
        getSlide: function (index) {
            return slides[index - 1];
        },
        getNumberOfSlides: function() {
            return slides.length;
        },
        getQuestion: function (index) {
            return questions[index - 1];
        },
        getNumberOfQuestions: function() {
            return questions.length;
        },
    };
});

