:start
dotnet build ChatHubProject.Webapi --no-incremental --force
dotnet watch run -c Debug --project ChatHubProject.Webapi 
goto start
