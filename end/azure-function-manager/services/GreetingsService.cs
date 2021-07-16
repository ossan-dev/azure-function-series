namespace azure_function_manager
{
    public class GreetingsService : IGreetingsService
    {
        public string SayHello(string name) => $"Hello {name}!";
    }
}
