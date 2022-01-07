// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AzureMapUpdater
{
    public static class AzureMapUpdater
    {
        private static string statesetID = Environment.GetEnvironmentVariable("statesetid");
        private static string subscriptionKey = Environment.GetEnvironmentVariable("subscriptionkey");

        [FunctionName("AzureMapUpdater")]
        public static void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            JObject message = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());

            string twinId = eventGridEvent.Subject;
            string modelId = message["data"]["modelId"].ToString();

            //Parse updates to "space" twins
            if (modelId == "dtmi:com:smartbuilding:Sensor;1")
            {
                // Iterate through the properties that have changed
                foreach (var operation in message["data"]["patch"])
                {
                    if (operation["op"].ToString() == "replace" && operation["path"].ToString() == "/temperature")
                    {
                        string value = operation["value"].ToString();

                        log.LogInformation($"AZUREMAP-RECEIVED twinId:{twinId} modelId:{modelId} temperaturevalue:{value}");

                        // Update the maps feature stateset
                        UpdateMapStateset(twinId, value, log);

                    }
                }
            }
        }

        static async void UpdateMapStateset(string featureId, string value, ILogger log)
        {
            HttpClient httpClient = new HttpClient();

            log.LogInformation($"AZUREMAP-START");

            try
            {

                var postcontent = new JObject(
                    new JProperty(
                        "States",
                        new JArray(
                            new JObject(
                                new JProperty("keyName", "temperature"),
                                new JProperty("value", value),
                                new JProperty("eventTimestamp", DateTime.UtcNow.ToString("s"))))));

                log.LogInformation($"AZUREMAP-VALUES FeatureId:{featureId} Value:{value} Subscription-key:{subscriptionKey}");

                // think it need to be a putasync
                //var response = await httpClient.PostAsync(
                var response = await httpClient.PutAsync(

                $"https://eu.atlas.microsoft.com/featurestatesets/{statesetID}/featureStates/{featureId}?api-version=2.0&subscription-key={subscriptionKey}",
                    new StringContent(postcontent.ToString()));

                string result = await response.Content.ReadAsStringAsync();

                log.LogInformation($"AZUREMAP-RESULT:{result}");
            }
            catch(Exception ex)
            {
                log.LogInformation($"AZUREMAP-UPDATEERROR error:{ex.Message}");
            }

            log.LogInformation($"AZUREMAP-END");
        }

    }
}
