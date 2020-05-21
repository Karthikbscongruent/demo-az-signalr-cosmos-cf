using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.SignalR.Api
{
    public static class SignalRForwarderActivity
    {
        [FunctionName("SignalRForwarderActivity")]
        public static async Task Run([CosmosDBTrigger(databaseName: "Iot", collectionName: "Telemetry", ConnectionStringSetting = "CosmosDBConnectionString", LeaseCollectionName = "leases-signalr", CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input,
            [SignalR(HubName = "IotHub")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            log.LogInformation("SignalRForwarderActivity");
            if (input != null && input.Count > 0)
            {
                foreach (var document in input)
                {
                    var telemetry = JsonConvert.DeserializeObject<Telemetry>(document.ToString());
                    await signalRMessages.AddAsync(new SignalRMessage
                    {
                        Target = "telemetry",
                        Arguments = new[] { telemetry }
                    });
                }
            }
        }
    }
}
