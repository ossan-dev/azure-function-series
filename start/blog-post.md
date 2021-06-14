# Azure Function Series: build .NET Core 3.1 function

## Hashtags

dotnet, azure, function, serverless

In these days the usage of Azure Function is increasing constantly. Azure Function is an amazing tool that u can use in many different real-world scenario such as exposing a lightweight REST-API, scheduling some process, respond to certain events and so on. With this in mind I decided to make a series about them and this is the first article of it.
This will be a practical series: I'll guide you in a step-by-step process in order to make u more confortable at writing, testing and finally migrating a timer triggered Azure Function from .NET Core 3.1 LTS (in process worker) to [.NET 5](https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide) (isolated worker process).  
As always if u get in trouble in following the process you can find all the code of this blog post in "start" folder of my GitHub repo that u can find [here](https://github.com/ivan-pesenti/azure-function-series)

## Software requirements

In order to follow this tutorial your machine must have:

- Visual Studio Code: this is our IDE for this demo
- [Azure Functions](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions) and [Azure Account](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account) VS Code extensions
- Azure Storage Emulator: direct link to download [here](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409)
- .NET Core 3.1 sdk: u can download from [here](https://dotnet.microsoft.com/download/dotnet/3.1)
- Azure Functions Core Tools: u can download from this [repo](https://github.com/Azure/azure-functions-core-tools)

## Our mission 🐱‍👤

Our super-duper mission is to build out a timer-triggered Azure Functions written in C#. This Azure Function will simply logs some messages in the console. Our Azure Functions will use the Dependency Injection to inject services in our function class via the constructor DI. In order to do this we must setup an IOC container in order to resolve and provide the dependencies at runtime. We'll also make use of User Secrets in order to keep secure our sensitive keys.  
Bonus: I'll show u how to manage a multi-layered solution in VS Code by managing the solution using dotnet CLI.

## Let's start 🚀
