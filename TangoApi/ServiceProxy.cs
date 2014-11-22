using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using TangoApi.Entity;

namespace TangoApi
{
    public class ErrorResponse
    {
        public bool success { get; set; }
        public string error_message { get; set; }
        public string invalid_input_message { get; set; }
        public List<InvalidInput> invalid_inputs { get; set; }
    }

    public class InvalidInput
    {
        public string field { get; set; }
        public string error { get; set; }
    }
    public class ServiceProxy
    {
        public TResponse Execute<TRequest, TResponse>(TRequest request, string uri)
        {
            Console.WriteLine(uri);
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/json; charset=utf-8");
            client.Credentials = new NetworkCredential(TangoCredentials.Identifier, TangoCredentials.Key);
            var data = JsonConvert.SerializeObject(request);
            Console.WriteLine(data);
            string uriString = uri;
            Console.WriteLine(uriString);
            var json = "";
            try
            {
                if (data.Length > 3)
                {
                    Console.WriteLine("POSTING");
                    json = client.UploadString(new Uri(uriString), "POST", data);
                }
                else
                {
                    Console.WriteLine("GETTING");
                    json = client.DownloadString(new Uri(uriString));
                    Console.WriteLine(json);
                }
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpErrorResponse = (HttpWebResponse)we.Response as HttpWebResponse;

                    StreamReader reader = new StreamReader(httpErrorResponse.GetResponseStream(), Encoding.UTF8);
                    string responseBody = reader.ReadToEnd();
                    Console.WriteLine(responseBody);
                    var errResp = JsonConvert.DeserializeObject<ErrorResponse>(responseBody);
                    Trace.WriteLine(errResp.error_message);
                    //foreach (var invalid in errResp.invalid_inputs)
                    //{
                       //Console.WriteLine(invalid.field); 
                       //Console.WriteLine(invalid.error); 
                    //}
                    throw new TangoException(errResp.error_message, errResp.error_message );
                }
                    throw we;
            }

            var items = JsonConvert.DeserializeObject<TResponse>(json);

            return items;
        }
    }
}