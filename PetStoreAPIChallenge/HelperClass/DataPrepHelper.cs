using PetStoreAPIChallenge.Entities;
using PetStoreAPIChallenge.Models.PetModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetStoreAPIChallenge.HelperClass
{
    public class DataPrepHelper
    {
        public int findInexistentPetID(APIHelper api)
        {
            Random rid = new Random();
            int Id;
            bool continuee = false;

            do
            {
                Id = rid.Next(1000000, 9999999);
                var clientGet = api.SetUrl("pet");
                var requestGet = api.CreateGetRequestWithID(Id);
                var responseGet = api.GetResponse(clientGet, requestGet);

                if (responseGet.StatusCode == HttpStatusCode.OK)
                    continuee = false;
                else continuee = true;
            }
            while (!continuee);

            return Id;
        }

        public RestResponse CreatePetForInternalUsage(APIHelper api,int Id)
        {
            var pet = new Pet()
            {
                Id = Id,
                Name = "Test Dog Internal",
                Status = PetStatus.available.ToString(),
            };

            var client = api.SetUrl("pet");
            var request = api.CreatePostRequest(pet);
            var response = api.GetResponse(client, request);

            return response;
        }

        public int findInexistentOrderID(APIHelper api)
        {
            Random rid = new Random();
            int Id;
            bool continuee = false;

            do
            {
                Id = rid.Next(1, 1000);
                var clientGet = api.SetUrl("store/order");
                var requestGet = api.CreateGetRequestWithID(Id);
                var responseGet = api.GetResponse(clientGet, requestGet);

                if (responseGet.StatusCode == HttpStatusCode.OK)
                    continuee = false;
                else continuee = true;
            }
            while (!continuee);

            return Id;
        }

        public RestResponse CreateOrderForInternalUsage(APIHelper api, int Id)
        {
            var order = new Order()
            {
                Id = Id,
                petId = 85,
                quantity = 1,
                shipDate = DateTime.Now,
                status = "placed",
                complete = false
            };

            var client = api.SetUrl("store/order");
            var request = api.CreatePostRequest(order);
            var response = api.GetResponse(client, request);

            return response;
        }
    }
}
