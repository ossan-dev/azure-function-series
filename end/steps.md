## setup
1. copy "start" folder
1. rename it to "end"
1. rename sln file from "start.sln" to "end.sln"
1. open "end" folder with VS Code
1. if u have a .vscode folder one level above
    1. open tasks.json and update the references to "start" folder by using "end" one
1. open terminal and check if compile

## entities & managers projs
1. open azure-function-entities.proj
1. change TargetFramework from "netstandard2.1" to "net5.0"
1. open "Output" under "models" and change the class to a record
1. compile this proj singularly
1. open azure-function-managers.proj
1. change TargetFramework from "netstandard2.1" to "net5.0"
1. compile this proj singularly (it also will compile the ref proj)

## azure function proj

### azure-function.proj
1. open azure-function.proj
1. change TargetFramework from "netcoreapp3.1" to "net5.0"
1. IMPORTANT: right below "AzureFunctionsVersion" setting add `<OutputType>Exe</OutputType>`
1. upgrade "ItemGroup" section of nuget packages with the new ones
1. issue a "dotnet restore" command within this proj in order to download the newer package versions

### local.settings.json
1. change FUNCTIONS_WORKER_RUNTIME from "dotnet" to "dotnet-isolated"

### Startup.cs
1. delete this file

### Program.cs
1. create a Program.cs file under "azure-function" folder
1. add the code for the class
1. warning: check the namespaces

### TimerTriggerFunc.cs
1. change the using statements at the top of the file
1. change the signature of Run()
    1. Change attribute from "FunctionName" to "Function"
    1. replace "ILogger log" with "FunctionContext context"
1. change the body of the Run()
    1. note the logger
    1. the target-type new expression of C# 9.0
    1. the use of records
1. add a class inside right below the Run() for the type "TimerInfo"

#### TimerInfo & ScheduleStatus
1. paste the code

## final check
1. dotnet build "azure-function.proj"
1. make sure the local storage emulator has been started
1. press F5 and enjoy
