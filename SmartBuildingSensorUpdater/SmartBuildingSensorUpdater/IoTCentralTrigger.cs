using System;
using System.Net.Http;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartBuildingConsoleApp.DigitalTwins;

namespace SmartBuildingSensorUpdater
{
    public static class IoTCentralTrigger
    {
        const string adtAppId = "https://digitaltwins.azure.net";
        const string queueName = "iotcentral";

        [FunctionName("IoTCentralTrigger")]
        public async static void Run([ServiceBusTrigger(queueName, Connection = "ServiceBusConnection")] Message message, ILogger log)
        {
            /// probably iotcentral-device-id
            string sensorId = message.UserProperties["iotcentral-device-id"].ToString();

            string value = Encoding.ASCII.GetString(message.Body, 0, message.Body.Length);
            var bodyProperty = (JObject)JsonConvert.DeserializeObject(value);

            JToken temperatureToken = bodyProperty["telemetry"]["temperature"];

            float temperature = temperatureToken.Value<float>();

            log.LogInformation(string.Format("Sensor Id:{0}", sensorId));
            log.LogInformation(string.Format("Sensor Temperature:{0}", temperature));

            DigitalTwinsManager manager = new DigitalTwinsManager(adtAppId);
            manager.UpdateDigitalTwinProperty(sensorId, "temperature", temperature);
        }
    }
}
