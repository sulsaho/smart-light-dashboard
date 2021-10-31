using System.Collections.Generic;
using System.Threading.Tasks;
using LightWebAPI.Models;
using LightWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace LightWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LightStateController : ControllerBase
    {
        private readonly ILightStateRepository _lightStateRepository;
        private string LightToken = "Bearer cfe132196d8b5eadb1a8ec2f8e09d6ec90a96267e721a7f9295932eabfbdfff0";

        public LightStateController(ILightStateRepository lightStateRepository)
        {
            _lightStateRepository = lightStateRepository;
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
    }
}