using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace LightWebAPI.Utils
{
    public class Utility
    {
        private static readonly string Path = $"{Environment.CurrentDirectory}/data.json";
        private readonly double LATITUDE = 46.8564;
        private readonly double LONGITUDE = -96.8123;
        private readonly string SUNRISE_SUNSET_API = "https://api.sunrise-sunset.org/json";
        
        public JObject GetSunriseSunset()
        {
            dynamic sunriseSunsetObject = new JObject();

            try
            {
                var client = new RestClient(SUNRISE_SUNSET_API);
                var request = new RestRequest(Method.GET);
                request.AddParameter("lat", LATITUDE);
                request.AddParameter("long", LONGITUDE);
                request.AddParameter("date", "today");
                
                var response = client.Execute(request);
                
                var parsedJson = JObject.Parse(response.Content);
                sunriseSunsetObject.srss_feature = new JObject();
                sunriseSunsetObject.srss_feature.sunrise = parsedJson["results"]?["sunrise"];
                sunriseSunsetObject.srss_feature.sunset = parsedJson["results"]?["sunset"];
            }
            catch(Exception e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return sunriseSunsetObject;
        }

        public JObject EnableSunriseSunsetFeature(bool enable)
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

        public JObject GetData()
        {
            if (!File.Exists(GetDataJsonPath())) return new JObject();
            
            StreamReader readJson = new StreamReader(GetDataJsonPath());
            string jsonString = readJson.ReadToEnd();
            return JObject.Parse(jsonString);
        }
        
        public DateTime ConvertToDateTime(string stringValue)
        {
            return DateTime.ParseExact(stringValue, "h:mm:ss tt", CultureInfo.InvariantCulture);
        }

        public static string GetDataJsonPath()
        {
            return Path;
        }
    }
}