mainModule.directive("logoUploadPanel", ['$http', function($http) {
    return {
        link: function($scope, elem, attr) {
            $scope.currentImageUrl = "";

            $http.get("/Accounts/GetLogoUrl" + "?ticks=" + new Date().getTime()).success(function(response) {
                $scope.currentImageUrl = response.url +  "?" + new Date().getTime();
            });

            $scope.deleteImage = function() {
                $http.post("/UPload/Delete").success(function() {
                    $scope.currentImageUrl = "" ;
                });
            };
            
            $scope.newImageUploaded = false;
            $(":file").filestyle({ input: false, buttonText: "Upload Logo", classButton: "btn btn-success btn-lg"});

            $('#logoImage').on("error", function() {
                $scope.$apply(function() {
                    $scope.currentImageUrl = "";
                });
            });

            $('#fileupload').fileupload({
                progressall: function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    $('.progress .progress-bar').css(
                        'width',
                        progress + '%'
                    );
                },
                url: '/Upload/Image/',
                success: function (response) {
                    $scope.newImageUploaded = true;
                    $scope.$apply(function () {
                        $scope.currentImageUrl = response.filename + "?ticks=" + new Date().getTime();
                    });
                },
                error: function (response) {
                    alert('Looks like you are not loggin in. Please login to continue');
                    $scope.$apply(function () {
                        $scope.goToPanel('/loginPanel');
                    });
                },
                dataType: 'json',
                // Enable image resizing, except for Android and Opera,
                // which actually support image resizing, but fail to
                // send Blob objects via XHR requests:
                disableImageResize: /Android(?!.*Chrome)|Opera/
                    .test(window.navigator && navigator.userAgent),
            });

        }
    };
}]);