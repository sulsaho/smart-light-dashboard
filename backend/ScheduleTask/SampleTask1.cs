using System;
using System.Threading.Tasks;
using LightWebAPI.BackgroundService;
using LightWebAPI.Controllers;
using LightWebAPI.Models;
using LightWebAPI.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LightWebAPI.ScheduleTask
{
    public class SampleTask1 : ScheduledProcessor
    {

        public SampleTask1(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }
        protected override string Schedule
        {
            get
            {
                var hour = 14;
                var minute = 06;
                return $"{minute} {hour} * * * ";
            }
        }

        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            Console.WriteLine("SampleTask1 : " + DateTime.Now.ToString());
            LightStateController lightStateController = scopeServiceProvider.GetRequiredService<LightStateController>();
            lightStateController.TogglePower();
            
            return Task.CompletedTask;
        }
    }
}
