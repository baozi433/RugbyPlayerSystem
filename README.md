# Rugby Player Management System Overview

The Rugby player syetem repo has three projects: RugbyPlayerSystem.API, RugbyPlayerSystem.Models, RugbyPlayerSystem.Test.\
The backend is RugbyPlayerSystem.API(ASP.NET Core Web API).

In this Github repos, there is only one main branch(master). The code is built using visual studio 2022, click the .sln file to access the source code to build and run.\

## RugbyPlayerSystem.API
This project is an ASP.NET Core Web API using .NET 7 Framework which references RugbyPlayerSystem.Models project and used the following packages:

### Packages:
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Design
* Microsoft.EntityFrameworkCore.SqlServer
* Microsoft.EntityFrameworkCore.Tools
* Swashbuckle.AspNetCore

This project is using EntityFrameworkCore this ORM for data operation, Swashbuckle.AspNetCore is for api test UI.

### Folder structure
I used a repository design pattern with the following structure:
* Controllers: player and team API controllers
* Data: RugbyDbContext sql server setup and configuration, with defult data once the database(MS SQL database) is setup
* Repository: PlayerReopository, TeamRepository, and with relevant contracts for data access through designed methods

### Run
This project is launched as a single startup project. All the Apis can be tested through swagger.\
Before build and run, please check the following setup:
* Make sure the EntityFrameworkCore related packages(Mentioned above) have been installed with same version. I am suing 7.0.3.
* Check your Sql Sever connect string in /data/RugbyDbContext, you need to create a database name "RugbySystem" or you can change to other name.

To setup the database and create relevant tables run the following code in Package Manager Console:

```
dir
cd RugbyPlayerSystem.API
dotnet ef migrations add Initial
dotnet ef database update
```
So, the database and tables have been setup, you can check the database either from visual studio or MS SQL server management studio to access the data.
In the visual studio, click start to run.

## RugbyPlayerSystem.Models
This project is a Class Library using .NET 7 Framework targets .NET or .NET Standard.
There are three models: Player, Team, and Union. This project cannot be launched as a single startup project as it is a library.

## RugbyPlayerSystem.Test
This is a xUnit test projects using packages FakeItEasy for unit test

