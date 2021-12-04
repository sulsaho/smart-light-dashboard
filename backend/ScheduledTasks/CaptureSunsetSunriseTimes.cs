using System;
using System.IO;
using System.Threading.Tasks;
using LightWebAPI.BackgroundService;
using LightWebAPI.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace LightWebAPI.ScheduleTask
{
    public class CaptureSunsetSunriseTimes : ScheduledProcessor
    {
        public CaptureSunsetSunriseTimes(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        // everyday day at midnight
        protected override string Schedule => $"0 0 * * *";

        /**
         * Retrieves sunrise and sunset times and save to a json file
         */
        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            //Console.WriteLine("Capture sunset and sunrise times : " + DateTime.Now);
            
            var utility = new Utility();
            dynamic getData = utility.GetData();
            
            bool enabled = getData.srss_feature.enabled ? getData.srss_feature.enabled : false;
            
            dynamic getSunriseSunsetTimes = utility.GetSunriseSunset();
            getSunriseSunsetTimes.srss_feature.enabled = enabled;

            using (var streamWriter = new StreamWriter(Utility.GetDataJsonPath()))
            {
                streamWriter.Write(getSunriseSunsetTimes.ToString());
                streamWriter.Close();
            }
            
            return Task.CompletedTask;
        }
    }
}