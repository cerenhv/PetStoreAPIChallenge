using NUnit.Framework;
using PetStoreAPIChallenge.Entities;
using PetStoreAPIChallenge.HelperClass;
using PetStoreAPIChallenge.Models.PetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PetStoreAPIChallenge.Tests
{
    [TestFixture]
    public class StoreTests
    {
        private APIHelper api;
        private DataPrepHelper dataPrep;
        public int internalOrderId;

        [OneTimeSetUp]
        public void SetupStore()
        {
            api = new APIHelper();
            dataPrep = new DataPrepHelper();

            internalOrderId = dataPrep.findInexistentPetID(api);
            dataPrep.CreateOrderForInternalUsage(api, internalOrderId);
        }

        #region Get Method Tests

        [Test]
        public void GetStoreInventory()
        {
            var request = new RestSharp.RestRequest();
            var client = api.SetUrl("store/inventory");
            request = api.CreateGetRequest();
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void GetOrderWithIDSuccessfully()
        {
            var request = new RestSharp.RestRequest();
            var client = api.SetUrl("store/order");
            request = api.CreateGetRequestWithID(8);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [TestCase(0, HttpStatusCode.BadRequest, TestName = "Id must be greater than 0")]
        [TestCase(11, HttpStatusCode.BadRequest, TestName = "Id must be less than 11")]
        public void GetOrderWithID_NegativeCases(int Id, HttpStatusCode expectedStatusCode)
        {
            var request = new RestSharp.RestRequest();
            var client = api.SetUrl("store/order");
            request = api.CreateGetRequestWithID(Id);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
        }

        #endregion

        #region Post Method Tests

        [Test]
        public void CreateOrderSuccessfully()
        {
            var order = new Order()
            {
                Id = dataPrep.findInexistentOrderID(api),
                petId = 85,
                quantity = 1,
                shipDate = DateTime.Now,
                status = "placed",
                complete = false
            };

            var client = api.SetUrl("store/order");
            var request = api.CreatePostRequest(order);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CreateOrder_InvalidData()
        {
            var client = api.SetUrl("store/order");
            var request = api.CreatePostRequest(String.Empty);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        #endregion

        #region Delete Method Tests

        [Test]
        public void DeleteExistingOrderSuccessfully()
        {
            var client = api.SetUrl("store/order");
            var request = api.CreateDeleteRequest(internalOrderId);
            var response = api.GetResponse(client, request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestCase(10000, HttpStatusCode.NotFound, TestName = "Delete order with inexistent Id")]
        [TestCase(-55, HttpStatusCode.BadRequest, TestName = "Delete order with negative Id")]
        [TestCase("tst1", HttpStatusCode.BadRequest, TestName = "Delete order with non-integer Id")]
        public void DeleteOrder_NegativeCases(Object id, HttpStatusCode expectedStatusCode)
        {
            var client = api.SetUrl("store/order");
            var request = api.CreateDeleteRequest(id);
            var response = api.GetResponse(client, request);

            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }

        #endregion

    }
}
