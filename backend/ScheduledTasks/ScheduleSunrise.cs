using System;
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
                var utility = new Utility();
                dynamic data = utility.GetData();
                
                if (data.srss_feature == null) return "0 0 * * *";
                DateTime sunrise = utility.ConvertToDateTime(data.srss_feature.sunrise.ToString());
                return $"{sunrise.Minute} {sunrise.Hour} * * *";
            }
        }
        
        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            Console.WriteLine("Turn On at Sunrise : " + DateTime.Now);
            var lightStateController = scopeServiceProvider.GetRequiredService<LightStateController>();
            
            var utility = new Utility();
            dynamic data = utility.GetData();
            
            var enabled = (bool) data.srss_feature.enabled;
            if (enabled) lightStateController.TurnOn();
            
            return Task.CompletedTask;
        }
    }
}