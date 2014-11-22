﻿using Newtonsoft.Json;

namespace WePaySDK
{
    public class User
    {
        public UserRegisterResponse Register(UserRegisterRequest req)
        {
            UserRegisterResponse response;
            try
            {
                response = new WePayClient().Invoke<UserRegisterRequest, UserRegisterResponse>(req, req.actionUrl);
            }
            catch (WePayException ex)
            {
                response = new UserRegisterResponse { access_token = "error", Error=ex };
            }

            return response;
        }

        public UserResponse GetUser(string accessToken)
        {
            UserRequest req = new UserRequest { accessToken = accessToken };
            UserResponse response;
            try
            {
                response = new WePayClient().Invoke<UserRequest, UserResponse>(req, req.actionUrl, accessToken);
            }
            catch (WePayException ex)
            {
                response = new UserResponse { state = "error", Error=ex };
            }

            return response;
        }
    }

    public class UserRegisterRequest
    {
        [JsonIgnore]
        public string actionUrl = @"user/register";

        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string email { get; set; }
        public string scope { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string original_ip { get; set; }
        public string original_device { get; set; }
        public string redirect_uri { get; set; }
    }

    public class UserRegisterResponse
    {
        public string user_id { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
    }

    public class UserRequest
    {
        [JsonIgnore]
        public string accessToken { get; set; }

        [JsonIgnore]
        public string actionUrl = @"user";
    }

    public class UserResponse
    {
        public string user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string state { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
    }

}