# Playing with Azure Functions: Mission 2

## Hashtags

dotnet, azure, function, serverless, vscode, development

Hi guys! Welcome back to my Azure Function series. I hope u're ready to play, develop and (more important) learn with me. I'm really excited to be back on track 😎.  
🔴IMPORTANT❗🔴: this is the second post of a series so, if u missed the previuos one, I strongly suggest u to check it out at this [link](https://dev.to/ivan_pesenti/playing-with-azure-functions-mission-1-55b8).  
As always if u get in trouble in following the tutorial u can find all the code in "start" folder of my GitHub repo that u can find [here](https://github.com/ivan-pesenti/azure-function-series).

## What u got from MISSION 1

If u've completed the MISSION 1 (congrats again 🍻) u now have the following weapons in your toolset:
- A working Azure Function timer-triggered written with C# and .NET Core 3.1
- A solution with three projects:
  - Azure function proj called "azure-function"
  - Entities class library proj called "azure-function-entities"
  - Managers class library proj called "azure-function-managers"

Now it's time to go ahead and start to deal with MISSION 2.

## Let's start 🚀

## Mission 2

Now the game is becoming more complicated than before. In this mission u must show your development skills and push hard to achieve it. In this mission our goal will be:
- Use local.settings.json to set the CRONTAB expression
- Creation of class & service in their respective projs
- Add the necessary references among the projs in the solution
- Make use of Dependency Injection
- Keep secure sensitive data through User Secrets  

If this sounds exciting for u please take a deep breath and follow me in the battle 🐱‍🐉.

## Make the CRONTAB not hard-coded

The first task is to tie our CRONTAB expression (which is responsible for defining when our function will run) to a key in the local.settings.json. This is done in order to be able to change it without redeploying our function and to make our function smarter. If u keep this expression within your source code (such as in TimerTriggerFunc.cs) when u want to change it you must do a full publish. This is a pretty annoying stuff for a trivial change 🥱. So let's do this.  
First change the file TimerTriggerFunc.cs to this:  
```csharp
public static class TimerTriggerFunc
{
    [FunctionName("TimerTriggerFunc")]
    public static void Run([TimerTrigger("%TimerCron%")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
    }
}
```  
After that u have to add this key in your local.settings.json, in your "Values" object:

```json 
"TimerCron": "*/5 * * * * *" 
```

## Creation of model and service
The next task is to create the models and services needed by our application. Now we're going to use the two class library projs created in the previous mission. So let's start with models.

### Models
First, we need of a place for our classes. So, select the "azure-function-entities" proj and create a folder named "models". Inside it create a class named "Output.cs". Please fill the class with the following code:  
```csharp
namespace azure_function_entities.models
{
    public class Output
    {
        public string ApiKey { get; set; }
        public string Message { get; set; }
        public Output(string message, string apiKey)
        {
            ApiKey = apiKey;
            Message = message;
        }
    }
}
```

⚠️WARNING⚠️: during class creation (especially if u're using VSCode like me) pay attention to the namespace that should reflect the position of the class within the proj. If the namespace generated is different please change it accordingly.  

This code is very trivial and its only purpose is to hold the info that will be logged to the console during the function's execution.

### Services
Now it's time to focus about the services section. This section will hold the core logic of our application. So, create a folder called "services" in the "azure-function-managers" proj. In this folder create a class called "GreetingsService.cs" with the following code:  

```csharp
namespace azure_function_managers.services
{
    public class GreetingsService
    {
        public string SayHello(string name) => $"Hello {name}!";
    }
}
```

This will be our concrete implementation of the greetings functionality. What we need to do in order to use it through depencency injection is to create an interface for it and make this class implement it. But wait! Instead of manually creating a file for the interface, populating it and changing the concrete class to implement it we're going to use the VSCode auto-completion feature to achieve it.  
Okay, let's do the trick 🛹.  
Open the GreetingsService class and place the cursor on its name. Then u must press the keyboard shortcut: "Ctrl" + "." and a little context menu will appear:  
<br>
<p align="center">
    <img src="https://github.com/ivan-pesenti/azure-function-series/blob/main/start/img/post-2/extract-interface.png?raw=true" alt="Extract interface" width="750px" />  
</p>  

Make sure to select "Extract interface..." and after that the interface "IGreetingsService" will appear before your concrete class. Also note that the concrete implementation now implement it as u can see from this line:
```csharp
public class GreetingsService : IGreetingsService
```
I'm not completely satisfied with this so the last thing we're going to do is to move the interface in its own file.  
Again place your cursor on IGreetingsService (the first occurrence u meet in the file) and press the same the shortcut as before:  
<br>
<p align="center">
    <img src="https://github.com/ivan-pesenti/azure-function-series/blob/main/start/img/post-2/move-interface.png?raw=true" alt="Extract interface" width="650px" />  
</p>  
This time u select "Move type to IGreetingsService.cs".  

After this two touches of magic (every time I use these shortcuts I feel a bit like a little wizard 🧙🏻‍♂️) we can go ahead and carry out other tasks.

## Add references among projs

Let's take a look at the various proj files and do the necessary changes.  
- azure-function-managers.csproj:  
under the first section "PropertyGroup" place this code to ref entities proj  
```xml
<ItemGroup>
    <ProjectReference Include="../azure-function-entities/azure-function-entities.csproj"/>
</ItemGroup>
```
- azure-function.csproj:  
under the first "ItemGroup" section add the following code to ref the managers proj
```xml
<ItemGroup>
      <ProjectReference Include="../azure-function-manager/azure-function-manager.csproj" />
</ItemGroup>
```

## Creation of the Startup class

One of the most important thing we need to to is to build a class where we can resolve our dependencies at runtime (through the use of an IOC container) and provide the necessary configurations for our function. This place is called "Startup.cs".  
Before adding this class u must add this package in "azure-function" proj (⚠️WARNING⚠️: be sure to be in this proj when issuing the following statement):
```
dotnet add package Microsoft.Azure.Functions.Extensions
```  
After this step, u can create the Startup.cs in "azure-function" proj:
```csharp  
using azure_function;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using azure_function_managers.services;

[assembly: FunctionsStartup(typeof(Startup))]
namespace azure_function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IGreetingsService, GreetingsService>();
        }
    }
}

```  
As u can see in the configure method we registered our service with a lifetime of scoped. U can learn more about the service lifetime in .NET [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1).  
🧐*NOTE*🧐: be sure to decorate the function with the attribute "FunctionsStartup". This is mandatory in order to take advantage of the DI in our function. 

## Keep it secret

Nowadays it's fundamental to keep your confidential data safe 🔐. To achieve this we brought the User Secrets into the scene. To start using them u have to install a Nuget package. If u're not already there goes to "azure-function" proj through the CLI and issue this statement:  
```
dotnet add package Microsoft.Extensions.Configuration.UserSecrets --version 3.1.16
```
🤑*BONUS*🤑: in order to manage your User Secrets in a similiar way to the one provided by Visual Studio, u must install [this](https://marketplace.visualstudio.com/items?itemName=adrianwilczynski.user-secrets) VSCode extension.  
With this extension installed and enabled u can right click the "azure-function.proj" and select "Manage User Secrets". Complete the "secrets.json" with this info:  
```json
{
    "ApiKey": "ec47301e-f354-4a98-8796-174737964f2f"
}
```  
Our Azure Function is not going to use this secret automatically. We must tell to take it into consideration when defining the settings for the function. So open up the "Startup.cs" again and add the following method below the Configure one:  
```csharp  
public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
{
    var ctx = builder.GetContext();
    Configuration = new ConfigurationBuilder()
        .AddUserSecrets(assembly: Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
}
```  
With this piece of code we're telling how to configure our function. When u type this code the compiler start to complain so u've to add these using statements at the top of the "Startup.cs":  
```csharp  
using Microsoft.Extensions.Configuration;
using System.Reflection;
```  
The last thing u need to add is a property of type IConfiguration:  
```csharp  
public IConfiguration Configuration { get; set; }
```  
Now your Startup.cs looks complete and ready to use in our battle ⚔.

## TimerTriggerFunc.cs

The last file that we need to change is "TimerTriggerFunc.cs".
```csharp  
using System;
using azure_function_entities.models;
using azure_function_managers.services;
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
```  
With this last change we get rid of "static" keywords in order to make this class instantiable and add its constructor. Into its constructor we injected two dependencies:
- IGreetingsService: is the service that we developed above and return a welcome message to us
- IConfiguration: represents all of the configuration providers of a .NET Core application. U can deep dive this matter [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1)

## Check if mission succeded 🕵🏻‍♀️

Now with all this in place u can try to run your Azure Function and check if it's working as expected. Press F5 and keep your fingers crossed 🤞🏻:  
<br>
<p align="center">
    <img src="https://github.com/ivan-pesenti/azure-function-series/blob/main/start/img/post-2/function-output.png?raw=true" alt="Extract interface" width="900px" />  
</p>  
Et voilà! Your function is working as expected.

## Another great success 🏆
CONGRATULATIONS! You're unstoppable! This is your second mission completed.  
Through this mission u learned how to develop a more complex Azure Function by taking advantage of DI, User Secrets, other class library projs and so on. U make your Azure Function more reliable, customizable, reusable and even more performant. Now u're prepared to accomplish your final mission 🤴🏻.  
This mission will be about migrating this Azure Function that use the "in-process" programming model to the "isolated-process". We will also change the target framework of all projs to net5.0 that is the current target framework out right now. This will be a huge change for us so be prepared because the best is yet to come 🌞.    

I hope you enjoy this post and find it useful. If you have any questions or you want to spot me some errors I really appreciate it and I'll make my best to follow up. If you enjoy it and would like to sustain me consider giving a like and sharing on your favorite socials. If u want u can add me on your socials this makes me very very happy!

Stay safe and see you soon! 😎
