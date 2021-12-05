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


        public LightOnTask(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule
        {
            get
            {

                return $"* * * * * ";
               
            }
        }
        
        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            var hour = Int32.Parse(JsonUtil.GetHour());
            var minute = Int32.Parse(JsonUtil.GetMinutes());
            var isOnOff = JsonUtil.GetOnOff();
            DateTime dateTime = DateTime.Now;
            if (dateTime.Hour == hour && dateTime.Minute == minute)
            {
                Console.WriteLine("LightOnTask : " + DateTime.Now.ToString());
                
                LightStateController lightStateController = scopeServiceProvider.GetRequiredService<LightStateController>();
                switch (isOnOff)
                {
                    case "ON":
                        Console.WriteLine("TurnON");
                        lightStateController.TurnOn();
                        break;
                    case "OFF":
                        Console.WriteLine("TurnOFF");
                        lightStateController.TurnOff();
                        break;
                }
            }
            return Task.CompletedTask;
        }
    }
}