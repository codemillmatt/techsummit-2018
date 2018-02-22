using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Spatial;
using System.Net.Http.Headers;

using AwesomeContacts.SharedModels;
using BingMapsRESTToolkit;
using System;
using System.Linq;

namespace AwesomeContacts.Functions
{
    public static class UpdateGeolocation
    {
        static HttpClient httpClient = new HttpClient();

        static string cosmosEndpointUrl = Environment.GetEnvironmentVariable("CosmosEndpointUrl");
        static string cosmosAuthKey = Environment.GetEnvironmentVariable("CosmosAuthKey");
        static DocumentClient docClient = new DocumentClient(new Uri(cosmosEndpointUrl), cosmosAuthKey);

        [FunctionName("UpdateGeolocation")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"Webhook was triggered!");

            GraphInfo cdaInfo = null;

#if DEBUG
            cdaInfo = new GraphInfo { UserPrincipalName = "testuser@microsoft.com" };
#else
            cdaInfo = await GetCDAGraphInfo(req, log);

            if (cdaInfo == null)
            {
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error using the Graph");
            }
#endif

            string jsonContent = await req.Content.ReadAsStringAsync();
            LocationUpdate data = null;

            if (!string.IsNullOrWhiteSpace(jsonContent))
                data = JsonConvert.DeserializeObject<LocationUpdate>(jsonContent);
#if DEBUG
            else
                data = new LocationUpdate
                {
                    Position = new Microsoft.Azure.Documents.Spatial.Point(-122.130602, 47.6451599)
                };
#endif

            var key = System.Environment.GetEnvironmentVariable("BingMapsKey", EnvironmentVariableTarget.Process);

            //if we can't detect the city in app, let's figure it out!
            if (string.IsNullOrWhiteSpace(data.Country) || string.IsNullOrWhiteSpace(data.State) || string.IsNullOrWhiteSpace(data.Town))
            {
                var reverse = await ServiceManager.GetResponseAsync(new ReverseGeocodeRequest()
                {
                    BingMapsKey = key,
                    Point = new Coordinate(data.Position.Position.Latitude, data.Position.Position.Longitude),
                    IncludeEntityTypes = new System.Collections.Generic.List<EntityType>
                    {
                        EntityType.CountryRegion,
                        EntityType.Address,
                        EntityType.AdminDivision1
                    }
                });

                var result1 = reverse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault() as BingMapsRESTToolkit.Location;
                if (result1 != null)
                {
                    //update the city/state here
                    data.State = result1.Address.AdminDistrict;
                    data.Country = result1.Address.CountryRegion;
                    data.Town = result1.Address.Locality;
                }
            }

            //find the generic lat/long
            var r = await ServiceManager.GetResponseAsync(new GeocodeRequest()
            {
                BingMapsKey = key,
                Query = $"{data.Town}, {data.State} {data.Country}"
            });

            var result2 = r.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault() as BingMapsRESTToolkit.Location;

            var point = result2?.GeocodePoints?.FirstOrDefault()?.GetCoordinate();
            if (point != null)
            {
                data.Position = new Microsoft.Azure.Documents.Spatial.Point(point.Longitude, point.Latitude);
            }
            else
            {
                //blank out lat/long
                data.Position = new Microsoft.Azure.Documents.Spatial.Point(-122.1306030, 47.6451600);
            }

            //update cosmos DB here
            data.UserPrincipalName = cdaInfo.UserPrincipalName;
            data.InsertTime = DateTimeOffset.UtcNow;
            var collectionUri = UriFactory.CreateDocumentCollectionUri("CDALocations", "Location");
            var doc = await docClient.CreateDocumentAsync(collectionUri, data);

            return req.CreateResponse(HttpStatusCode.OK, new
            {
                greeting = $"Hello {data.UserPrincipalName}, you are in {data.Country} {data.Town}!"
            });

        }

        static async Task<GraphInfo> GetCDAGraphInfo(HttpRequestMessage req, TraceWriter log)
        {
            GraphInfo cdaInfo = null;

            try
            {
                // Using the passed in token from the mobile app (which will authenticate against the MSFT corp AD) - call the MS Graph to get user info
                if (!req.Headers.Authorization.Scheme.Equals("bearer", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(req.Headers.Authorization.Parameter))
                    return null;

                var token = req.Headers.Authorization.Parameter;

                var graphReqMsg = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
                graphReqMsg.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

                var graphResponse = await httpClient.SendAsync(graphReqMsg);

                cdaInfo = GraphInfo.FromJson(await graphResponse.Content.ReadAsStringAsync());
                if (string.IsNullOrWhiteSpace(cdaInfo.UserPrincipalName))
                    return null;

                log.Info(cdaInfo.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Graph HTTP call", ex);
                return null;
            }

            return cdaInfo;
        }
    }
}
