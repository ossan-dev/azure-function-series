namespace azure_function_managers.services
{
    public class GreetingsService : IGreetingsService
    {
        public string SayHello(string name) => $"Hello {name}!";
    }
}
