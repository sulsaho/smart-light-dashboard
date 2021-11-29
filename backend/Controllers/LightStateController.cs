using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using LightWebAPI.Models;
using LightWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace LightWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LightStateController : ControllerBase
    {
        private readonly ILightStateRepository _lightStateRepository;
        private readonly Timer _timer;
        private string LightToken = "Bearer cfe132196d8b5eadb1a8ec2f8e09d6ec90a96267e721a7f9295932eabfbdfff0";

        public LightStateController(ILightStateRepository lightStateRepository)
        {
            _lightStateRepository = lightStateRepository;
            
            /*var startTimeSpan = TimeSpan.FromSeconds(3);
            var periodTimeSpan = TimeSpan.FromSeconds(3);
            
            _timer = new Timer( (e) =>
            {
                StateCheck();
            }, null, startTimeSpan, periodTimeSpan);*/
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

        [HttpPost("light/state-check")]
        public async Task<string> StateCheck()
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", LightToken);
            var response = client.Execute(request);
            
            /*
            var statusClient = new RestClient("https://api.lifx.com/v1/lights/all/state");
            var statusRequest = new RestRequest(Method.PUT);
            statusRequest.AddHeader("Authorization", LightToken);
            var statusResponse = statusClient.Execute(statusRequest);

            JObject jsonStatus = JObject.Parse(statusResponse.Content);
            if (jsonStatus["results"][0]["status"].ToString() == "timed_out" || jsonStatus["results"][0]["status"].ToString() == "offline")
            {
                return "Light is currently unreachable";
            }
            else
            {
                return "Success";
            }
            */
            
            var json = JArray.Parse(response.Content);
            var lightResponse = JObject.Parse(json[0].ToString());
            Console.WriteLine(lightResponse["power"]);
            Console.WriteLine(lightResponse["brightness"]);
            Console.WriteLine(lightResponse["last_seen"]);

            var lightState = new LightState()
            {
                IsOn = (lightResponse["power"].ToString() == "on") ? true : false,
                Brightness = lightResponse["brightness"].ToString(),
                TimeStamp = lightResponse["last_seen"].ToString()
            };
            var newLightState = await _lightStateRepository.Create(lightState);
            
            return response.Content;
        }
        
        [HttpPost("light/turn-on")]
        public string TurnOn()
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all/state");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", LightToken);
            request.AddParameter("power", "on");
            
            var response = client.Execute(request);
            return response.Content;
        }
        
        [HttpPost("light/turn-off")]
        public string TurnOff()
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all/state");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", LightToken);
            request.AddParameter("power", "off");
            
            var response = client.Execute(request);
            return response.Content;
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
        public string SetBrightness(int id)
        {
            var divisor = id / (float)100;
            var client = new RestClient("https://api.lifx.com/v1/lights/all/state");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", LightToken);
            request.AddParameter("power", "on");
            request.AddParameter("brightness", divisor);
            
            var response = client.Execute(request);
            return response.Content;
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
            return response.Content;
        }
        
        [HttpPost("light/breathe")]
        public string SetBreathe(string color)
        {
            var client = new RestClient("https://api.lifx.com/v1/lights/all/effects/breathe");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", LightToken);
            request.AddParameter("period", 6);
            request.AddParameter("cycles", 6);
            request.AddParameter("color", "yellow");
            
            var response = client.Execute(request);
            return response.Content;
        }

        [HttpPost("light/get-time/{time}")]

        public string getTime(string time)
        {
            Console.Write("Input time :" + time);
            return time;
        }

    }
}