using System.Reflection;
using azure_function;
using azure_function_manager;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace azure_function
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration Configuration { get; set; }
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IGreetingsService, GreetingsService>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var ctx = builder.GetContext();
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets(assembly: Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
