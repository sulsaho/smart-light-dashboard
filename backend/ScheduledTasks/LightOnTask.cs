using System;
using System.Threading.Tasks;
using LightWebAPI.BackgroundService;
using LightWebAPI.Controllers;
using LightWebAPI.Repositories;
using LightWebAPI.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace LightWebAPI.ScheduleTask
{
    public class LightOnTask : ScheduledProcessor
    {
        private int _hour;
        private int _minute;
        
        public LightOnTask(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule
        {
            get
            {
                _hour = Int32.Parse(JsonUtil.GetHour());
                _minute = Int32.Parse(JsonUtil.GetMinutes());
                return $"{_minute} {_hour} * * * ";
            }
        }
        
        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            Console.WriteLine("LightOnTask : " + DateTime.Now.ToString());
            
            LightStateController lightStateController = scopeServiceProvider.GetRequiredService<LightStateController>();
            if (lightStateController._IsScheduledON == true)
            {
                lightStateController.TurnOn();
            }
            if (lightStateController._IsScheduledOff == true)
            {
                lightStateController.TurnOff();
            }
            return Task.CompletedTask;
        }
    }
}