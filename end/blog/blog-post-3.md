# Playing with Azure Functions: Mission 3

## Hashtags

dotnet, azure, function, serverless, net5, vscode, development

Hi champions! Welcome back to my Azure Function series. First, if u're here with me I must celebrate u as u've been successfully carried out the previous missions. If this is not the case, please be sure to checkout the other two posts.  
Even if it's not mandatory to checkout them,  I strongly suggest u to do because in this last mission we're going to take our Azure Function biult in the previous missions and make some changes to it. Moreover, this post will assume that u've a basic knowledge about the topic (don't worry u can gain it by reading the previous posts and u'll learn the basic concepts with a bit of fun 😂).  
Today we're gonna to talk about UPGRADING our Azure Function from .NET Core 3.1 to the desired .NET 5.0.  
So, stay tuned!  
NOTE: If u get in trouble in following the tutorial u can find all the code in "end" folder of my GitHub repo that u can find [here](https://github.com/ivan-pesenti/azure-function-series).

## Our previous achievements

If u have completed the MISSION 1 and 2 u now have a working solution with the following features:
- A proj called "azure-function-entities" that holds the models for our application. It has a class named "Output.cs" with props that are logged by our Azure Function. Its target framework is .NET Standard 2.1.
- A proj called "azure-function-managers" responsible to hold the services for our application. It has only a service named "GreetingsService" with its interface "IGreetingsService". Its target framework is .NET Standard 2.1.
- A proj called "azure-function" with our Timer-Triggered Azure Function that on each second divisible by 5 will log a message to our console. It used models and services provided by the projs listed above. Its target framework is .NET Core 3.1 (LTS).

## Let' start 👨🏻‍🚀👩🏻‍🚀

## Final mission

Be ready! Our journey is going to finish but this mission will be the hardest. Through this mission we're going to upgrade our .NET Core 3.1 Azure Function that has been developed with in-process programming model to a NET 5.0 Azure Function that will run in an isolated worker process. This migration is divided into three minor migrations: one for each projs in our solution.  
Now it's time to stop to speak and start to develop 💻.

## Initial setup

### .NET 5 SDK

First, it's mandatory to be sure that our development environment can support projs with NET 5.0 as the target framework. If u haven't developed yet a NET 5.0 application probably u should download the related SDK. U can download it from [here](https://dotnet.microsoft.com/download/dotnet/5.0). After u have successfully installed it u can try to issue this comamnd on your machine: 
```shell
dotnet --version
```
If the number u got is starting with 5, u're good to go. 

### Renaming phase
HINT: imo you should do these steps with VSCode closed and open it later. 
As we're going to keep the previous working function, we opt to duplicate the "start" folder and make the migration upon this newly created folder.  
So, copy-paste the "start" folder and rename it to "end". Using the terminal go into "end" folder and u'll find a file called "start.sln". Rename it to "end.sln" to be consistent with parent directory name.  
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
NOTE: this is a new language syntax introduced by C# 9.0. It's called record and u can learn more about them [here](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record).  
Compile this project singularly to be sure that it's still working as expected.

## azure-function-managers proj
Open the file "azure-function-managers.csproj" and change the target framework as below:
```xml
<TargetFramework>net5.0</TargetFramework>
```
Compile this project singularly to be sure that it's still working as expected.
