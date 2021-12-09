using System.IO;
using Newtonsoft.Json.Linq;

namespace LightWebAPI.Utils
{
    public static class JsonUtil
    {
        private static readonly string Path = Directory.GetCurrentDirectory();
        
        /**
         * Functions retrieves the necessary API key
         * used to communicate with LifX
         */
        public static string GetApiKey()
        {
            using StreamReader r = new StreamReader($"{Path}/configuration.json");
            var json = r.ReadToEnd();
            var config = JObject.Parse(json);
            var key = config["configuration"]?["api_key"]?.ToString();
            return key != null ? key.ToString() : "";
        }

        /**
         * Function retrieves the hour stored in the date JSON blob
         */
        public static string GetHour()
        {
            using StreamReader r = new StreamReader($"{Path}/TimeDate.json");
            var json = r.ReadToEnd();
            var time = JObject.Parse(json);
            var hour = time["hour"]?.ToString();
            return hour;
        }
        
        /**
         * Function retrieves the minute stored in the date JSON blob
         */
        public static string GetMinutes()
        {
            using StreamReader r = new StreamReader($"{Path}/TimeDate.json");
            var json = r.ReadToEnd();
            var time = JObject.Parse(json);
            var minute = time["minute"]?.ToString();
            return minute;
        }
        
        public static string GetOnOff()
        {
            using StreamReader r = new StreamReader("../backend/TimeDate.json");
            var json = r.ReadToEnd();
            var time = JObject.Parse(json);
            var isOnOff = time["isOnOff"]?.ToString();
            return isOnOff;
        }
    }
}