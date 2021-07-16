using azure_function_managers.services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace azure_function
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration(options => options.AddUserSecrets(assembly: Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true))
            .ConfigureServices(services => services.AddScoped<IGreetingsService, GreetingsService>())
            .Build();

            host.Run();
        }
    }
}
