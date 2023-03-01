using RestSharp;
using System;
using PetStoreAPIChallenge.Properties;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

namespace PetStoreAPIChallenge.HelperClass
{
    public class PetApiHelper
    {
        public RestRequest restRequest;

        public RestRequest CreatePostRequestwithFormdata(Object Id, Object formdata1, Object formdata2, string prm1, string prm2)
        {
            var restRequest = new RestRequest(Convert.ToString(Id), Method.Post);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            if (formdata1 != null && prm1 != null)
                restRequest.AddParameter(prm1, formdata1, ParameterType.RequestBody);
            if (formdata2 != null && prm2 != null)
                restRequest.AddParameter(prm2, formdata2, ParameterType.RequestBody);
            return restRequest;
        }

        public RestRequest CreatePostRequest_UploadImage(Object Id, Object additionalMetadata, Object image)
        {
            ResourceManager rm = Resources.ResourceManager;
            var filePath = System.Reflection.Assembly.GetExecutingAssembly().Location.Split(new string[] { "bin" }, StringSplitOptions.None)[0] + @"\Resources\dog.jpeg"; ;

            var restRequest = new RestRequest(Convert.ToString(Id)+ "/uploadImage", Method.Post);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            if (additionalMetadata != null)
                restRequest.AddParameter("additionalMetadata", additionalMetadata, ParameterType.RequestBody);
            if (image != null)             
                restRequest.AddFile("file",filePath, "image/jpeg");
            return restRequest;
        }
    }
}
