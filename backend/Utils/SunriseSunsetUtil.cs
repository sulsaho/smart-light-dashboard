using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace LightWebAPI.Utils
{
    public static class Utility
    {
        /** Path to the sunrise and sunset feature JSON object */
        private static readonly string Path = $"{Environment.CurrentDirectory}/data.json";
        /** Latitude of Fargo city*/
        private static readonly float LATITUDE = 46.877229f;
        /** Longitude of Fargo city*/
        private static readonly float LONGITUDE = -96.789821f;
        /** URL for the sunrise and sunset API */
        private static readonly string SUNRISE_SUNSET_API = "https://api.sunrise-sunset.org/json";
        
        /**
         * Retrieves the sunrise and sunset times 
         */
        public static JObject GetSunriseSunset()
        {
            dynamic sunriseSunsetObject = new JObject();
            
            try
            {
                var client = new RestClient(SUNRISE_SUNSET_API);
                var request = new RestRequest(Method.GET);
                request.AddParameter("lat", LATITUDE);
                request.AddParameter("lng", LONGITUDE);
                request.AddParameter("date", "today");
                
                var response = client.Execute(request);

                var parsedJson = JObject.Parse(response.Content);
                sunriseSunsetObject.srss_feature = new JObject();
                sunriseSunsetObject.srss_feature.sunrise = ConvertToDateTime(parsedJson["results"]?["sunrise"]?.ToString());
                sunriseSunsetObject.srss_feature.sunset = ConvertToDateTime(parsedJson["results"]?["sunset"]?.ToString());
            }
            catch(Exception e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return sunriseSunsetObject;
        }
 
        /**
         * Enables the sunrise sunset feature by updating
         * the srss JSON blob with a variable, enable to true
         * or false
         */
        public static JObject EnableSunriseSunsetFeature(bool enable)
        {
            dynamic sunriseSunsetObject = new JObject();
            
            try
            {
                sunriseSunsetObject = GetSunriseSunset();
                sunriseSunsetObject.srss_feature.enabled = enable;
                
                using (var streamWriter = new StreamWriter(Path))
                {
                    streamWriter.Write(sunriseSunsetObject.ToString());
                    streamWriter.Close();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return sunriseSunsetObject;
        }

        /**
         * Retrieve srss JSON object from stored file
         */
        public static JObject GetData()
        {
            if (!File.Exists(GetDataJsonPath())) return new JObject();
            
            StreamReader readJson = new StreamReader(GetDataJsonPath());
            string jsonString = readJson.ReadToEnd();
            return JObject.Parse(jsonString);
        }
        
        /**
         * Converts JSON string to date time object
         * in America/Chicago timezone
         */
        public static string ConvertToDateTime(string stringValue)
        {
            var getDate = DateTime.ParseExact(stringValue, "h:mm:ss tt", CultureInfo.InvariantCulture);
            TimeZoneInfo targetZone = TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");
            return TimeZoneInfo.ConvertTimeFromUtc(getDate, targetZone).ToString("h:mm:ss tt");
        }

        public static string GetDataJsonPath()
        {
            return Path;
        }
    }
}