using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TestingNumbers
{
    public static class Function1
    {
        [FunctionName("SumTwoIntegers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string number1 = req.Query["num1"];
            string number2 = req.Query["num2"];

            int? int1 = StringToNullableInt(number1);
            int? int2 = StringToNullableInt(number2);

            if (int1 == null)
            {
                log.LogInformation("Returning bad request for number 1.");
                return new BadRequestObjectResult("Please pass an integer num1 on the query string or in the request body");
            }

            if (int2 == null)
            {
                log.LogInformation("Returning bad request for number 2.");
                return new BadRequestObjectResult("Please pass an integer num2 on the query string or in the request body");
            }

            var sum = int1 + int2;
            log.LogInformation($"Returning Ok with the value of {sum} for inputs {number1} and {number2}");

            return new OkObjectResult($"Result: {sum}");
        }

        private static int? StringToNullableInt(string input)
        {
            int outValue;
            return int.TryParse(input, out outValue) ? (int?)outValue : null;
        }
    }
    
}
