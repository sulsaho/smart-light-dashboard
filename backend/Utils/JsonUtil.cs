using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR.Protocol;
using Newtonsoft.Json;
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
    }
}