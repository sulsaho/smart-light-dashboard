using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;
using LightWebAPI.Models;
using LightWebAPI.Repositories;
using LightWebAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace LightWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightStateController : ControllerBase
    {
        private readonly ILightStateRepository _lightStateRepository;
        private string LightToken = "Bearer ";

        public bool _IsScheduledON;
        public bool _IsScheduledOff;

        public LightStateController(ILightStateRepository lightStateRepository)
        {
            _lightStateRepository = lightStateRepository;
            var key = JsonUtil.GetApiKey();
            LightToken += key;
        }

        private bool IsLastEntrySameState(string currentState)
        {
            var onOrOff = (currentState == "on") ? true : false;
            var lightStates = GetLightStates().Result;
            var lastEntry = lightStates.LastOrDefault();
            return lastEntry != null && lastEntry.IsOn == onOrOff;
        }
        
        private bool IsLastEntrySameBrightness(string currentBrightness)
        {
            var lightStates = GetLightStates().Result;
            var lastEntry = lightStates.LastOrDefault();
            return lastEntry != null && lastEntry.Brightness == currentBrightness;
        }

        [HttpGet]
        public async Task<IEnumerable<LightState>> GetLightStates()
        {
            return await _lightStateRepository.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LightState>> GetLightStates(int id)
        {
            return await _lightStateRepository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<LightState>> PostLightStates([FromBody] LightState lightState)
        {
            var newLightState = await _lightStateRepository.Create(lightState);
            return CreatedAtAction(nameof(GetLightStates), new { id = newLightState.Id }, newLightState);
        }

        [HttpPut]
        public async Task<ActionResult> PutLightState(int id, [FromBody] LightState lightState)
        {
            if (id != lightState.Id)
            {
                return BadRequest();
            }
            else
            {
                await _lightStateRepository.Update(lightState);
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var lightStateToDelete = await _lightStateRepository.Get(id);
            if (lightStateToDelete == null)
            {
                return NotFound();
            }

            await _lightStateRepository.Delete(lightStateToDelete.Id);
            return NoContent();

        }

        private JObject GetState()
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", LightToken);
            var response = client.Execute(request);
            
            var json = JArray.Parse(response.Content);
            return JObject.Parse(json[0].ToString());

        }

        [HttpPost("light/state-check")]
        public async Task<string> AddState()
        {
            var lightResponse = GetState();

            var lightState = new LightState()
            {
                IsOn = (lightResponse["power"]?.ToString() == "on") ? true : false,
                Brightness = lightResponse["brightness"]?.ToString(),
                TimeStamp = lightResponse["last_seen"]?.ToString()
            };
            if (GetLightStates().Result.LastOrDefault().IsOn != lightState.IsOn ||
                GetLightStates().Result.LastOrDefault().Brightness != lightState.Brightness)
            {
                await _lightStateRepository.Create(lightState);
            }

            return lightResponse.ToString();
        }
        
        [HttpPost("light/turn-on")]
        public async Task<string> TurnOn()
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all/state");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", LightToken);
            request.AddParameter("power", "on");
            
            var response = client.Execute(request);

            var responseBody = JObject.Parse(response.Content);
            var isSuccess = (responseBody["results"]?[0]?["status"]?.ToString() == "ok") ? true : false ;
            
            if (isSuccess && !IsLastEntrySameState("on"))
            {
                var lastStateCheckOnServer = GetState();
                var lightState = new LightState()
                {
                    IsOn = true,
                    Brightness = lastStateCheckOnServer["brightness"]?.ToString(),
                    TimeStamp = lastStateCheckOnServer["last_seen"]?.ToString()
                };
                var newLightState = await _lightStateRepository.Create(lightState);
                return "turned on and entry made";
            }
            else if (isSuccess && IsLastEntrySameState("on"))
            {
                return "turned on but entry not made";
            }

            return "light not available";
        }
        
        [HttpPost("light/turn-off")]
        public async Task<string> TurnOff()
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all/state");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", LightToken);
            request.AddParameter("power", "off");
            
            var response = client.Execute(request);
            
            
            var responseBody = JObject.Parse(response.Content);
            var isSuccess = (responseBody["results"]?[0]?["status"]?.ToString() == "ok") ? true : false ;
            
            if (isSuccess && !IsLastEntrySameState("off"))
            {
                var lastStateCheckOnServer = GetState();
                var lightState = new LightState()
                {
                    IsOn =  false,
                    Brightness = lastStateCheckOnServer["brightness"]?.ToString(),
                    TimeStamp = lastStateCheckOnServer["last_seen"]?.ToString()
                };
                var newLightState = await _lightStateRepository.Create(lightState);
                return "turned off and entry made";
            }
            else if (isSuccess && IsLastEntrySameState("on"))
            {
                return "turned off but entry not made";
            }

            return "light not available";
        }

        [HttpPost("light/toggle-power")]
        public string TogglePower()
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all/toggle");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", LightToken);

            var response = client.Execute(request);
            return response.Content;
        }

        [HttpPost("light/brightness/{id}")]
        public async Task<string> SetBrightness(int id)
        {
            var divisor = id / (float)100;
            var client = new RestClient("https://api.lifx.com/v1/lights/all/state");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", LightToken);
            request.AddParameter("power", "on");
            request.AddParameter("brightness", divisor);
            
            var response = client.Execute(request);
            
            var responseBody = JObject.Parse(response.Content);
            var isSuccess = (responseBody["results"]?[0]?["status"]?.ToString() == "ok") ? true : false ;
            
            switch (isSuccess)
            {
                case true when !IsLastEntrySameBrightness(divisor.ToString(CultureInfo.InvariantCulture)):
                {
                    var lastStateCheckOnServer = GetState();
                    var lightState = new LightState()
                    {
                        IsOn =  true,
                        Brightness = divisor.ToString(CultureInfo.InvariantCulture),
                        TimeStamp = lastStateCheckOnServer["last_seen"]?.ToString()
                    };
                    var newLightState = await _lightStateRepository.Create(lightState);
                    return $"Brightness set to {id} and entry made";
                }
                case true when IsLastEntrySameState("on"):
                    return $"Brightness set to {id} but no entry made";
                default:
                    return "light not available";
            }
        }
        
        [HttpPost("light/color/{color}")]
        public string SetColor(string color)
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all/state");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", LightToken);
            request.AddParameter("power", "on");
            request.AddParameter("color", color);
            
            var response = client.Execute(request);
            var responseBody = JObject.Parse(response.Content);
            return (responseBody["results"]?[0]?["status"]?.ToString() == "ok") ? $"color set to {color}" : "light not available" ;
        }
        
        [HttpPost("light/breathe")]
        public string SetBreathe()
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all/effects/breathe");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", LightToken);
            request.AddParameter("period", 6);
            request.AddParameter("cycles", 6);
            request.AddParameter("color", "yellow");
            
            var response = client.Execute(request);
            var responseBody = JObject.Parse(response.Content);
            return (responseBody["results"]?[0]?["status"]?.ToString() == "ok") ? $"Initiating breathe effect" : "light not available" ;
        }
        
        [HttpPost("light/enable-srss-feature/{enableFeature}")]
        public string SunriseSunsetFeature(bool enableFeature)
        {
            var utility = new Utility();
            var jObject =  utility.EnableSunriseSunsetFeature(enableFeature);

            return jObject.ToString();
        }
        
        [HttpPost("light/fetch-srss-feature")]
        public string FetchSunriseSunsetFeature()
        {
            var utility = new Utility();
            var jObject =  utility.GetData();
            
            return jObject.ToString();
        }


        [HttpPost("light/stats")]
        public List<string> GetStats()
        {
            var lightResponse = GetState();

            var stats = new List<string>()
            {
                "Last Seen: " + lightResponse["last_seen"]?.ToString(),
                "Label: " + lightResponse["label"]?.ToString(),
                "Connected: " + lightResponse["connected"]?.ToString(),
                "Power: " + lightResponse["power"]?.ToString(),
                "Hue: " + lightResponse["color"]?["hue"]?.ToString(),
                "Saturation: " + lightResponse["color"]?["saturation"]?.ToString(),
                "Kelvin: " + lightResponse["color"]?["kelvin"]?.ToString(),
                "Brightness: " + lightResponse["brightness"]?.ToString(),
                "Light Name: " + lightResponse["product"]?["name"]?.ToString(),
                "Identifier: " + lightResponse["product"]?["identifier"]?.ToString(),
            };
            return stats;
        }
        

        [HttpPost("light/utility")]
        public List<string> ProcessUtility()
        {
            var filtered = new List<LightState>();
            var lightStates = GetLightStates().Result;
            var diff = TimeSpan.Parse("0");
            var lastIsOn = lightStates.FirstOrDefault().IsOn;

            foreach (var lightState in lightStates)
            {
                switch (lastIsOn)
                {
                    case true when lightState.IsOn:
                        filtered.Add(lightState);
                        lastIsOn = false;
                        break;
                    case false when !lightState.IsOn:
                        lastIsOn = true;
                        filtered.Add(lightState);
                        break;
                }
            }

            if (filtered[0].IsOn == false)
            {
                filtered.RemoveAt(0);
            }

            var lastTimeStamp = DateTime.Parse(filtered[0].TimeStamp);

            for (var index = 0; index < filtered.Count; index++)
            {
                var lightState = filtered[index];
                if (lightState.IsOn) continue;
                diff += DateTime.Parse(lightState.TimeStamp).Subtract(lastTimeStamp);
                lastTimeStamp = DateTime.Parse(filtered[index + 1].TimeStamp);
            }
            return new List<string>()
                { diff.ToString(), lightStates.FirstOrDefault()?.TimeStamp };
        }

        [HttpPost("light/brightness-list")]
        public List<float> GetBrightnesses()
        {
            return (from lightState in GetLightStates().Result where lightState.IsOn select float.Parse(lightState.Brightness)).ToList();
        }
        
        [HttpPost("light/initial-timestamp")]
        public string GetFirstTimeStamp()
        {
            return GetLightStates().Result.FirstOrDefault()?.TimeStamp;
        }
        
        [HttpPost("light/current-state")]
        public string GetCurrentState()
        {
            return GetState()["power"]?.ToString();
        }
        
        [HttpPost("light/current-brightness")]
        public float GetCurrentBrightness()
        {
            return float.Parse(GetState()["brightness"]?.ToString() ?? string.Empty)*100;
            
        }

        [HttpPost("light/get-time/{time}")]

        public void GetTime(string time)
        {
            String[] userTime = time.Split(":");
            var hour = userTime[0];
            var minute = userTime[1];

            JObject timeObj = new JObject(
                new JProperty("hour", hour),
                new JProperty("minute", minute)
            );
            //using(StreamWriter file = System.IO.File.CreateText("../backend/TimeDate.json"));
            System.IO.File.WriteAllText("../backend/TimeDate.json",timeObj.ToString());
        }

        [HttpPost("light/get-schedule/{onOff}")]
        public void GetSchedule(string onOff)
        {
            if (onOff == "ON")
            {
                _IsScheduledON = true;
                _IsScheduledOff = false;
            }
            else
            {
                _IsScheduledON = false;
                _IsScheduledOff = true;
            }

        }
    }
}