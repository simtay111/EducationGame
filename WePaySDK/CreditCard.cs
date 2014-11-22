using Newtonsoft.Json;
using WePaySDK;

namespace WePaySDK
{
    public class CreditCard
    {
        public CreditCardCreateResponse Create(CreditCardCreateRequest req)
        {
            CreditCardCreateResponse response;
            try
            {
                response = new WePayClient().Invoke<CreditCardCreateRequest, CreditCardCreateResponse>(req, req.actionUrl, req.accessToken);
            }
            catch (WePayException ex) 
            {
                response = new CreditCardCreateResponse { credit_card_id = 0, state = "?error=" + ex.error, Error = ex };
            }

            return response;
        }

        public CreditCardAuthorizeResponse Authorize(CreditCardAuthorizeRequest req)
        {
            CreditCardAuthorizeResponse response;
            try
            {
                response = new WePayClient().Invoke<CreditCardAuthorizeRequest, CreditCardAuthorizeResponse>(req, req.actionUrl, req.accessToken);
            }
            catch (WePayException ex)
            {
                response = new CreditCardAuthorizeResponse { state = "?error=" + ex.error, Error = ex };
            }

            return response;
        }

        //public CreditCardResponse GetStatus(long checkout_id)
        //{
        //    var req = new CreditCardRequest { checkout_id = checkout_id };
        //    CreditCardResponse response;
        //    try
        //    {
        //        response = new WePayClient().Invoke<CreditCardRequest, CreditCardResponse>(req, req.actionUrl);
        //    }
        //    catch (WePayException ex) 
        //    {
        //        response = new CreditCardResponse { state = ex.error, amount = 0, Error = ex };
        //    }
        //    return response;
        //}
        public CreditCardResponse Post(CreditCardRequest req)
        {
            CreditCardResponse response;
            try
            {
                response = new WePayClient().Invoke<CreditCardRequest, CreditCardResponse>(req, req.actionUrl, req.accessToken);
            }
            catch (WePayException ex)
            {
                response = new CreditCardResponse { state = "?error=" + ex.error, Error = ex };
            }

            return response;
        }

        public CreditCardDeleteResponse Delete(CreditCardDeleteRequest req)
        {
            CreditCardDeleteResponse response;
            try
            {
                response = new WePayClient().Invoke<CreditCardDeleteRequest, CreditCardDeleteResponse>(req, req.actionUrl, "");
            }
            catch (WePayException ex)
            {
                response = new CreditCardDeleteResponse { state = "?error=" + ex.error, Error = ex };
            }
            
            return response;
        }
    }

    public class CreditCardCreateRequest
    {
        [JsonIgnore]
        public readonly string actionUrl = @"credit_card/create";

        [JsonIgnore]
        public string accessToken { get; set; }
        [JsonIgnore]
        public long account_id { get; set; }
        public long client_id { get; set; }
        public long cc_number { get; set; }
        public long cvv { get; set; }
        public long expiration_month { get; set; }
        public long expiration_year { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public Address address { get; set; }
    }

    public class Address
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
    }

    public class CreditCardCreateResponse
    {
        public long credit_card_id { get; set; }
        public string state { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
    }

    public class CreditCardAuthorizeRequest
    {
        public long client_id { get; set; }
        public string client_secret { get; set; }
        public long credit_card_id { get; set; }
        [JsonIgnore]
        public string accessToken { get; set; }

        [JsonIgnore]
        public readonly string actionUrl = @"credit_card/authorize";
    }

    public class CreditCardDeleteRequest
    {
        public long client_id { get; set; }
        public string client_secret { get; set; }
        public long credit_card_id { get; set; }

        [JsonIgnore]
        public readonly string actionUrl = @"credit_card/delete";
    }
    public class CreditCardDeleteResponse
    {
        public long credit_card_id { get; set; }
        public string state { get; set; }
        [JsonIgnore]
        public WePayException Error { get; set; }
    }

    public class CreditCardAuthorizeResponse
    {
        public long credit_card_id { get; set; }
        public string state { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
    }

    public class CreditCardRequest
    {
        public long client_id { get; set; }
        public string client_secret { get; set; }
        public long credit_card_id { get; set; }
        [JsonIgnore]
        public string accessToken { get; set; }

        [JsonIgnore]
        public readonly string actionUrl = @"credit_card";
    }

    public class CreditCardResponse
    {
        public long credit_card_id { get; set; }
        public string state { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
    }
}