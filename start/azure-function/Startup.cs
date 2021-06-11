[assembly: FunctionsStartup(typeof(Startup))]
namespace azure_function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {

        }
    }
}
