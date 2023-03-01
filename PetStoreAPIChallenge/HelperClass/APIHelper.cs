using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStoreAPIChallenge.HelperClass
{
    public class APIHelper
    {
        public RestClient restClient;
        public RestRequest restRequest;

        public string baseUrl = "https://petstore.swagger.io/v2/";

        public RestClient SetUrl(string endpoint)
        {
            string url = Path.Combine(baseUrl + endpoint);
            restClient = new RestClient(url);
            return restClient;
        }

        #region Get Requests
        public RestRequest CreateGetRequest()
        {
            var restRequest = new RestRequest("", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            return restRequest;
        }

        public RestRequest CreateGetRequestWithParameters(string paramName, string[] values)
        {
            var restRequest = new RestRequest("", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            foreach (string s in values)
            {
                restRequest.AddParameter(paramName, s, ParameterType.QueryString);
            }

            return restRequest;
        }

        public RestRequest CreateGetRequestWithID(Object Id)
        {
            var restRequest = new RestRequest(Convert.ToString(Id), Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            return restRequest;
        }
        #endregion

        #region Post Requests

        public RestRequest CreatePostRequest(Object Payload)
        {
            var restRequest = new RestRequest("", Method.Post);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddParameter("application/json", Payload, ParameterType.RequestBody);
            return restRequest;
        }

       
        #endregion

        #region Put Requests

        public RestRequest CreatePutRequest(Object Payload)
        {
            var restRequest = new RestRequest("", Method.Put);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddParameter("application/json", Payload, ParameterType.RequestBody);
            return restRequest;
        }

        #endregion

        #region Delete Requests

        public RestRequest CreateDeleteRequest(Object Id)
        {
            var restRequest = new RestRequest(Convert.ToString(Id), Method.Delete);
            restRequest.AddHeader("Accept", "application/json");
            return restRequest;
        }
        #endregion

        #region Responses
        public RestResponse GetResponse(RestClient client, RestRequest request)
        {
            return client.Execute(request);
        }

        public Entities GetResponseContent<Entities>(RestResponse response)
        {
            var content = response.Content;
            Entities entitiesObject = JsonConvert.DeserializeObject<Entities>(content);
            return entitiesObject;
        }
        #endregion

    }
}
