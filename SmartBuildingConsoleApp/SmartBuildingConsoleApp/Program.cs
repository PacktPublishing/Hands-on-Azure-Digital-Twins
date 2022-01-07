using Azure;
using Azure.DigitalTwins.Core;
using SmartBuildingConsoleApp.DigitalTwins;
using SmartBuildingConsoleApp.Sensor;
using System;
using System.Collections.Generic;

namespace SmartBuildingConsoleApp
{
    class Program
    {
        public static void GenerateSensorData()
        {
            DigitalTwinsManager dtHelper = new DigitalTwinsManager();

            // generate sensor data
            TemperatureSensor sensor = new TemperatureSensor();
            Console.WriteLine("Temperature sensor");
            while (true)
            {
                double temperature = sensor.GetMeasurement();
                Console.WriteLine(string.Format("{0} degrees", temperature));

                dtHelper.UpdateDigitalTwinProperty("MeetingRoom1.01", "temperaturevalue", temperature);

                System.Threading.Thread.Sleep(1000);
            }
        }


        static void Main(string[] args)
        {
            DigitalTwinsManager dtHelper = new DigitalTwinsManager();

            //string[] paths = new string[] {
            ////    "Models/chapter4/campus.json",
            ////    "Models/chapter4/building.json",
            ////    "Models/chapter4/floor.json",
            ////    "Models/chapter4/room.json",
            ////    "Models/chapter4/workarea.json",
            //"Models/chapter4/meetingroom.json"//,
            //    "Models/chapter5/airconditioningsystemunit.json",
            //    "Models/chapter5/airconditioningsystemcontroller.json",
            //    "Models/chapter5/airconditioningsystem.json"
            ////    "Models/chapter4/sensor.json"
            //};
            //dtHelper.CreateModels(paths);
            //Console.WriteLine("Models created");

            //DigitalTwinsManager dtHelper = new DigitalTwinsManager();

            //string[] paths = new string[] {
            //    "Models/chapter6/building.json"
            //};

            //dtHelper.DeleteModel("dtmi:com:smartbuilding:Building;1");
            //dtHelper.CreateModels(paths);
            //Console.WriteLine("Model updated");



            //dtHelper.DeleteModel("dtmi:com:smartbuilding:Room;1");

            //DigitalTwinsModelData model = dtHelper.GetModel("dtmi:com:smartbuilding:Room;1");
            //Console.WriteLine(model.Id);
            //Console.WriteLine(model.LanguageDisplayNames["en"]);

            //var models = dtHelper.GetModels();
            //foreach (DigitalTwinsModelData model in models)
            //{
            //    Console.WriteLine(model.Id);
            //    Console.WriteLine(model.LanguageDisplayNames["en"]);
            //}

            //dtHelper.CreateDigitalTwin("Campus", "dtmi:com:smartbuilding:Campus;1");
            //dtHelper.CreateDigitalTwin("MainBuilding", "dtmi:com:smartbuilding:Building;1");
            //dtHelper.CreateDigitalTwin("GroundFloor", "dtmi:com:smartbuilding:Floor;1");
            //dtHelper.CreateDigitalTwin("MeetingRoom1.01", "dtmi:com:smartbuilding:Meetingroom;1");

            //dtHelper.UpdateDigitalTwin("MeetingRoom1.01", "occupied", true);

            //dtHelper.DeleteDigitalTwin("MeetingRoom1.01");

            //BasicDigitalTwin twin = dtHelper.GetDigitalTwin("MeetingRoom1.01");
            //bool occupied = false;
            //Boolean.TryParse(twin.Contents["occupied"].ToString(), out occupied);
            //if (occupied)
            //{
            //    Console.WriteLine("the meeting room is occupied");
            //}

            //DigitalTwinsManager dtHelper = new DigitalTwinsManager();

            //var map = new Dictionary<string, float>();
            //map.Add("MeetingRoom101", 21.3f);
            //map.Add("MeetingRoom102", 22.1f);
            //map.Add("MeetingRoom103", 20.9f);

            ////dtHelper.UpdateDigitalTwin("MainBuilding", "rooms", map);
            //dtHelper.UpdateDigitalTwinProperty("MainBuilding", "rooms", map);

            //DigitalTwinsManager dtHelper = new DigitalTwinsManager();

            //dtHelper.UpdateDigitalTwinProperty("GroundFloor", "floornumber", 1);
            //dtHelper.UpdateDigitalTwinProperty("GroundFloor", "lightson", true);


            // generate sensor data
            //System.Threading.Thread sensorThread = new System.Threading.Thread(GenerateSensorData);
            //sensorThread.Start();

            //DigitalTwinsManager dtHelper = new DigitalTwinsManager();

            //Dictionary<string, object> properties = new Dictionary<string, object>();
            //properties["level"] = 0;

            //dtHelper.CreateRelationship("MainBuilding", "GroundFloor", "has", null);
            //dtHelper.CreateRelationship("MainBuilding", "GroundFloor", "has", properties);
            ////dtHelper.DeleteRelationship("MainBuilding", "GroundFloor");

            //dtHelper.UpdateRelationship("MainBuilding", "GroundFloor", "level", 8);


            //BasicRelationship relationship = dtHelper.GetRelationship("MainBuilding", "GroundFloor");
            //if (relationship != null)
            //{
            //    Console.WriteLine(relationship.Id);
            //}

            //Pageable<BasicRelationship> relationships = dtHelper.ListRelationships("MainBuilding");

            //foreach (BasicRelationship relationship in relationships)
            //{
            //    Console.WriteLine(relationship.Id);

            //    foreach (string key in relationship.Properties.Keys)
            //    {
            //        Console.WriteLine(string.Format("{0}:{1}", key, relationship.Properties[key]));
            //    }
            //}

            //Pageable<BasicDigitalTwin> result = dtHelper.QueryDigitalTwins("SELECT * FROM DIGITALTWINS");

            //foreach (BasicDigitalTwin item in result)
            //{
            //    Console.WriteLine(item.Id);
            //}

            //Pageable<Dictionary<string, BasicDigitalTwin>> result = dtHelper.Query("SELECT BU,FL FROM DIGITALTWINS BU JOIN FL RELATED BU.has WHERE BU.$dtId='centralbuilding'");

            //foreach (Dictionary<string, BasicDigitalTwin> item in result)
            //{
            //    BasicDigitalTwin BU = item["BU"] as BasicDigitalTwin;
            //    BasicDigitalTwin FL = item["FL"] as BasicDigitalTwin;

            //    Console.WriteLine(string.Format("{0}-{1}", BU.Id, FL.Id));
            //}


            dtHelper.QueryDigitalTwins("SELECT * FROM DIGITALTWINS", OnQueryResult);

            while(true) { }
        }

        public static void OnQueryResult(BasicDigitalTwin dt)
        {
            Console.WriteLine(dt.Id);
        }
    }
}
