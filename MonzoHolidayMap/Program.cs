using System;
using System.Linq;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace MonzoHolidayMap
{
    class Program 
    {
        static void Main(string[] args)
        {
            using (var stream = File.OpenRead("./.env"))
            {
                DotNetEnv.Env.Load(stream);
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var client = new RestClient("https://api.monzo.com");
            
            var request = new RestRequest("transactions", Method.GET);
            request.AddParameter("expand[]", "merchant");

            request.AddParameter("account_id", Environment.GetEnvironmentVariable("ACCOUNT_ID")); // adds to POST or URL querystring based on Method
                                                                                   
            // easily add HTTP Headers
            request.AddHeader("authorization", "Bearer " + Environment.GetEnvironmentVariable("AUTHORIZATION"));

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            Console.Write(content);
            Console.Write(response.StatusCode);

            LogRequest(request, response, client);
 
        }
        
        private static void LogRequest(IRestRequest request, IRestResponse response, RestClient restClient)
        {
            var requestToLog = new
            {
                resource = request.Resource,
                // Parameters are custom anonymous objects in order to have the parameter type as a nice string
                // otherwise it will just show the enum value
                parameters = request.Parameters.Select(parameter => new
                {
                    name = parameter.Name,
                    value = parameter.Value,
                    type = parameter.Type.ToString()
                }),
                // ToString() here to have the method as a nice string otherwise it will just show the enum value
                method = request.Method.ToString(),
                // This will generate the actual Uri used in the request
                uri = restClient.BuildUri(request),
            };

            var responseToLog = new
            {
                statusCode = response.StatusCode,
                content = response.Content,
                headers = response.Headers,
                // The Uri that actually responded (could be different from the requestUri if a redirection occurred)
                responseUri = response.ResponseUri,
                errorMessage = response.ErrorMessage,
            };

            Console.Write(
                string.Format(
                    "Request: {0}, Response: {1}",
                    JsonConvert.SerializeObject(requestToLog),
                    JsonConvert.SerializeObject(responseToLog)
                )
            );
        }
    }
}
