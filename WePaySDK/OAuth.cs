﻿using Newtonsoft.Json;

namespace WePaySDK
{
    public class OAuth
    {
        public TokenResponse Authorize(TokenRequest req)
        {
            TokenResponse response;
            try
            {
                response = new WePayClient().Invoke<TokenRequest, TokenResponse>(req, req.actionUrl);
            }
            catch (WePayException ex)
            {
                response = new TokenResponse { access_token = "error", Error=ex };
            }

            return response;
        }
    }

    public class TokenRequest
    {
        public int client_id { get; set; }
        public string redirect_uri { get; set; }
        public string client_secret { get; set; }
        public string code { get; set; }

        [JsonIgnore]
        public string actionUrl = @"oauth2/token";
    }

    public class TokenResponse
    {
        public int user_id { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
    }
}