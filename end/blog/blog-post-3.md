# Playing with Azure Functions: Mission 3

## Hashtags

dotnet, azure, function, serverless, net5, vscode, development

Hi champions! Welcome back to my Azure Function series. First, if u're here with me I must celebrate u 👏🏻 as u've been successfully carried out the previous missions. If this is not the case, please be sure to checkout the other two posts.  
Even if it's not mandatory to checkout them,  I **strongly** suggest u to do because in this last mission we're going to take our Azure Function built in the previous missions and make some changes on top of it. Moreover, this post will assume that u've a _basic knowledge_ about the topic (don't worry u can gain it by reading the previous posts and u'll learn the basic concepts with a bit of fun 😂).  
Today we're gonna to talk about **UPGRADING** our Azure Function from .NET Core 3.1 to the desired .NET 5.0.  
So, stay tuned!  
NOTE: If u get in trouble in following the tutorial u can find all the code in "end" folder of my GitHub repo that u can find [here](https://github.com/ivan-pesenti/azure-function-series).

## Our previous achievements 🥳

If u have completed the MISSION 1 and 2 u now have a working solution with the following features:
- A proj called "azure-function-entities" that holds the **models** for our application. It has a class named "Output.cs" made up by props that are logged by our Azure Function. Its target framework is .NET Standard 2.1.
- A proj called "azure-function-managers" responsible for holding the **services** for our application. It has only a service named "GreetingsService" with its interface "IGreetingsService". Its target framework is .NET Standard 2.1.
- A proj called "azure-function" with our **Timer-Triggered** Azure Function that on each second divisible by 5 will log messages to our console. It used models and services provided by the projs listed above. Its target framework is .NET Core 3.1 (LTS).

## Let' start 👨🏻‍🚀👩🏻‍🚀

## Final mission

Be ready ⚔! Our journey is going to finish but this mission will be the _hardest_. Through this mission we're going to upgrade our .NET Core 3.1 Azure Function that has been developed with **in-process** programming model to a NET 5.0 Azure Function that will run in an **isolated worker process**. This migration is divided into three minor migrations: one for each projs in our solution.  
Now it's time to stop to speak and start to develop 💻.

## Initial setup 👷🏻‍♂️

### .NET 5 SDK 

First, it's mandatory to be sure that our development environment can support projs targeting the NET 5.0 framework. If u haven't developed yet a NET 5.0 application probably u should download the related SDK. U can download it from [here](https://dotnet.microsoft.com/download/dotnet/5.0). After u have successfully installed it u can try to issue this comamnd in a shell: 
```shell
dotnet --version
```
If the number u got is starting with 5, u're good to go. 

### Renaming phase
**📖HINT📖**: imo you should do these steps with VSCode closed and open it later. 
As we're going to keep the previous working function, we opt to duplicate the "start" folder and make the migration upon this newly created folder.  
So, copy-paste the "start" folder and rename it to "end". Using the terminal go into "end" folder and u'll find a file called "start.sln". Rename it to "end.sln" to be consistent with parent directory name.  
**⚠️WARNING⚠️**: also check the "end.sln" in order to be sure that no more refs pointing to "start" are left in it. If this is not the case, be sure to adapt them according to the new file system structure.  
Now issue this command to bring up VSCode:
```shell
code . -r
```

## azure-function-entities proj

Open the file "azure-function-entities.csproj" and change the target framework as below:
```xml
<TargetFramework>net5.0</TargetFramework>
```
After that go into "models" folder and open up the "Output.cs" class file. Change the actual implementation with the following code: 
```csharp
namespace azure_function_entities.models
{
    public record Output(string Message, string ApiKey);
}
```
**🔎NOTE🔎**: this is a new language syntax introduced by C# 9.0. It's called _record_ and u can learn more about them [here](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record).  
Compile this project singularly to be sure that it's still working as expected.

## azure-function-managers proj
Open the file "azure-function-managers.csproj" and change the target framework as below:
```xml
<TargetFramework>net5.0</TargetFramework>
```
Compile this project singularly to be sure that it's still working as expected.

## azure-function proj
In this project we're going to make the most substantial changes in our application. So, in order to keep things neat and clear, I decided to dedicate a little section to each affected file.

### "azure-function.csproj"
First, edit the target framework as shown below:
```xml
<TargetFramework>net5.0</TargetFramework>
```
**🔴IMPORTANT🔴**: right below this setting add this one that is mandatory to successfully do the migration:
```xml
<OutputType>Exe</OutputType>
```
The last change is to upgrade the section about Nuget Package: replace your section with this one:
```xml
  <ItemGroup>
    <PackageReference Include="Microsoft.Azure.Functions.Worker.Sdk" Version="1.0.3" OutputItemType="Analyzer"/>
    <PackageReference Include="Microsoft.Azure.Functions.Worker" Version="1.3.0"/>
    <PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="5.0.0"/>
    <PackageReference Include="Microsoft.Azure.Functions.Worker.Extensions.Timer" Version="4.0.1" />
  </ItemGroup>
```
When u save, VS Code will prompt u to restore the dependencies of csproj file, confirm and wait for the operation to finish.  
**🔎NOTE🔎**: once the operation finished a bunch of error messages and warning will start arising. Don't worry, we'll fix them in a minute 🐱‍👤.  

### "local.settings.json"
In order to change the Azure Function host from "in-process" to "isolated" we need to update the "FUNCTION_WORKER_RUNTIME" setting as follow:
```json
"FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
```
U can learn more about the isolated process for Azure Function [here](https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide).

### "Startup.cs" ❌

With this new programming model, we've full control over the configuration of the working process. We can register our application services, define the configuration of the worker process and so on. All this bunch of stuff will be defined in the next file we can deal with. We'll go step-by-step in the next paragraph.  
With this in mind we can safely delete this file from our proj.

### "Program.cs" 🗼

This is the place where we can do all of the necessary setup of our Azure Function. Of course, we've to create this file in our proj.  
**🕵🏻‍♂️NOTE🕵🏻‍♂️**: in our case, this file is missing because we're upgrading from an older version of Azure Function. If u start a NET 5.0 proj from scratch, this file will be included in the auto-generated files.  
Add a file named "Program.cs" in the "azure-function" proj and paste the following code:
```csharp
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
```

**⚠️WARNING⚠️**: always double check the namespaces as their are one of the primary source of errors in this copy-paste operations.
The purpose of this code is to **configure**, **build** and **run** the worker process for our Azure Function. More in depth u can see that:
- The host builder is setup with default configs
- User secrets are added as configuration providers for our host process
- The "Greetings" service is added to the built-in container with a _service lifetime of scoped_
Finally the host is built and launched.

### "TimerTriggerFunc.cs"
This file is where the Run method is defined. Here u can see the _input/output bindings_ of the function, the _trigger_ that fires it up and the _code_ to execute. There are three main changes to do in order to get the job done (I'll explain in a bit):

```csharp
using System;
using azure_function_entities.models;
using azure_function_managers.services;
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
        public void Run([TimerTrigger("%TimerCron%")] TimerInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger<TimerTriggerFunc>();
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            Output output = new(_greetingsService.SayHello("ivan"), _configuration.GetValue<string>("ApiKey"));
            logger.LogInformation($"Message: {output.Message}");
            logger.LogInformation($"ApiKey: {output.ApiKey}");
        }

        public class TimerInfo
        {
            public ScheduleStatus ScheduleStatus { get; set; }
            public bool IsPastDue { get; set; }
        }

        public class ScheduleStatus
        {
            public DateTime Last { get; set; }
            public DateTime Next { get; set; }
            public DateTime LastUpdated { get; set; }
        }
    }
}
```

The first thing to notice is the new **using statements** at the top of the file. The most important is `using Microsoft.Azure.Functions.Worker;` that allow us to use "isolated" process features.  
Second the new signature of the Run method: instead of ILogger now the function expects a FunctionContext type that represents the execution context of our function. From it we can get a logger to use in our method instead of the one provided by "in-process" model & .NET Core 3.1.  
**🔎NOTE🔎**: in the third line of the body of Run method we used two new features of **_C# 9.0_**: the _record_ types (link above) and the _Target-typed new_ expression (more on it [here](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/target-typed-new)).  
Last but not least we must define the TimerInfo and ScheduleStatus types that now are no more provided by the package `Microsoft.Azure.WebJobs`.  
**🔵IMPORTANT🔵**: it's always a best practice to define the types in their own class files but for the sake of simplicity I put the types definition inside the "TimerTriggerFunc.cs" file. 

## Kind remainder before the battle ⚔

If u've not started the Azure Storage Emulator a popup will appear like this one:
<p align="center">
    <img src="https://github.com/ivan-pesenti/azure-function-series/blob/main/end/blog/img/azure-storage-emulator.png?raw=true" alt="Azure Storage Emulator" width="700px" />
</p>

Go in the start menu and type in "Azure Storage Emulator" and open the program. In the command prompt u should see the confirmation "The storage emulator was successfully started.".

## Win or lose? 🏁
Now the moment of truth... Press F5 and check if our Azure Function works or not.
<p align="center">
    <img src="https://github.com/ivan-pesenti/azure-function-series/blob/main/end/blog/img/function-execution.png?raw=true" alt="Azure Function executions" width="1000px" />
</p>

The function still runs even if it's been upgraded to the new features.

## Final thoughts 💭
I think that this is an important achievements for us. Microsoft will follow this direction so at certain point in time will be mandatory to conform to the "isolated-process" programming model. Just for ref I'll share u an image that shows off the Azure Function Roadmap:

<p align="center">
    <img src="https://techcommunity.microsoft.com/t5/image/serverpage/image-id/262318i4234B132C742509C/image-size/large?v=v2&px=999" alt="Azure Function roadmap" width="700px"/>
</p>

This image is self-explanatory: sooner or later u will be about migrating your old-fashioned Azure Function to the fresh-new way of doing things.  

## Outro + Greetings 👋🏻
The intent of this post is to make your life easier in this migration process that u may encountered in your development life.  
Congratulations for reaching this final step and achieve it. It's been a pleasure to fight with u. I hope u find this series helpful and funny. I made my best to make this content as valuable as possible.  

I hope you enjoy this post and find it useful. If you have any questions or you want to spot me some errors I really appreciate it and I'll make my best to follow up. If you enjoy it and would like to sustain me consider giving a like and sharing on your favorite socials. If u want u can add me on your socials this makes me very very happy!

Stay safe and see you soon! 😎
