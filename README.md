# ChatHub

Started on 7. September 2023

## Description
The ChatHub project is a web application based on Discord with already known features.

## Features
* Chatting with friends in private or in groups with Accounts / Admin
* Call friends
* More features are tbd

## Team members
| Name                        | Email                    | Task   | 
| --------------------------- | ------------------------ | ------ |
| Mohamad *Aldulemi*, 4CHIF   | alu22162@spengergasse.at | tba    |
| Matija *Radomirovic*, 4CHIF | rad22669@spengergasse.at | tba    |
| Richard *Liu*, 4CHIF        | liu22291@spengergasse.at | tba    |
| Ömer *Toprak*, 4CHIF        | top22166@spengergasse.at | tba    |

## Info 4 Team members
1. Add *appsettings.Development.json* to ChatHubProject.Webapi
```
{
  "ConnectionStrings": {
    "Default": "Server=127.0.0.1,11433;Initial Catalog=ChatHubDb;User Id=sa;Password=SqlServer2019;TrustServerCertificate=true"
  },
  "Secret": "JEKEXCOywg+FqXOSDnmOk/GixQo+xi16hQjehDF46vE4DxgHdbsiR7JpXELO831Z6n9T32mgqc4W4S2sjtNESeHNp4KyYMPcclsyhDuRNxXX4RiOBnrHrc5TTuYQSqNJfUW691i2eu7KvvWpn8JftfHU3NjH+TccklFjPBy7k28=",
  "Searchuser": "",
  "Searchpass": "",
  "LocalAdmins": "",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
2. Add Container with this:
```
startServer.cmd
```
## Synchronize the repository to a folder
Run the following command to download the repository:
```
git clone https://github.com/Momo00o0/ChatHub.git
```
> Install the latest version of [git](https://git-scm.com/downloads) with the default settings.

Run the following command to download the latest version from the server:
**Windows**
```
resetGit.cmd
```

**macOS, Linux**
```
chmod a+x resetGit.sh
./resetGit.sh
```
> **Warning:** All local changes will be reset.

### Running the server
**Windows**
```
startServer.cmd
```

**macOS, Linux**
```
chmod a+x startServer.sh
./startServer.sh
```
