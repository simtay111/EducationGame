angular.module("Quizzes", ['ui.router']).controller("quizSearch", ['$scope', function ($scope) {
    $scope.items = [
  {
      "id": 21582,
      "name": "Comcubine",
      "company": "Ziore",
      "points": 177
  },
  {
      "id": 677,
      "name": "Zensor",
      "company": "Golistic",
      "points": 201
  },
  {
      "id": 87239,
      "name": "Geekosis",
      "company": "Endicil",
      "points": 266
  },
  {
      "id": 82323,
      "name": "Accufarm",
      "company": "Extragen",
      "points": 298
  },
  {
      "id": 26529,
      "name": "Ozean",
      "company": "Plasto",
      "points": 149
  },
  {
      "id": 75449,
      "name": "Nutralab",
      "company": "Enjola",
      "points": 166
  },
  {
      "id": 31329,
      "name": "Kidgrease",
      "company": "Obones",
      "points": 171
  },
  {
      "id": 3802,
      "name": "Providco",
      "company": "Digifad",
      "points": 172
  },
  {
      "id": 42330,
      "name": "Nikuda",
      "company": "Locazone",
      "points": 172
  },
  {
      "id": 90202,
      "name": "Confrenzy",
      "company": "Codact",
      "points": 264
  },
  {
      "id": 3451,
      "name": "Kangle",
      "company": "Centrexin",
      "points": 116
  },
  {
      "id": 1329,
      "name": "Geekmosis",
      "company": "Austech",
      "points": 251
  },
  {
      "id": 78434,
      "name": "Ludak",
      "company": "Recognia",
      "points": 182
  },
  {
      "id": 32095,
      "name": "Pyrami",
      "company": "Evidends",
      "points": 170
  },
  {
      "id": 63821,
      "name": "Glukgluk",
      "company": "Knowlysis",
      "points": 198
  },
  {
      "id": 94948,
      "name": "Portica",
      "company": "Zoinage",
      "points": 123
  },
  {
      "id": 25143,
      "name": "Vixo",
      "company": "Dognosis",
      "points": 266
  },
  {
      "id": 24349,
      "name": "Xleen",
      "company": "Zilch",
      "points": 252
  },
  {
      "id": 12936,
      "name": "Memora",
      "company": "Zanilla",
      "points": 137
  },
  {
      "id": 63664,
      "name": "Makingway",
      "company": "Omnigog",
      "points": 246
  }
    ];
    $scope.sortBy = function (type) {
        $scope.isoContainer.isotope({ sortBy: type });
    };

    setTimeout(function () {
        $scope.isoContainer = $('#QuizItemsContainer').isotope({
            itemSelector: ".quiz-item",
            layoutMode: 'masonry',
            getSortData: {
                points: '.quiz-item-points',
                name: '.quiz-item-name',
                company: '.quiz-item-company'
            }
        });
    }, 100);
}]);