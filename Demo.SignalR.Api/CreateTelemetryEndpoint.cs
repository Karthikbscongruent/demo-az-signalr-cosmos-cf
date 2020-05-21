using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.SignalR.Api
{
    public static class CreateTelemetryEndpoint
    {
        [FunctionName("CreateTelemetryEndpoint")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "Iot", collectionName: "Telemetry", ConnectionStringSetting = "CosmosDBConnectionString", CreateIfNotExists = true)] IAsyncCollector<Telemetry> documents, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Telemetry>(requestBody);
            await documents.AddAsync(data);

            return new OkObjectResult("OK");
        }
    }
}
