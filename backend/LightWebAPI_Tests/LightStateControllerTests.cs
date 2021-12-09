using System.Collections.Generic;
using System.Linq;
using LightWebAPI.Controllers;
using LightWebAPI.Models;
using LightWebAPI.Repositories;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LightWebAPI_Tests
{
    public class LightStateControllerTests
    {
        private LightStateController Controller;
        
        public LightStateControllerTests()
        {
            var mockRepo = new Mock<ILightStateRepository>();
            mockRepo.Setup(repo => repo.Get()).ReturnsAsync(GetMockStates());
            Controller = new LightStateController(mockRepo.Object);
        }

        [Fact]
        public async void EnsureTurnOn()
        {
            var result = await Controller.TurnOn();
            Assert.StartsWith("turned on", result);
        }

        [Fact]
        public async void EnsureTurnOff()
        {
            var result = await Controller.TurnOff();
            Assert.StartsWith("turned off", result);
        }

        [Fact]
        public void EnsureTogglePower()
        {
            var result = Controller.TogglePower();
            Assert.Contains("SS Light", result);
            
            var parsedJson = JObject.Parse(result);
            Assert.NotEmpty(parsedJson);
            Assert.NotNull(parsedJson["results"]);
            Assert.NotNull(parsedJson["results"][0]);
            Assert.NotNull(parsedJson["results"][0]["status"]);
            Assert.Equal("ok", parsedJson["results"][0]["status"].ToString());
        }

        [Fact]
        public async void EnsureSetBrightness()
        {
            const int id = 25;
            var result = await Controller.SetBrightness(id);
            Assert.StartsWith($"Brightness set to {id}", result);
        }

        [Fact]
        public void EnsureSetColor()
        {
            const string color = "blue";
            Assert.Equal($"color set to {color}", Controller.SetColor(color));
        }

        [Fact]
        public void EnsureSetBreathe()
        {
            Assert.Equal("Initiating breathe effect", Controller.SetBreathe());
        }

        [Fact]
        public void EnsureSunriseSunsetFeature()
        {
            var result = Controller.SunriseSunsetFeature(true);
            
            var parsedJson = JObject.Parse(result);
            Assert.NotEmpty(parsedJson);
            Assert.NotNull(parsedJson["srss_feature"]);
            Assert.NotNull(parsedJson["srss_feature"]["enabled"]);
            Assert.True((bool) parsedJson["srss_feature"]["enabled"]);
        }

        [Fact]
        public void EnsureFetchSunriseSunsetFeature()
        {
            var result = Controller.FetchSunriseSunsetFeature();
            
            var parsedJson = JObject.Parse(result);
            Assert.NotEmpty(parsedJson);
            Assert.NotNull(parsedJson["srss_feature"]);
            Assert.NotNull(parsedJson["srss_feature"]["sunset"]);
            Assert.EndsWith("PM", parsedJson["srss_feature"]["sunset"].ToString());
        }

        [Fact]
        public void EnsureProcessUtility()
        {
            var result = Controller.ProcessUtility();
            Assert.NotNull(result);
            Assert.Equal("0.28", result.Last());
            Assert.Equal("0.00", result.First());
        }
        
        private List<LightState> GetMockStates()
        {
            var states = new List<LightState>();
            states.Add(new LightState()
            {
                Id = 1,
                IsOn = true,
                Brightness = "0.5",
                TimeStamp = "12/04/2021 05:52:01"
            });
            states.Add(new LightState()
            {
                Id = 2,
                IsOn = true,
                Brightness = "0.4",
                TimeStamp = "12/07/2021 09:35:06"
            });
            return states;
        }
    }
}