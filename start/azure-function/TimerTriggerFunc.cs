using System;
using azure_function_entities.models;
using azure_function_manager;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
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

        [FunctionName("TimerTriggerFunc")]
        public void Run([TimerTrigger("%TimerCron%")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var output = new Output(_greetingsService.SayHello("ivan"), _configuration.GetValue<string>("ApiKey"));
            log.LogInformation($"Message: {output.Message}");
            log.LogInformation($"ApiKey: {output.ApiKey}");
        }
    }
}
