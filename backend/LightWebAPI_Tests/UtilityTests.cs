using LightWebAPI.Utils;
using Newtonsoft.Json.Linq;
using Xunit;

namespace LightWebAPI_Tests
{
    public class UtilityTests
    {
        [Fact]
        public void EnsureGetSunriseSunsetTimes()
        {
            dynamic srssObj = Utility.GetSunriseSunset();
            
            Assert.IsType<JObject>(srssObj);
            Assert.EndsWith("PM", srssObj.srss_feature.sunset.ToString());
            Assert.EndsWith("AM", srssObj.srss_feature.sunrise.ToString());
        }
        
        [Fact]
        public void EnsureEnableSunriseSunsetFeature()
        {
            dynamic srssObj = Utility.EnableSunriseSunsetFeature(true);
            
            Assert.True((bool)srssObj.srss_feature.enabled);
        }
        
        [Fact]
        public void EnsureDisableSunriseSunsetFeature()
        {
            dynamic srssObj = Utility.EnableSunriseSunsetFeature(false);
            
            Assert.IsType<JObject>(srssObj);
            Assert.False((bool)srssObj.srss_feature.enabled);
        }
        
        [Fact]
        public void EnsureRetrievalOfSrssJsonObject()
        {
            dynamic storedObj = Utility.GetData();
            
            Assert.IsType<JObject>(storedObj);
            Assert.NotEmpty(storedObj);
        }
        
        [Fact]
        public void EnsureConversionOfDateTimeStringToCst()
        {
            string time = Utility.ConvertToDateTime("1:59:05 PM");
            Assert.Equal("7:59:05 AM", time);
        }
        
        [Fact]
        public void EnsureGetApiKey()
        {
            Assert.NotNull(JsonUtil.GetApiKey());
        }
        
        [Fact]
        public void EnsureGetHour()
        {
            Assert.NotNull(JsonUtil.GetHour());
        }
        
        [Fact]
        public void EnsureGetMinute()
        {
            Assert.NotNull(JsonUtil.GetMinutes());
        }
    }
}