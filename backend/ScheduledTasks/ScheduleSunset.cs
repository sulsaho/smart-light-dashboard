using System;
using System.Threading.Tasks;
using LightWebAPI.BackgroundService;
using LightWebAPI.Controllers;
using LightWebAPI.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace LightWebAPI.ScheduleTask
{
    public class ScheduleSunset : ScheduledProcessor
    {
        public ScheduleSunset(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule
        {
            get
            {
                var utility = new Utility();
                dynamic data = utility.GetData();
                
                if (data.srss_feature == null) return "0 0 * * *";
                DateTime sunset = utility.ConvertToDateTime(data.srss_feature.sunset.ToString());
                return $"{sunset.Minute} {sunset.Hour} * * *";
            }
        }
        
        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            Console.WriteLine("Turn Off at Sunset : " + DateTime.Now);
            var lightStateController = scopeServiceProvider.GetRequiredService<LightStateController>();
            
            var utility = new Utility();
            dynamic data = utility.GetData();
            
            var enabled = (bool) data.srss_feature.enabled;
            if (enabled) lightStateController.TurnOff();
            
            return Task.CompletedTask;
        }
    }
}