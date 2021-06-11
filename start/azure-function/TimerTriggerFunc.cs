using System;
using azure_function_entities.models;
using azure_function_manager;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace azure_function
{
    public class TimerTriggerFunc
    {
        private readonly IGreetingsService _greetingsService;
        public TimerTriggerFunc(IGreetingsService greetingsService)
        {
            _greetingsService = greetingsService;

        }

        [FunctionName("TimerTriggerFunc")]
        public void Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var output = new Output("Message from TimerTriggerFunc()", "");

            log.LogInformation($"Message: {output.Message}");
            log.LogInformation($"ApiKey: {output.ApiKey}");
        }
    }
}
