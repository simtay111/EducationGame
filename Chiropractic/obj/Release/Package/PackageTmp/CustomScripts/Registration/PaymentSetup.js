mainModule.directive("paymentSetup", ['$http', function ($http) {
    return {
        link: function ($scope, elem, attr) {
            WePay.set_endpoint("production"); // change to "production" when live //stage
            //WePay.set_endpoint("stage"); // change to "production" when live //stage

            $scope.hasCcOnFile = false;
            $scope.autoRenew = false;
            $scope.price = 2;
            $scope.currentType = 1;

            $scope.onAutoRenewChange = function () {
                if ($scope.autoRenew) {
                    var confirmed = confirm("By selecting this you are agreeing to allow PracticeOwl to charge your account when your current subscription ends with the subscription type selected above.  You may change this subscription at any time and auto renew will use whatever one you choose. ");
                    if (confirmed)
                        $http.post("/CreditCard/UpdateAutoRenew", { autoRenew: $scope.autoRenew }).success(function () {
                        });
                }
            };

            var getStatus = function () {
                $http.get("/CreditCard/Status").success(function (response) {
                    $scope.hasCcOnFile = response.hasCcOnFile;
                    $scope.autoRenew = response.autoRenew;
                    $scope.cost = response.cost;
                    $scope.currentType = response.price;
                    $scope.subscriptionEnd = moment(response.subscriptionThrough).calendar();
                    $scope.payedOnce = response.payedOnce;
                });
            };
            getStatus();

            $scope.updateSubscription = function () {
                $http.post("/CreditCard/UpdateSubscription", { price: $scope.currentType });
            };

            $scope.removeCard = function () {
                $http.post("/CreditCard/Delete").success(function (response) {
                    $scope.hasCcOnFile = false;
                });
            };

            $scope.startBillingAgain = function () {
                $scope.isLoading = true;
                $http.post("/CreditCard/BillClinic").success(function (response) {
                    $scope.isLoading = false;
                    getStatus();
                    alert(response.message);
                });
            };

            $scope.saveCard = function () {
                var confirmString = "$141 for 3 months of service.";
                if ($scope.price == 2)
                    confirmString = "$97 each month for service.";
                if ($scope.price == 3)
                    confirmString = "$997 for one year of service.";
                var confirmed = confirm("By continuing you authorize us to charge " + confirmString);
                if (!confirmed)
                    return;
                $scope.errors = "";
                var response = WePay.credit_card.create({
                    //"client_id": 151055, //staging
                    "client_id": 150580, //prod
                    "user_name": $scope.name,
                    "email": $scope.email,
                    "cc_number": $scope.ccNum,
                    "cvv": $scope.cvv,
                    "expiration_month": $scope.month,
                    "expiration_year": $scope.year,
                    "address":
                    {
                        "zip": $scope.zipCode
                    }
                }, function (data) {
                    if (data.error) {
                        $scope.$apply(function () {
                            $scope.errors = data.error_description;
                        });
                    } else {
                        $scope.isLoading = true;
                        $http.post("/CreditCard/AddCreditCardToAccount", { token: data.credit_card_id, promoCode: $scope.promoCode, price: $scope.price }).success(function (response) {
                            $scope.isLoading = false;
                            if (response.error) {
                                $scope.errors = response.error;
                                return;
                            }
                            alert("We charged your account " + response.costCharged + ". Thanks for signing up! We have some tutorials to get you started." +
                                " Happy hands-off educating!");
                            $scope.hasCcOnFile = true;
                            $scope.$emit("nextSlide", "paymentSetup");
                        });
                    }
                });
            };
        }
    };
}]);