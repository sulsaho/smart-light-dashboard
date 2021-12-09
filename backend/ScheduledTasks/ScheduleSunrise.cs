using System;
using System.Globalization;
using System.Threading.Tasks;
using LightWebAPI.BackgroundService;
using LightWebAPI.Controllers;
using LightWebAPI.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace LightWebAPI.ScheduleTask
{
    public class ScheduleSunrise : ScheduledProcessor
    {
        public ScheduleSunrise(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule
        {
            get
            {
                dynamic data = Utility.GetData();
                
                if (data.srss_feature == null) return "0 0 * * *";
                DateTime sunrise = DateTime.ParseExact(data.srss_feature.sunrise.ToString(), "h:mm:ss tt", CultureInfo.InvariantCulture);
                return $"{sunrise.Minute} {sunrise.Hour} * * *";
            }
        }
        
        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            var lightStateController = scopeServiceProvider.GetRequiredService<LightStateController>();
            
            dynamic data = Utility.GetData();
            
            var enabled = (bool) data.srss_feature.enabled;
            if (enabled) lightStateController.TurnOn();
            
            return Task.CompletedTask;
        }
    }
}