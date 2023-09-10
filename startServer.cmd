@echo off
docker rm -f chathub_sqlserver 2> nul
docker run -d -p 11433:1433 --name chathub_sqlserver -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SqlServer2019" mcr.microsoft.com/azure-sql-edge
IF ERRORLEVEL 1 GOTO error
dotnet build ChatHubProject\src\ChatHubProject.Webapi --no-incremental --force
dotnet watch run -c Debug --project ChatHubProject\src\ChatHubProject.Webapi
GOTO end

:error
PAUSE
:end