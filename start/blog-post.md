# Playing with Azure Function: Mission 1

## Hashtags

dotnet, azure, function, serverless, vscode

In these days the usage of Azure Function is increasing constantly. Azure Function is an amazing tool that u can use in many different real-world scenario such as exposing a lightweight REST-API, scheduling some process, respond to certain events and so on. With this in mind I decided to make a series about them and this is the first of three posts.
This will be a practical series: I'll guide you in a step-by-step process in order to make u more confortable at developing, testing and finally migrating a timer triggered Azure Function from .NET Core 3.1 LTS (in process worker) to [.NET 5](https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide) (isolated worker process).  
As always if u get in trouble in following the tutorial u can find all the code in "start" folder of my GitHub repo that u can find [here](https://github.com/ivan-pesenti/azure-function-series).

## Our **ULTIMATE** mission 🐱‍👤

Our super-duper mission is to build out a timer-triggered Azure Functions written in C#. This Azure Function will simply logs some messages in the console. Our Azure Functions will use the Dependency Injection to inject services in our function class via the constructor DI. In order to do this we must setup an IOC container that will resolve and provide the dependencies at runtime. We'll also make use of User Secrets in order to keep secure our sensitive keys. The last step will be migrating our Azure Function to **in-process** to **isolated-process** or **out-of-process** model. With this migration we're going to change the target framework of the function project from .NET Core 3.1 LTS to .NET 5.0.  
Bonus: I'll show u how to manage a multi-layered solution in VS Code by managing the solution using dotnet CLI.  
But wait! This mission looks like a huge mission that can scary if u don't tackle it step-by-step. So in order to prepare u as much as possible I'm going to split this mission into 3 sub-mission that u must complete in order to gain your desidered expertise.

## Let's start 🚀

### Mission 1

To warmup the first mission u have been assigned is to create the solution with all of the necessary projects in it. This can be summarized as follows:

- Create Azure Function project
- Create solution file
- Add Function proj to solution file
- Create two class library projs to hold entities and managers for our missions
- Add the class library projs to solution file
- Test that everything looks OK!

### Weapons 🗡🔫

In order to follow this tutorial your machine must have:

- Visual Studio Code: this is our IDE for this demo
- [Azure Functions](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions) and [Azure Account](https://marketplace.visualstudio.com/items?itemName=ms-vscode.azure-account) VS Code extensions
- Azure Storage Emulator: direct link to download [here](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409)
- .NET Core 3.1 sdk: u can download from [here](https://dotnet.microsoft.com/download/dotnet/3.1)
- Azure Functions Core Tools: u can download from this [repo](https://github.com/Azure/azure-functions-core-tools)

### Prepare the function project

The first thing u need to do is to create a containing folder for this serverless solution. I called it "start". Once you'have created it you have to cd (change directory command) in and issue the statement to create a solution file in your terminal:

```
dotnet new sln
```

This command we'll create a file called "start.sln" in my case as it depends on the name of the containing folder (unless u explicitly set the name for the sln file through the use of the flag "-n" or "--name" ).
Now it's time to create the Azure Function proj.  
In "start" create a folder named "azure-function" (I love this naming convention 😍).  
Now u must go to the Azure Functions extension and select the folder with a little thunderbolt to its right (u can do this even if u are not logged into any Azure Subscription).
<br>
<br>
<br>

<p align="center">
    <img src="./img/azure-func-ext.png" alt="Azure Function Extension" width="450px" />  
</p>

After that u pressed it a wizard will guide u through the process of getting started with your first Azure Function (please be sure to select the "azure-function" directory that should be empty).  
I took these decisions (in the Visual Studio Code command palette):

- Select a language => C#
- Select a .NET runtime => .NET Core 3 LTS
- Select a template for your project's first function => Timer Trigger
- Provide a function name => TimerTriggerFunc
- Provide a namespace => azure_function
- Enter a cron expression of the format '{second} {minute} {hour} {day} {month} {day of week}' to specify the schedule => leave the default as we'll change it soon

Warning: At this moment a little popup will appear on the screen, please be sure to select "Use local emulator".  
When Visual Studio Code has finished the project's creation it will prompt u to restore the unresolved dependencies, select the "Restore" button on the notification and let it finishes to prepare our boilerplate template codebase to work with.  
Before giving it a try we fix the schedule of our timer trigger.  
Open the TimerTriggerFunc.cs file and change the TimerTrigger from "0 /5 \* \* \*" to "\*/5 \* \* \* \* \*". With this change our Azure Function will run every 5 seconds so it will run on each second that is divisible by 5. This is a CRONTAB expression. U can learn more about them by visiting [this](https://crontab.guru/) website.  
Once you changed this you can run the function in order to check if everything looks fine.  
Important: please be sure that the Local Storage Emulator is up and running otherwise u're not able to run your function. If u missed this step u should have the following message:

<p align="center">
    <img src="./img/local-storage-stopped.png" alt="Local Storage stopped" width="650px"/>
</p>

If u encounter this issue u must search for "Azure Storage Emulator" in the start menu and open it. A shell like this one should appear on your machine:

<p align="center">
    <img src="./img/local-storage-shell.png" alt="Local Storage shell" width="750px"/>
</p>

Press F5 and BOOOOM 🤩. Your function is up and running and every 5 seconds u should see in the console a message like this one:

<p align="center">
    <img src="./img/function-executions-log.png" alt="Azure Function logs" width="1050px"/>
</p>

### Setup the other projs

Once we tested successfully that the boilerplate is doing well we must setup all the remaining projects and the solution file.  
First add the function proj to the "start.sln" by issuing this command (make sure to be in "start" folder):

```
dotnet sln .\start.sln add .\azure-function\azure-function.csproj
```

After that we must add two projects: one for entities and one for managers.
Note: if the creation of other two projects in a small function like this seems to be a bit useless... you're right! Anyway I'd like to show u how to use class library projects in your function proj. Maybe, in your real-life scenario this approach fits well and u can reuse this tutorial to carry out your needs.  
In "start" folder create a folder named "azure-function-entities", in your terminal u can type:

```
mkdir azure-function-entities
```

Now navigate into the newly created folder. And issue the following command to create a class library project:

```
dotnet new classlib
```

Remove the auto-generated Class1.cs file and make this change in the .csproj file:

```
<TargetFramework>netstandard2.1</TargetFramework>
```

This is done mainly because we're not yet using the .NET 5.0 stuff as our func project is still .NET Core 3.1 so we must stick with .NET Standard 2.1 within our .csproj file.  
Now we must add the class library proj to our solution file (warning: issue this command in the start folder):

```
dotnet sln .\start.sln add .\azure-function-entities\azure-function-entities.csproj
```

Repeat these steps also for another project called "azure-function-managers".  
Now u can re-rerun the function in order to check that we've not introduced any errors with these changes. Press F5 again et voilà 🤩. Our Azure function is up and running as before.

## Conclusion

Congratulations 👏🏻❗ U have just completed the first mission.  
Now you are able to create an Azure Function, debug and run it within Visual Studio Code. U have also learned how to create and manage a solution, how to create new projects based upon the .NET templates and how to add them to a solution through the use of the dotnet CLI.  
Now u are ready to go to the next mission (stay updated as it will be rolled out soon). In this mission we're going to setup an IOC container in order to resolve and provide the dependencies at runtime. We'll also make use of the .NET Core User Secrets and I'll show you other amazing stuff.  
Hint: don't miss it for any reasons 🐱‍💻

I hope you enjoy this post and find it useful. If you have any questions or you want to spot me some errors I really appreciate it and I'll make my best to follow up. If you enjoy it and would like to sustain me consider giving a like and sharing on your favorite socials.
Stay safe and see you soon! 😎
