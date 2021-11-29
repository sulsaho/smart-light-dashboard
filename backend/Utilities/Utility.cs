using System;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using LightWebAPI.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using RestSharp;

namespace LightWebAPI.Utilities
{
    public class Utility
    {
        private readonly double LATITUDE = 46.877229;
        private readonly double LONGITUDE = -96.789821;
        private readonly string SUNRISE_SUNSET_API = "https://api.sunrise-sunset.org/json";
        
        public SunriseSunset GetSunriseSunset()
        {
            var sunriseSunsetObject = new SunriseSunset();
            try
            {
                var client = new RestClient(SUNRISE_SUNSET_API);
                var request = new RestRequest(Method.GET);
                request.AddParameter("lat", LATITUDE);
                request.AddParameter("long", LONGITUDE);
                request.AddParameter("date", "today");
                
                var response = client.Execute(request);
                
                sunriseSunsetObject = JsonSerializer.Deserialize<SunriseSunset>(response.Content);

            }
            catch(Exception e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return sunriseSunsetObject;
        }

        public DateTime ConvertToDateTime(string stringValue)
        {
            return DateTime.ParseExact(stringValue, "h:mm:ss tt", CultureInfo.InvariantCulture);
        }
/*
        public static void ActivateTimer(DateTime from, DateTime to)
        {
            Timer _timer = new Timer( (e) =>
              {
                  OnTimer();
              }, null, from.TimeOfDay, to.TimeOfDay);
        }*/
    }

    public class SunriseSunset
    {        
        public Result results { get; set; }
    }
    
    public class Result
    {
        [JsonPropertyName("sunrise")]
        public String Sunrise { get; set; }
    
        [JsonPropertyName("sunset")]
        public String SunSet { get; set; }
    }
}