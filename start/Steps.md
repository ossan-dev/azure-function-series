1. create "start" folder
1. push to origin
1. in start folder create sln file
1. `dotnet new sln`
1. in start folder create azure-function in order to contain the azure function proj
1. with function ext create func in above folder
1. use this crontab expression "\*/5 \* \* \* \*"
1. run the azure func with F5
1. git push
1. add func proj to sln file
1. cd into folder with sln file
1. `dotnet sln .\start.sln add .\azure-function\azure-function.csproj`
1. in "start" add "azure-function-manager"
1. cd in the newly created folder
1. `dotnet new classlib`
1. `dotnet sln .\start.sln add .\azure-function-manager\azure-function-manager.csproj`
1.
