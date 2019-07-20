using System;
using System.Net;

using Microsoft.Extensions.Configuration;
using NUnit.Framework;

using RestSharp;

namespace Tests
{
    public class Tests
    {
        private readonly string UrlBase;
        private readonly string AppKey;
        private readonly string SumFunctionName;

        public Tests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("jsconfig.json")
                .Build();

            UrlBase = config["urlBase"];
            AppKey = config["functionAppKey"];
            SumFunctionName = config["testNumberFunctionName"];
        }

        [Test]
        public void Post_WithNum1AndNum2Undefined_ErrorRespose()
        {
            var client = MakeClient();
            var request = MakeSumRequest();
            var response = client.Post(request);

            Assert.AreEqual("\"Please pass an integer num1 on the query string or in the request body\"", response.Content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void Get_WithNum1AndNum2Undefined_ErrorRespose()
        {
            var client = MakeClient();
            var request = MakeSumRequest();
            var response = client.Get(request);

            Assert.AreEqual("\"Please pass an integer num1 on the query string or in the request body\"", response.Content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void Post_WithNum1DefinedButNoNum2_ErrorRespose()
        {
            var client = MakeClient();
            var request = MakeSumRequest();
            request.AddQueryParameter("num1", "1");
            var response = client.Post(request);

            Assert.AreEqual("\"Please pass an integer num2 on the query string or in the request body\"", response.Content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public void Post_WithNum1AndNum2Defined_ReturnSumWhichIs3()
        {
            var client = MakeClient();
            var request = MakeSumRequest();
            request.AddQueryParameter("num1", "1");
            request.AddQueryParameter("num2", "2");
            var response = client.Post(request);

            Assert.AreEqual("\"Result: 3\"", response.Content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        private IRestClient MakeClient()
        {
            RestClient client = new RestClient(UrlBase);
            return client;
        }

        private IRestRequest MakeSumRequest()
        {
            RestRequest request = new RestRequest(SumFunctionName, Method.GET);
            request.AddQueryParameter("code", AppKey);
            return request;
        }
    }
}