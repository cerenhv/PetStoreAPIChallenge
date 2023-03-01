using NUnit.Framework;
using PetStoreAPIChallenge.HelperClass;
using PetStoreAPIChallenge.Models.PetModels;
using PetStoreAPIChallenge.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;

namespace PetStoreAPIChallenge.Tests
{
    [TestFixture]
    public class PetTests
    {
        private APIHelper api;
        private DataPrepHelper dataPrep;
        private PetApiHelper petApi;
        public int internalPetId;

        [OneTimeSetUp]
        public void SetupPet()
        {
            api = new APIHelper();
            dataPrep = new DataPrepHelper();
            petApi = new PetApiHelper();

            internalPetId = dataPrep.findInexistentPetID(api);
            dataPrep.CreatePetForInternalUsage(api, internalPetId);
        }

        #region Get Method Tests

        [TestCase(new string[] { "available", "pending", "sold" }, HttpStatusCode.OK, TestName = "Get records for all status")]
        [TestCase(new string[] { "qwerty" }, HttpStatusCode.BadRequest, TestName = "Get records with invalid status")]
        public void GetPetsFindByStatus(string[] statusList, HttpStatusCode expectedHttpStatusCode)
        {
            var request = new RestSharp.RestRequest();
            var client = api.SetUrl("pet/findByStatus");

            request = api.CreateGetRequestWithParameters("status", statusList);

            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
        }

        [TestCase(10000, HttpStatusCode.NotFound, TestName = "Pet not found")]
        [TestCase(-5, HttpStatusCode.BadRequest, TestName = "Get Pet_Invalid ID")]
        public void GetPetWithId_NegativeCases(Object Id, HttpStatusCode expectedHttpStatusCode)
        {
            var client = api.SetUrl("pet");
            var request = api.CreateGetRequestWithID(Id);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
        }

        [Test]
        public void GetPetWithId()
        {
            var client = api.SetUrl("pet");
            var request = api.CreateGetRequestWithID(internalPetId);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        #endregion

        #region Post Method Tests

        [Test]
        public void CreatePetSuccessfully()
        {
            var pet = new Pet()
            {
                Id = dataPrep.findInexistentPetID(api),
                Name = "Test Dog",
                Status = PetStatus.available.ToString(),
            };

            var client = api.SetUrl("pet");
            var request = api.CreatePostRequest(pet);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CreatePet_InvalidData()
        {
            var client = api.SetUrl("pet");
            var request = api.CreatePostRequest(String.Empty);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public void PostWithPetIdSuccessfully_withoutFormdata()
        {
            var client = api.SetUrl("pet");
            var request = petApi.CreatePostRequestwithFormdata(internalPetId, null, null, null, null);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void PostWithPetIdSuccessfully_withFormdata()
        {
            var client = api.SetUrl("pet");
            var request = petApi.CreatePostRequestwithFormdata(internalPetId, "test", "sold", "name", "status");
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [TestCase(10000, HttpStatusCode.NotFound, TestName = "PostWithPetId_PetId not found")]
        [TestCase(-55, HttpStatusCode.BadRequest, TestName = "PostWithPetId_PetId invalid")]
        public void PostWithPetId_NegativeCases(Object id, HttpStatusCode expectedHttpStatusCode)
        {
            var client = api.SetUrl("pet");
            var request = petApi.CreatePostRequestwithFormdata(id, null, null, null, null);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
        }

        [Test]
        public void PostPetUploadImageSuccessfully()
        {
            Image img = Resources.dogImage;

            var client = api.SetUrl("pet");
            var request = petApi.CreatePostRequest_UploadImage(internalPetId, "test metadata", img);
            var response = api.GetResponse(client, request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void PostPetUploadImageSuccessfully_withoutFormdata()
        {
            var client = api.SetUrl("pet");
            var request = petApi.CreatePostRequest_UploadImage(internalPetId, null, null);
            var response = api.GetResponse(client, request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestCase(10000, HttpStatusCode.NotFound, TestName = "UploadImage with inexistent Id")]
        [TestCase(-55, HttpStatusCode.BadRequest, TestName = "UploadImage with invalid Id")]
        public void PostPetUploadImage_NegativeCases(Object id, HttpStatusCode expectedHttpStatusCode)
        {
            var client = api.SetUrl("pet");
            var request = petApi.CreatePostRequest_UploadImage(id, null, null);
            var response = api.GetResponse(client, request);

            Assert.AreEqual(expectedHttpStatusCode, response.StatusCode);
        }

        #endregion

        #region Put Method Tests

        [Test]
        public void UpdatePetSuccessfully()
        {
            List<string> list = new List<string>();
            list.Add("C:\\dog.jpeg");

            var pet = new Pet()
            {
                Id = internalPetId,
                Name = "Test Dog",
                PhotoUrls = list,
                Status = PetStatus.sold.ToString(),
            };

            var client = api.SetUrl("pet");
            var request = api.CreatePutRequest(pet);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void UpdatePet_InvalidData()
        {
            var client = api.SetUrl("pet");
            var request = api.CreatePutRequest(String.Empty);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));
        }

        [Test]
        public void UpdatePetInexistentId()
        {
            List<string> list = new List<string>();
            list.Add("C:\\dog.jpeg");

            var pet = new Pet()
            {
                Id = 1054345230,
                Name = "Test Dog",
                PhotoUrls = list,
                Status = PetStatus.sold.ToString(),
            };

            var client = api.SetUrl("pet");
            var request = api.CreatePutRequest(pet);
            var response = api.GetResponse(client, request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        #endregion

        #region Delete Method Tests

        [Test]
        public void DeleteExistingPetSuccessfully()
        {
            int Id = dataPrep.findInexistentPetID(api);
            var postResponse = dataPrep.CreatePetForInternalUsage(api, Id);

            if (postResponse.StatusCode == HttpStatusCode.OK)
            {
                var client = api.SetUrl("pet");
                var request = api.CreateDeleteRequest(Id);
                var response = api.GetResponse(client, request);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
            else Assert.Fail("Delete operation failed.");
        }

        [TestCase(10000, HttpStatusCode.NotFound, TestName ="Delete pet with inexistent Id")]
        [TestCase("testt", HttpStatusCode.BadRequest, TestName = "Delete pet with invalid Id")]
        public void DeletePet_NegativeCases(Object id, HttpStatusCode expectedStatusCode)
        {
            var client = api.SetUrl("pet");
            var request=api.CreateDeleteRequest(id);
            var response= api.GetResponse(client, request);

            Assert.AreEqual(expectedStatusCode, response.StatusCode);
        }

        #endregion
    }
}
