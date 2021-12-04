using System;
using System.Threading.Tasks;
using LightWebAPI.BackgroundService;
using LightWebAPI.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace LightWebAPI.ScheduleTask
{
    public class ScheduleStateChange : ScheduledProcessor
    {
        public ScheduleStateChange(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => $"* * * * *";
        
        public override Task ProcessInScope(IServiceProvider scopeServiceProvider)
        {
            var lightStateController = scopeServiceProvider.GetRequiredService<LightStateController>();
            lightStateController.AddState();
            Console.WriteLine("It happened");
            return Task.CompletedTask;
        }
    }
}