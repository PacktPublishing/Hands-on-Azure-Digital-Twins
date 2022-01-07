using Azure;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SmartBuildingConsoleApp.DigitalTwins
{
    public class DigitalTwinsManager
    {
        private static readonly string adtInstanceUrl = "https://DTBDigitalTwins.api.weu.digitaltwins.azure.net";

        private DigitalTwinsClient client;

        public DigitalTwinsManager()
        {
            Connect();
        }

        #region Connect and authentication
        public void Connect()
        {
            var cred = new DefaultAzureCredential();
            client = new DigitalTwinsClient(new Uri(adtInstanceUrl), cred);
        }
        #endregion

        #region Models
        public bool CreateModel(string path)
        {
            return CreateModels(new string[] { path });
        }

        public bool CreateModels(string[] path)
        {
            List<string> dtdls = new List<string>();

            foreach (string p in path)
            {
                using var modelStreamReader = new StreamReader(p);
                string dtdl = modelStreamReader.ReadToEnd();

                dtdls.Add(dtdl);
            }

            try
            {
                DigitalTwinsModelData[] models = client.CreateModels(dtdls.ToArray());
            }
            catch (RequestFailedException)
            {
                return false;
            }

            return true;
        }

        public void DeleteModel(string modelId)
        {
            try
            {
                client.DeleteModel(modelId);
            }
            catch (RequestFailedException)
            {

            }
        }

        public DigitalTwinsModelData GetModel(string modelId)
        {
            try
            {
                return client.GetModel(modelId);
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public Pageable<DigitalTwinsModelData> GetModels()
        {
            GetModelsOptions options = new GetModelsOptions();

            return client.GetModels(options);
        }
        #endregion

        #region Digital Twins
        public bool CreateDigitalTwin(string twinId, string modelId)
        {
            BasicDigitalTwin digitalTwin = new BasicDigitalTwin();
            digitalTwin.Metadata = new DigitalTwinMetadata();
            digitalTwin.Metadata.ModelId = modelId;
            digitalTwin.Id = twinId;

            try
            {
                client.CreateOrReplaceDigitalTwin<BasicDigitalTwin>(twinId, digitalTwin);
            }
            catch (RequestFailedException)
            {
                return false;
            }

            return true;
        }

        public bool DeleteDigitalTwin(string twinId)
        {
            try
            {
                client.DeleteDigitalTwin(twinId);
            }
            catch (RequestFailedException)
            {
                return false;
            }

            return true;
        }

        public BasicDigitalTwin GetDigitalTwin(string twinId)
        {
            try
            {
                return client.GetDigitalTwin<BasicDigitalTwin>(twinId);
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public bool UpdateDigitalTwin(string twinId, string property, object value)
        {
            try
            {
                BasicDigitalTwin digitalTwin = client.GetDigitalTwin<BasicDigitalTwin>(twinId);
                digitalTwin.Contents[property] = value;
                client.CreateOrReplaceDigitalTwin<BasicDigitalTwin>(twinId, digitalTwin);
            }
            catch (RequestFailedException)
            {
                return false;
            }

            return true;
        }

        public void UpdateDigitalTwinProperty(string twinId, string property, object value)
        {
            JsonPatchDocument patch = null;
            try
            {
                patch = new JsonPatchDocument();
                patch.AppendAdd("/" + property, value);
                client.UpdateDigitalTwin(twinId, patch);
            }
            catch (RequestFailedException)
            {
            }

            patch = new JsonPatchDocument();
            patch.AppendReplace("/" + property, value);
            client.UpdateDigitalTwin(twinId, patch);
        }

        public bool UpdateDigitalTwin(string twinId, string property, Dictionary<string, object> map)
        {
            try
            {
                JsonPatchDocument patch = new JsonPatchDocument();
                patch.AppendReplace(property, map);

                client.UpdateDigitalTwin(twinId, patch);
            }
            catch (RequestFailedException)
            {
                return false;
            }

            return true;
        }


        #endregion

        #region Relationships
        public string RelationshipId(string twinSourceId, string twinDestinationId)
        {
            return string.Format("{0}-{1}", twinSourceId, twinDestinationId);
        }

        public void CreateRelationship(string twinSourceId, string twinDestinationId, string description, Dictionary<string, object> properties = null)
        {
            string relationShipId = RelationshipId(twinSourceId, twinDestinationId);

            BasicRelationship relationship = new BasicRelationship
            {
                Id = "buildingFloorRelationshipId",
                SourceId = twinSourceId,
                TargetId = twinDestinationId,
                Name = description,
                Properties = properties
            };

            try
            {
                client.CreateOrReplaceRelationship(twinSourceId, relationShipId, relationship);
            }
            catch (RequestFailedException)
            {
            }
        }

        public void DeleteRelationship(string twinSourceId, string twinDestinationId)
        {
            string relationShipId = RelationshipId(twinSourceId, twinDestinationId);

            try
            {
                client.DeleteRelationship(twinSourceId, relationShipId);
            }
            catch (RequestFailedException)
            {
            }
        }

        public BasicRelationship GetRelationship(string twinSourceId, string twinDestinationId)
        {
            string relationShipId = RelationshipId(twinSourceId, twinDestinationId);

            try
            {
                Response<BasicRelationship> relationship = client.GetRelationship<BasicRelationship>(twinSourceId, relationShipId);

                return relationship.Value;
            }
            catch (RequestFailedException)
            {
            }

            return null;
        }

        public Pageable<BasicRelationship> ListRelationships(string twinSourceId)
        {
            try
            {
                Pageable<BasicRelationship> relationships = client.GetRelationships<BasicRelationship>(twinSourceId);

                return relationships;
            }
            catch (RequestFailedException)
            {
            }

            return null;
        }

        public void UpdateRelationship(string twinSourceId, string twinDestinationId, string property, object value)
        {
            string relationShipId = RelationshipId(twinSourceId, twinDestinationId);

            JsonPatchDocument patch = null;
            try
            {
                patch = new JsonPatchDocument();
                patch.AppendReplace("/" + property, value);

                client.UpdateRelationship(twinSourceId, relationShipId, patch);
            }
            catch (RequestFailedException)
            {
            }
        }
        #endregion

        #region Global
        public void ClearAll(string twinId)
        {
            Pageable<BasicRelationship> relationships = ListRelationships(twinId);

            foreach(BasicRelationship relationship in relationships)
            {
                DeleteRelationship(relationship.SourceId, relationship.TargetId);
            }

            Pageable<BasicDigitalTwin> digitalTwins = QueryDigitalTwins("SELECT * FROM DIGITALTWINS");

            foreach(BasicDigitalTwin digitalTwin in digitalTwins)
            {
                DeleteDigitalTwin(digitalTwin.Id);
            }
        }
        #endregion

        #region Queries

        public Pageable<Dictionary<string, BasicDigitalTwin>> Query(string query)
        {
            Pageable<Dictionary<string, BasicDigitalTwin>> result = null;

            try
            {
                result = client.Query<Dictionary<string, BasicDigitalTwin>>(query);
            }
            catch(RequestFailedException)
            {

            }

            return result;
        }

        public Pageable<BasicDigitalTwin> QueryDigitalTwins(string query)
        {
            Pageable<BasicDigitalTwin> result = null;
            try
            {
                result = client.Query<BasicDigitalTwin>(query);
            }
            catch (RequestFailedException)
            {
            }

            return result;
        }

        public delegate void QueryResult(BasicDigitalTwin dt);

        public void QueryDigitalTwins(string query, QueryResult onQueryResult)
        {
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Run(
                () => QueryDigitalTwinsAsync(query, onQueryResult));
        }

        public async void QueryDigitalTwinsAsync(string query, QueryResult onQueryResult)
        {
            AsyncPageable<BasicDigitalTwin> result = client.QueryAsync<BasicDigitalTwin>(query);
            try
            {
                await foreach (BasicDigitalTwin dt in result)
                {
                    onQueryResult(dt);
                }
            }
            catch (RequestFailedException)
            {
            }
        }

        #endregion
    }
}
