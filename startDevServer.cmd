:start
dotnet build ChatBombProject.Webapi --no-incremental --force
dotnet watch run -c Debug --project ChatBombProject.Webapi 
goto start