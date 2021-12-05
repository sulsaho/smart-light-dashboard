using System.IO;
using Newtonsoft.Json.Linq;

namespace LightWebAPI.Utils
{
    public static class JsonUtil
    {
        public static string GetApiKey()
        {
            using StreamReader r = new StreamReader("../backend/configuration.json");
            var json = r.ReadToEnd();
            var config = JObject.Parse(json);
            var key = config["configuration"]?["api_key"]?.ToString();
            return key != null ? key.ToString() : "";
        }

        public static string GetHour()
        {
            using StreamReader r = new StreamReader("../backend/TimeDate.json");
            var json = r.ReadToEnd();
            var time = JObject.Parse(json);
            var hour = time["hour"]?.ToString();
            return hour;
        }
        public static string GetMinutes()
        {
            using StreamReader r = new StreamReader("../backend/TimeDate.json");
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