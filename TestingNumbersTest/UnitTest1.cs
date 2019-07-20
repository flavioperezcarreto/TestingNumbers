using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using RestSharp;
using System;
using System.Net;

namespace TestingNumbersTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly string UrlBase;
        private readonly string AppKey;
        private readonly string SumFunctionName;

        public UnitTest1()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("jsconfig.json")
                .Build();

            UrlBase = config["urlBase"];
            AppKey = config["functionAppKey"];
            SumFunctionName = config["testNumberFunctionName"];
        }

        [TestMethod]
        public void Get_WithNum1AndNum2Undefined_ErrorResponse()
        {
            var client = MakeClient();
            var request = MakeSumRequest();
            var response = client.Get(request);

            Assert.AreEqual("Please pass an integer num1 on the query string or in the request body", response.Content);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
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
