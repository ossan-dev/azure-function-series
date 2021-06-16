### The development part </>

The first thing u need to do is to create a "models" folder in "azure-function-entities". Into this newly created folder u must add a class called Output.cs which will contain the info that will be logged in our console. The Output class is as follows:

```
namespace azure_function_entities.models
{
    public class Output
    {
        public string Message { get; set; }
        public string ApiKey { get; set; }
    }
}
```

In "azure-function-managers" create a "services" folder that will hold our concrete service implementation and its interface definition. The service will be GreetingsService and will simply have a method called SayHello which returns a string through string interpolation.

```
namespace azure_function_managers
{
    public class GreetingsService
    {
        public string SayHello(string name) => $"Hello {name}!";
    }
}
```

If u refactor this service and pull up its interface you'll get a file that looks like this:

```
namespace azure_function_managers
{
    public interface IGreetingsService
    {
        string SayHello(string name);
    }
}

```

This file will be named "IGreetingsService.cs".
