using System;
using azure_function_entities.models;
using azure_function_manager;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace azure_function
{
    public class TimerTriggerFunc
    {
        private readonly IGreetingsService _greetingsService;
        private readonly IConfiguration _configuration;

        public TimerTriggerFunc(IGreetingsService greetingsService, IConfiguration configuration)
        {
            _greetingsService = greetingsService;
            _configuration = configuration;
        }

        [Function("TimerTriggerFunc")]
        public void Run([TimerTrigger("%TimerCron%", RunOnStartup = true)] TimerInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger<TimerTriggerFunc>();
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var output = new Output(_greetingsService.SayHello("ivan"), _configuration.GetValue<string>("ApiKey"));
            logger.LogInformation($"Message: {output.Message}");
            logger.LogInformation($"ApiKey: {output.ApiKey}");
        }

        public class TimerInfo
        {
            public ScheduleStatus ScheduleStatus { get; set; }
            public bool IsPastDue { get; set; }
        }
    }

    public class ScheduleStatus
    {
        public DateTime Last { get; set; }
        public DateTime Next { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
