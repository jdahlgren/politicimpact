function InitialiseFacebook(appId) {
    var credentials
    
    window.fbAsyncInit = function () {
        FB.init({
            appId: appId,
            status: true,
            cookie: true,
            xfbml: true
        });

        FB.Event.subscribe('auth.login', function (response) {
            credentials = { uid: response.authResponse.userID, accessToken: response.authResponse.accessToken };
            SubmitLogin(credentials);
        });
        
        FB.getLoginStatus(function (response) {
            
            if (response.session) {
                // logged in and connected user, someone you know
                credentials = { uid: response.authResponse.userID, accessToken: response.authResponse.accessToken };
                SubmitLogin(credentials);
            }
            
            if (response.status === 'connected') {
                credentials = { uid: response.authResponse.userID, accessToken: response.authResponse.accessToken };
                if (credentials == null) {
                    SubmitLogin(credentials);
                }
            }
            else if (response.status === 'not_authorized') { alert("user is not authorised"); }
            
        });
        
        function SubmitLogin(credentials) {
            $.ajax({
                //url: "/account/facebooklogin",
                url: "/Account/Index",
                type: "POST",
                data: credentials,
                error: function () {
                    alert("error logging in to your facebook account.");
                },
                success: function () {
                    window.location.reload();
                }
            });
        }

    };

    (function (d) {
        var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
        if (d.getElementById(id)) {
            return;
        }
        js = d.createElement('script');
        js.id = id;
        js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js";
        ref.parentNode.insertBefore(js, ref);
    }(document));

}